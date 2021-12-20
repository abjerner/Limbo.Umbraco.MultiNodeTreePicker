using System;
using System.Collections.Generic;
using System.Linq;
using Skybrud.Essentials.Collections.Extensions;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Umbraco.MultiNodeTreePicker.Composers;
using Skybrud.Umbraco.MultiNodeTreePicker.Converters;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace Skybrud.Umbraco.MultiNodeTreePicker.PropertyEditors.ValueConverters {

    public class MntpValueConverter : MultiNodeTreePickerValueConverter {

        #region Constructors

        private readonly IMemberService _memberService;
        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;
        private readonly MntpConverterCollection _converterCollection;

        public MntpValueConverter(IPublishedSnapshotAccessor publishedSnapshotAccessor, IUmbracoContextAccessor umbracoContextAccessor, IMemberService memberService, MntpConverterCollection converterCollection) : base(publishedSnapshotAccessor, umbracoContextAccessor, memberService) {
            _publishedSnapshotAccessor = publishedSnapshotAccessor ?? throw new ArgumentNullException(nameof(publishedSnapshotAccessor));
            _memberService = memberService;
            _converterCollection = converterCollection;
        }

        #endregion

        #region Member methods

        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.Equals(MntpEditor.EditorAlias);
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Snapshot;
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview) {
            return source?.ToString()?
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(UdiParser.Parse)
                .ToArray();
        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel cacheLevel, object source, bool preview) {

            // gets the picked items as IPublishedContent
            object value = GetPickerValue(propertyType, source, preview);

            // Return "value" if the data type isn't configured with an item converter
            if (propertyType.DataType.Configuration is not MntpConfiguration config) return value;
            if (config.ItemConverter == null) return value;
            
            // Get the key of the converter
            string key = config.ItemConverter.GetString("key");
            
            // Return "value" if item converter wasn't found
            if (!_converterCollection.TryGet(key, out IMntpItemConverter converter)) return value;
            
            switch (value) {
                
                // If "value" is a single item, we can call the converter directly
                case IPublishedContent item:
                    return converter.Convert(propertyType, item);

                // If "value" is a list of items, we call the converter for each item, and make sure to return a
                // list of the type specified by the converter
                case List<IPublishedContent> items:
                    Type type = converter.GetType(propertyType);
                    return items
                        .Select(x => converter.Convert(propertyType, x))
                        .Cast(type)
                        .ToList(type);

                // If neither of the above cases are matched, we return "value" as a fallback
                default:
                    return value;

            }

        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

            bool single = IsSingleNodePicker(propertyType);

            if (propertyType.DataType.Configuration is MntpConfiguration config && config.ItemConverter != null) {

                string key = config.ItemConverter.GetString("key");

                if (_converterCollection.TryGet(key, out IMntpItemConverter converter)) {

                    Type type = converter.GetType(propertyType);

                    return single ? type : typeof(IEnumerable<>).MakeGenericType(type);

                }

            }

            return single ? typeof(IPublishedContent) : typeof(IEnumerable<IPublishedContent>);

        }

        private object GetPickerValue(IPublishedPropertyType propertyType, object source, bool preview) {
            
            if (source == null) return null;

            Udi[] udis = source as Udi[] ?? Array.Empty<Udi>();
            if (propertyType.Alias.Equals(Constants.Conventions.Content.InternalRedirectId)) return udis.FirstOrDefault();
            if (propertyType.Alias.Equals(Constants.Conventions.Content.Redirect)) return udis.FirstOrDefault();
            
            // Get a reference to the current published snapshot
            _publishedSnapshotAccessor.TryGetPublishedSnapshot(out IPublishedSnapshot publishedSnapshot);
            if (publishedSnapshot == null) return source;

            // Is the data type configured as a single picker?
            bool single = IsSingleNodePicker(propertyType);

            // Initialize a new list for the items
            List<IPublishedContent> items = new List<IPublishedContent>();

            foreach (Udi udi in udis) {

                // Make sure we have a GUID UDI
                GuidUdi guidUdi = udi as GuidUdi;
                if (guidUdi == null) continue;

                IPublishedContent item = null;
                
                switch (udi.EntityType) {
                    case Constants.UdiEntityType.Document:
                        item = publishedSnapshot.Content.GetById(preview, guidUdi.Guid);
                        break;
                    case Constants.UdiEntityType.Media:
                        item = publishedSnapshot.Media.GetById(preview, guidUdi.Guid);
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

            return single ? items.FirstOrDefault() : items;

        }

        private static bool IsSingleNodePicker(IPublishedPropertyType propertyType) {
            return propertyType.DataType.ConfigurationAs<MultiNodePickerConfiguration>().MaxNumber == 1;
        }

        private IPublishedContent GetMemberByGuidUdi(GuidUdi udi, IPublishedSnapshot snapshot) {
            IMember member = _memberService.GetByKey(udi.Guid);
            return member == null ? null : snapshot.Members.Get(member);
        }

        #endregion

    }

}