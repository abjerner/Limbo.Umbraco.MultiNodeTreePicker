using Limbo.Umbraco.MultiNodeTreePicker.Composers;
using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Skybrud.Essentials.Collections.Extensions;
using Skybrud.Essentials.Json.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core;
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

    private static readonly string[] _versionToken = { ", Version=" };

    #region Constructors

    private readonly IMemberService _memberService;
    private readonly MntpTypeConverterCollection _typeConverterCollection;
    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;
    private readonly MntpConverterCollection _itemConverterCollection;

    public MntpValueConverter(IPublishedSnapshotAccessor publishedSnapshotAccessor, IUmbracoContextAccessor umbracoContextAccessor, IMemberService memberService, MntpTypeConverterCollection typeConverterCollection, MntpConverterCollection itemConverterCollection) : base(publishedSnapshotAccessor, umbracoContextAccessor, memberService) {
        _publishedSnapshotAccessor = publishedSnapshotAccessor;
        _memberService = memberService;
        _typeConverterCollection = typeConverterCollection;
        _itemConverterCollection = itemConverterCollection;
    }

    #endregion

    #region Member methods

    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias.Equals(MntpEditor.EditorAlias);
    }

    public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
        return PropertyCacheLevel.Snapshot;
    }

    public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) {
        return source?.ToString()?
            .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(UdiParser.Parse)
            .ToArray();
    }

    public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel cacheLevel, object? source, bool preview) {

        // Get the picked items as IPublishedContent
        IEnumerable<IPublishedContent> value = GetPickerValue(propertyType, source, preview);

        // Return "value" if the data type isn't configured with an item converter
        if (propertyType.DataType.Configuration is not MntpConfiguration config) return value;

        // Get the key of the converter
        string? key = GetItemConverterKey(config.ItemConverter);
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
                .Cast(type)
                .ToList(type);

        }

        // In theory we shouldn't reach this point, but if we do, we return the list of IPublishedContent if the
        // picker is configured as a multi picker, or the first item if the picker is configured as a single picker
        return config.IsSinglePicker ? value.FirstOrDefault() : value;

    }

    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

        bool single = IsSingleNodePicker(propertyType);

        if (propertyType.DataType.Configuration is MntpConfiguration { ItemConverter: { } } config) {

            string? key = GetItemConverterKey(config.ItemConverter);

            if (!string.IsNullOrWhiteSpace(key) && _typeConverterCollection.TryGet(key, out IMntpTypeConverter? typeConverter)) {

                return typeConverter.GetType(propertyType, config);

            }

            if (!string.IsNullOrWhiteSpace(key) && _itemConverterCollection.TryGet(key, out IMntpItemConverter? converter)) {

                Type type = converter.GetType(propertyType);

                return single ? type : typeof(IEnumerable<>).MakeGenericType(type);

            }

        }

        return single ? typeof(IPublishedContent) : typeof(IEnumerable<IPublishedContent>);

    }

    private IReadOnlyList<IPublishedContent> GetPickerValue(IPublishedPropertyType propertyType, object? source, bool preview) {

        if (source == null) return Array.Empty<IPublishedContent>();

        Udi[] udis = source as Udi[] ?? Array.Empty<Udi>();
        if (propertyType.Alias.Equals(Constants.Conventions.Content.InternalRedirectId)) return Array.Empty<IPublishedContent>();
        if (propertyType.Alias.Equals(Constants.Conventions.Content.Redirect)) return Array.Empty<IPublishedContent>();

        // Get a reference to the current published snapshot
        _publishedSnapshotAccessor.TryGetPublishedSnapshot(out IPublishedSnapshot? publishedSnapshot);
        if (publishedSnapshot == null) return Array.Empty<IPublishedContent>();

        // Is the data type configured as a single picker?
        bool single = IsSingleNodePicker(propertyType);

        // Initialize a new list for the items
        List<IPublishedContent> items = new();

        foreach (Udi udi in udis) {

            // Make sure we have a GUID UDI
            GuidUdi? guidUdi = udi as GuidUdi;
            if (guidUdi == null) continue;

            IPublishedContent? item = null;

            switch (udi.EntityType) {
                case Constants.UdiEntityType.Document:
                    item = publishedSnapshot.Content?.GetById(preview, guidUdi.Guid);
                    break;
                case Constants.UdiEntityType.Media:
                    item = publishedSnapshot.Media?.GetById(preview, guidUdi.Guid);
                    break;
                case Constants.UdiEntityType.Member:
                    item = GetMemberByGuidUdi(guidUdi, publishedSnapshot);
                    break;
            }

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
        return propertyType.DataType.ConfigurationAs<MultiNodePickerConfiguration>()!.MaxNumber == 1;
    }

    private IPublishedContent? GetMemberByGuidUdi(GuidUdi udi, IPublishedSnapshot snapshot) {
        IMember? member = _memberService.GetByKey(udi.Guid);
        return member == null ? null : snapshot.Members?.Get(member);
    }

    private static string? GetItemConverterKey(JToken? token) {
        return token switch {
            null => null,
            JObject obj => obj.GetString("key"),
            _ => token.Type switch {
                JTokenType.String => token.ToString().Split(_versionToken, StringSplitOptions.None)[0],
                _ => null
            }
        };
    }

    #endregion

}