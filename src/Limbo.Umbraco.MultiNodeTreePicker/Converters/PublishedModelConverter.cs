using System;
using System.Collections.Generic;
using System.Linq;
using Skybrud.Essentials.Strings;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MultiNodeTreePicker.Converters;

/// <summary>
/// Converter ensuring that MNTP properties have a value type using the published model instead of
/// <see cref="IPublishedContent"/>. If more than one content type is allowed, the converter will try to find a
/// common type - eg. based on a shared composition. If no common type is found, the property value type will still
/// be <see cref="IPublishedContent"/>.
/// </summary>
public class PublishedModelConverter : IMntpItemConverter {

    private readonly TypeLoader _typeLoader;

    #region Properties

    public string Name => "Published Model Converter";

    public string Icon => "icon-box";

    #endregion

    #region Constructors

    public PublishedModelConverter(TypeLoader typeLoader) {
        _typeLoader = typeLoader;
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
        if (propertyType.DataType.Configuration is not MultiNodePickerConfiguration config) throw new Exception("NOES!");

        // Get the allowed content types from the configuration
        string[] allowedTypes = StringUtils.ParseStringArray(config.Filter);

        // Set the default type (will be used as fallback
        Type type = typeof(IPublishedContent);

        // Determine a common value type if the data type is restricted to one or more types
        if (allowedTypes.Length > 0 && GetTypeForAllowedTypes(allowedTypes) is { } valueType) type = valueType;

        return type;

    }

    /// <remarks>
    /// The implementation of this method is based on a similar method in Callum's Super Value Converters package.
    /// </remarks>
    /// <see>
    ///     <cref>https://github.com/callumbwhyte/super-value-converters/blob/v9/dev/src/Our.Umbraco.SuperValueConverters/ValueConverters/SuperValueConverterBase.cs#L61</cref>
    /// </see>
    private Type? GetTypeForAllowedTypes(string[] allowedTypes) {

        // Calculate a list of all the allowed model types
        IEnumerable<Type> types = _typeLoader.GetTypes<IPublishedElement>().Where(type => {
            string modelName = type.GetCustomAttribute<PublishedModelAttribute>(false)?.ContentTypeAlias ?? type.Name;
            return allowedTypes.InvariantContains(modelName);
        });

        // If the data type only has one allowed type, we can return that type right away
        if (allowedTypes.Length == 1) return types.FirstOrDefault();

        return types
            .Select(x => x.GetInterfaces().Where(i => i.IsPublic && i != typeof(IPublishedElement)))
            .IntersectMany()
            .LastOrDefault();

    }

    #endregion

}

internal static class EnumerableExtensions {
    public static IEnumerable<T> IntersectMany<T>(this IEnumerable<IEnumerable<T>> values) {
        IEnumerable<T>? intersection = null;

        foreach (var value in values) {
            intersection = intersection == null ? new List<T>(value) : intersection.Intersect(value);
        }

        return intersection ?? Enumerable.Empty<T>();
    }
}