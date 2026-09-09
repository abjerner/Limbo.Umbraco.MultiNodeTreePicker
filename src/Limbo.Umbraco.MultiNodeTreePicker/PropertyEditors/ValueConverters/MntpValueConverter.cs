using System;
using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.MultiNodeTreePicker.Composers;
using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Skybrud.Essentials.Collections.Enumerables.Extensions;
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

    private readonly IMemberService _memberService;
    private readonly IPublishedContentCache _contentCache;
    private readonly IPublishedMediaCache _mediaCache;
    private readonly IPublishedMemberCache _memberCache;
    private readonly MntpTypeConverterCollection _typeConverterCollection;
    private readonly MntpConverterCollection _itemConverterCollection;

    #region Constructors

    public MntpValueConverter(IUmbracoContextAccessor umbracoContextAccessor, IMemberService memberService, IApiContentBuilder apiContentBuilder, IApiMediaBuilder apiMediaBuilder, IPublishedContentCache contentCache, IPublishedMediaCache mediaCache, IPublishedMemberCache memberCache, MntpTypeConverterCollection typeConverterCollection, MntpConverterCollection itemConverterCollection) : base(umbracoContextAccessor, memberService, apiContentBuilder, apiMediaBuilder, contentCache, mediaCache, memberCache) {
        _memberService = memberService;
        _contentCache = contentCache;
        _mediaCache = mediaCache;
        _memberCache = memberCache;
        _typeConverterCollection = typeConverterCollection;
        _itemConverterCollection = itemConverterCollection;
    }

    #endregion

    #region Member methods

    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias.Equals(MntpEditor.EditorAlias);
    }

    /// <inheritdoc />
    public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {

        // Default to "Elements" if configuration doesn't match (probably wouldn't happen)
        if (propertyType.DataType.ConfigurationObject is not MntpConfiguration config) return PropertyCacheLevel.Elements;

        // "Snapshot" no longer exists as a distinct cache level, so data types upgraded from v13 with "Snapshot"
        // selected are treated as "Elements" (which is also the default if nothing is selected)
        return config.CacheLevel switch {
            PropertyCacheLevel.Element => PropertyCacheLevel.Element,
            PropertyCacheLevel.None => PropertyCacheLevel.None,
            _ => PropertyCacheLevel.Elements
        };

    }

    public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) {
        return source?.ToString()?
            .Split(_commaSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => UdiParser.TryParse(x.Trim(), out Udi? udi) ? udi : null)
            .WhereNotNull()
            .ToArray();
    }

    public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel cacheLevel, object? source, bool preview) {

        // Get the picked items as IPublishedContent
        IReadOnlyList<IPublishedContent> value = GetPickerValue(propertyType, source, preview);

        // Return "value" if the data type isn't configured with a converter
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

        if (propertyType.DataType.ConfigurationObject is MntpConfiguration { TypeConverter.Type: { } key } config && !string.IsNullOrWhiteSpace(key)) {

            if (_typeConverterCollection.TryGet(key, out IMntpTypeConverter? typeConverter)) {
                return typeConverter.GetType(propertyType, config);
            }

            if (_itemConverterCollection.TryGet(key, out IMntpItemConverter? converter)) {
                Type type = converter.GetType(propertyType);
                return single ? type : typeof(IReadOnlyList<>).MakeGenericType(type);
            }

        }

        return single ? typeof(IPublishedContent) : typeof(IReadOnlyList<IPublishedContent>);

    }

    private IReadOnlyList<IPublishedContent> GetPickerValue(IPublishedPropertyType propertyType, object? source, bool preview) {

        if (source is not Udi[] udis || udis.Length == 0) return [];

        if (propertyType.Alias.Equals(global::Umbraco.Cms.Core.Constants.Conventions.Content.InternalRedirectId)) return [];
        if (propertyType.Alias.Equals(global::Umbraco.Cms.Core.Constants.Conventions.Content.Redirect)) return [];

        // Is the data type configured as a single picker?
        bool single = IsSingleNodePicker(propertyType);

        // Initialize a new list for the items
        List<IPublishedContent> items = [];

        foreach (Udi udi in udis) {

            // Make sure we have a GUID UDI
            if (udi is not GuidUdi guidUdi) continue;

            IPublishedContent? item = udi.EntityType switch {
                global::Umbraco.Cms.Core.Constants.UdiEntityType.Document => _contentCache.GetById(preview, guidUdi.Guid),
                global::Umbraco.Cms.Core.Constants.UdiEntityType.Media => _mediaCache.GetById(preview, guidUdi.Guid),
                global::Umbraco.Cms.Core.Constants.UdiEntityType.Member => GetMemberByGuidUdi(guidUdi),
                _ => null
            };

            // Continue to the next UDI if "item" is either null or an element type
            if (item == null) continue;
            if (item.ItemType == PublishedItemType.Element) continue;

            // Append the item to the list
            items.Add(item);

            // If the data type is configured as a single picker, we break the loop as we don't really need to
            // look up any additional items that may be picked
            if (single) break;

        }

        return items;

    }

    private static bool IsSingleNodePicker(IPublishedPropertyType propertyType) {
        return propertyType.DataType.ConfigurationObject is MultiNodePickerConfiguration { MaxNumber: 1 };
    }

    private IPublishedContent? GetMemberByGuidUdi(GuidUdi udi) {
        IMember? member = _memberService.GetById(udi.Guid);
        return member == null ? null : _memberCache.Get(member);
    }

    #endregion

}
