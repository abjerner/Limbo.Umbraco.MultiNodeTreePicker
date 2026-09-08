using System;
using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.MultiNodeTreePicker.Composers;
using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Skybrud.Essentials.Collections.Enumerables.Extensions;
using Skybrud.Essentials.Umbraco.Constants;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

#pragma warning disable 1591

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors.ValueConverters;

public class MntpValueConverter : MultiNodeTreePickerValueConverter {

    private static readonly char[] _commaSeparator = [','];

    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    private readonly IMemberService _memberService;

    private readonly IPublishedContentCache _publishedContentCache;

    private readonly IPublishedMediaCache _publishedMediaCache;

    private readonly IPublishedMemberCache _publishedMemberCache;

    private readonly MntpTypeConverterCollection _typeConverterCollection;

    private readonly MntpConverterCollection _itemConverterCollection;

    #region Constructors

    public MntpValueConverter(IUmbracoContextAccessor umbracoContextAccessor, IMemberService memberService, IApiContentBuilder apiContentBuilder, IApiMediaBuilder apiMediaBuilder, IPublishedContentCache publishedContentCache, IPublishedMediaCache publishedMediaCache, IPublishedMemberCache publishedMemberCache, MntpTypeConverterCollection typeConverterCollection, MntpConverterCollection itemConverterCollection) : base(umbracoContextAccessor, memberService, apiContentBuilder, apiMediaBuilder, publishedContentCache, publishedMediaCache, publishedMemberCache) {
        _umbracoContextAccessor = umbracoContextAccessor;
        _memberService = memberService;
        _publishedContentCache = publishedContentCache;
        _publishedMediaCache = publishedMediaCache;
        _publishedMemberCache = publishedMemberCache;
        _typeConverterCollection = typeConverterCollection;
        _itemConverterCollection = itemConverterCollection;
    }

    #endregion

    #region Member methods

    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias.Equals(MntpPropertyEditor.EditorAlias);
    }

    /// <inheritdoc />
    public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {

        // Default to "Elements" if configuration doesn't match (probably wouldn't happen)
        if (propertyType.DataType.ConfigurationObject is not MntpConfiguration config) return PropertyCacheLevel.Elements;

        // Return the configured cache level (or "Elements" if not specified)
        return config.CacheLevel ?? PropertyCacheLevel.Elements;

    }

    public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) {
        return source?.ToString()?
            .Split(_commaSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Select(UdiParser.Parse)
            .ToArray();
    }

    public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel cacheLevel, object? source, bool preview) {

        // Get the picked items as IPublishedContent
        IReadOnlyList<IPublishedContent> value = GetPickerValue(propertyType, source, preview);

        // Return "value" if the data type isn't configured with an item converter
        if (propertyType.DataType.ConfigurationObject is not MntpConfiguration config) return value;

        // Get the key of the converter
        string? key = config.TypeConverter?.Type;
        if (string.IsNullOrWhiteSpace(key)) return config.IsSinglePicker ? value.FirstOrDefault() : value;

        // Is the selected converter a type converter?
        if (_typeConverterCollection.TryGet(key, out IMntpTypeConverter? typeConverter)) {
            return typeConverter.Convert(owner, propertyType, value, config, preview);
        }

        // Is the selected converter an item converter?
        if (_itemConverterCollection.TryGet(key, out IMntpItemConverter? itemConverter)) {

            // If the multinode treepicker is configured as a single picker, we pick the first
            // item (if any) and run that through the converter
            if (config.IsSinglePicker) {
                IPublishedContent? first = value.FirstOrDefault();
                return itemConverter.Convert(propertyType, first);
            }

            // If configured as a multi picker, we run each item through the converter and return the result as a list
            Type type = itemConverter.GetType(propertyType);
            return value
                .Select(x => itemConverter.Convert(propertyType, x))
                .WhereNotNull()
                .Cast(type)
                .ToList(type);

        }

        // In theory, we shouldn't reach this point, but if we do, we return the list of IPublishedContent if the
        // picker is configured as a multi picker, or the first item if the picker is configured as a single picker
        return config.IsSinglePicker ? value.FirstOrDefault() : value;

    }

    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

        bool single = IsSingleNodePicker(propertyType);

        if (propertyType.DataType.ConfigurationObject is MntpConfiguration { TypeConverter: { } } config) {

            string? key = config.TypeConverter?.Type;

            if (!string.IsNullOrWhiteSpace(key) && _typeConverterCollection.TryGet(key, out IMntpTypeConverter? typeConverter)) {

                return typeConverter.GetType(propertyType, config);

            }

            if (!string.IsNullOrWhiteSpace(key) && _itemConverterCollection.TryGet(key, out IMntpItemConverter? converter)) {

                Type type = converter.GetType(propertyType);

                return single ? type : typeof(IReadOnlyList<>).MakeGenericType(type);

            }

        }

        return single ? typeof(IPublishedContent) : typeof(IReadOnlyList<IPublishedContent>);

    }

    private IReadOnlyList<IPublishedContent> GetPickerValue(IPublishedPropertyType propertyType, object? source, bool preview) {

        if (source == null) return [];
        if (propertyType.Alias.Equals(global::Umbraco.Cms.Core.Constants.Conventions.Content.InternalRedirectId)) return [];
        if (propertyType.Alias.Equals(global::Umbraco.Cms.Core.Constants.Conventions.Content.Redirect)) return [];
        if (!_umbracoContextAccessor.TryGetUmbracoContext(out _)) return [];

        // Get the saved UDIs from the source value, or an empty array if the source value is null
        Udi[] udis = source as Udi[] ?? [];

        // Is the data type configured as a single picker?
        bool single = IsSingleNodePicker(propertyType);

        List<IPublishedContent> list = [];

        foreach (Udi udi in udis) {

            if (udi is not GuidUdi guidUdi) continue;

            IPublishedContent? item = udi.EntityType switch {
                UmbracoEntityTypes.Document => GetContent(preview, guidUdi.Guid),
                UmbracoEntityTypes.Media => GetMedia(preview, guidUdi.Guid),
                UmbracoEntityTypes.Member => GetMember(guidUdi.Guid),
                _ => null
            };

            if (item != null) {
                list.Add(item);
                if (single) break;
            }

        }

        return list;

    }

    private IPublishedContent? GetContent(bool preview, Guid key) {
        return _publishedContentCache.GetById(preview, key) is { ItemType: PublishedItemType.Content } content ? content : null;
    }

    private IPublishedContent? GetMedia(bool preview, Guid key) {
        return _publishedMediaCache.GetById(preview, key) is { ItemType: PublishedItemType.Media } content ? content : null;
    }

    private IPublishedContent? GetMember(Guid key) {

        IMember? m = _memberService.GetById(key);
        if (m == null) return null;

        IPublishedContent? member = _publishedMemberCache.Get(m);
        return member is { ItemType: PublishedItemType.Member } ? member : null;

    }

    private static bool IsSingleNodePicker(IPublishedPropertyType propertyType) {
        return propertyType.DataType.ConfigurationAs<MultiNodePickerConfiguration>()!.MaxNumber == 1;
    }

    #endregion

}