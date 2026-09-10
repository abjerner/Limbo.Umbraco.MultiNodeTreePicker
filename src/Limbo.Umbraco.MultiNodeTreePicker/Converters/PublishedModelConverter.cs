using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;
using Skybrud.Essentials.Enums;
using Skybrud.Essentials.Strings;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Extensions;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MultiNodeTreePicker.Converters;

/// <summary>
/// Converter ensuring that MNTP properties have a value type using the published model instead of
/// <see cref="IPublishedContent"/>. If more than one content type is allowed, the converter will try to find a
/// common type - e.g. based on a shared composition. If no common type is found, the property value type will still
/// be <see cref="IPublishedContent"/>.
/// </summary>
public class PublishedModelConverter : IMntpItemConverter {

    private readonly TypeLoader _typeLoader;
    private readonly IPublishedContentTypeCache _publishedContentTypeCache;

    #region Properties

    public string Name => "Published Model Converter";

    public string Icon => "icon-box";

    #endregion

    #region Constructors

    public PublishedModelConverter(TypeLoader typeLoader, IPublishedContentTypeCache publishedContentTypeCache) {
        _typeLoader = typeLoader;
        _publishedContentTypeCache = publishedContentTypeCache;
    }

    #endregion

    #region Member methods

    /// <inheritdoc/>
    public object? Convert(IPublishedPropertyType propertyType, IPublishedContent? source) {
        return source;
    }

    /// <inheritdoc/>
    public Type GetType(IPublishedPropertyType propertyType) {

        // Ensure the configuration is of the correct type (probably always is)
        if (propertyType.DataType.ConfigurationObject is not MntpConfiguration config) throw new Exception("NOES!");

        // Get the allowed content types from the configuration
        HashSet<string> allowedTypes = StringUtils.ParseStringSet(config.Filter);

        // Set the default type (will be used as fallback
        Type type = typeof(IPublishedContent);

        // Determine a common value type if the data type is restricted to one or more types
        if (allowedTypes.Count > 0 && GetTypeForAllowedTypes(config, allowedTypes) is { } valueType) type = valueType;

        return type;

    }

    /// <remarks>
    /// The implementation of this method is based on a similar method in Callum's Super Value Converters package.
    /// </remarks>
    /// <see>
    ///     <cref>https://github.com/callumbwhyte/super-value-converters/blob/v9/dev/src/Our.Umbraco.SuperValueConverters/ValueConverters/SuperValueConverterBase.cs#L61</cref>
    /// </see>
    private Type? GetTypeForAllowedTypes(MntpConfiguration config, HashSet<string> allowedTypes) {

        // Calculate a list of all the allowed model types
        IEnumerable<Type> types = GetModelTypes(config, allowedTypes);

        // If the data type only has one allowed type, we can return that type right away
        if (allowedTypes.Count == 1) return types.FirstOrDefault();

        return types
            .Select(x => x.GetInterfaces().Where(i => i.IsPublic && i != typeof(IPublishedElement)))
            .IntersectMany()
            .LastOrDefault();

    }

    /// <summary>
    /// Returns a list of all available model types, based on the object type of the specified <paramref name="config"/> and the set of <paramref name="allowedTypes"/>.
    /// </summary>
    /// <param name="config">The MNTP configuration.</param>
    /// <param name="allowedTypes">A set of the allowed types.</param>
    /// <returns></returns>
    private IEnumerable<Type> GetModelTypes(MntpConfiguration config, HashSet<string> allowedTypes) {

        // Parse the object / item type specified in the data type configuration. Ideally we should
        // always be able to parse this, but if we can't, we just return an empty list of types.
        if (!EnumUtils.TryParseEnum(config.TreeSource?.ObjectType, out PublishedItemType objectType)) yield break;

        // First criteria is getting all types that implement IPublishedElement, which is the base interface for all
        // published models. Then we filter those types based on the allowed content types specified in the configuration
        foreach (Type type in _typeLoader.GetTypes<IPublishedElement>()) {

            // ModelsBuilder generated models will expose their content type alias via the PublishedModelAttribute, so
            // we can use that to check if the model is allowed - and whether the type actually has the attribute. If
            // the attribute is missing, we just skip the type
            string? modelAlias = type.GetCustomAttribute<PublishedModelAttribute>(false)?.ContentTypeAlias;
            if (string.IsNullOrWhiteSpace(modelAlias)) continue;

            // Models should also specify their item type (content, media, member) via the ModelItemType field. If the
            // field is missing, we just skip the type
            PublishedItemType itemType = type.GetField("ModelItemType", BindingFlags.Static | BindingFlags.Public)?.GetValue(null) as PublishedItemType? ?? PublishedItemType.Unknown;
            if (itemType != objectType) continue;

            // Legacy configuration of the MNTP data type would save the alias of the allowed content types in the
            // Filter property. So we first check whether the model's alias is allowed
            if (allowedTypes.Contains(modelAlias)) yield return type;

            // If the model's alias is not allowed, we need to check by the key instead, in which case we need to look
            // up the IPublishedContentType for the model. Umbraco is a bit silly here, as it will throw an exception
            // if the content type alias is not found, but doesn't offer a way to check if the content type exists.
            // TODO: should we wrap this in a try/catch?
            if (_publishedContentTypeCache.Get(objectType, modelAlias) is not { } contentType) continue;

            // If the model's content type key is allowed, we can return the type
            if (allowedTypes.Contains($"{contentType.Key:D}")) yield return type;

        }

    }

    #endregion

}

internal static class EnumerableExtensions {
    public static IEnumerable<T> IntersectMany<T>(this IEnumerable<IEnumerable<T>> values) {
        IEnumerable<T>? intersection = null;
        foreach (var value in values) {
            intersection = intersection == null ? new List<T>(value) : intersection.Intersect(value);
        }
        return intersection ?? [];
    }
}