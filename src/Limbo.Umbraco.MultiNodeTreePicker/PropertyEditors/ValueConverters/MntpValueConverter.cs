using System;
using System.Collections.Generic;
using System.Linq;
using Limbo.Essentials.Collections.Extensions;
using Limbo.Essentials.Json.Extensions;
using Limbo.Umbraco.MultiNodeTreePicker.Composers;
using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Composing;
using Umbraco.Web.PropertyEditors;
using Umbraco.Web.PropertyEditors.ValueConverters;
using Umbraco.Web.PublishedCache;

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors.ValueConverters {
    
    public class MntpValueConverter : MultiNodeTreePickerValueConverter {

        #region Constructors

        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

        private readonly MntpConverterCollection _converterCollection;

        public MntpValueConverter(IPublishedSnapshotAccessor publishedSnapshotAccessor, MntpConverterCollection converterCollection) : base(publishedSnapshotAccessor) {
            _publishedSnapshotAccessor = publishedSnapshotAccessor ?? throw new ArgumentNullException(nameof(publishedSnapshotAccessor));
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
            return source?.ToString()
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(Udi.Parse)
                .ToArray();
        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel cacheLevel, object source, bool preview) {

            object value = Bah(owner, propertyType, cacheLevel, source, preview);

            bool single = IsSingleNodePicker(propertyType);

            if (propertyType.DataType.Configuration is MntpConfiguration config && config.ItemConverter != null) {

                string key = config.ItemConverter.GetString("key");

                if (_converterCollection.TryGet(key, out IMntpItemConverter converter)) {

                    if (value is IPublishedElement element) return converter.Convert(propertyType, element);

                    if (single == false) {

                        Type type = converter.GetType(propertyType);
                        
                        return (value as IEnumerable<IPublishedElement> ?? new IPublishedElement[0])
                            .Select(x => converter.Convert(propertyType, x))
                            .Cast(type)
                            .ToList(type);

                    }

                }

            }

            return value;

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

        private IPublishedContent GetPublishedContent<T>(T nodeId, ref UmbracoObjectTypes actualType, UmbracoObjectTypes expectedType, Func<T, IPublishedContent> contentFetcher) {
            
            // is the actual type supported by the content fetcher?
            if (actualType != UmbracoObjectTypes.Unknown && actualType != expectedType) {
                // no, return null
                return null;
            }

            // attempt to get the content
            var content = contentFetcher(nodeId);
            if (content != null)
            {
                // if we found the content, assign the expected type to the actual type so we don't have to keep looking for other types of content
                actualType = expectedType;
            }
            return content;
        }

        public object Bah(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel cacheLevel, object source, bool preview) {

            if (source == null) return null;

            if (Current.UmbracoContext == null) return source;

            Udi[] udis = source as Udi[] ?? new Udi[0];

            if (propertyType.Alias.Equals(Constants.Conventions.Content.InternalRedirectId)) return udis.FirstOrDefault();
            if (propertyType.Alias.Equals(Constants.Conventions.Content.Redirect)) return udis.FirstOrDefault();

            bool single = IsSingleNodePicker(propertyType);

            List<IPublishedContent> multiNodeTreePicker = new List<IPublishedContent>();

            UmbracoObjectTypes objectType = UmbracoObjectTypes.Unknown;

            foreach (var udi in udis) {

                GuidUdi guidUdi = udi as GuidUdi;
                if (guidUdi == null) continue;

                IPublishedContent multiNodeTreePickerItem = null;
                switch (udi.EntityType) {
                    case Constants.UdiEntityType.Document:
                        multiNodeTreePickerItem = GetPublishedContent(udi, ref objectType, UmbracoObjectTypes.Document, id => _publishedSnapshotAccessor.PublishedSnapshot.Content.GetById(guidUdi.Guid));
                        break;
                    case Constants.UdiEntityType.Media:
                        multiNodeTreePickerItem = GetPublishedContent(udi, ref objectType, UmbracoObjectTypes.Media, id => _publishedSnapshotAccessor.PublishedSnapshot.Media.GetById(guidUdi.Guid));
                        break;
                    case Constants.UdiEntityType.Member:
                        multiNodeTreePickerItem = GetPublishedContent(udi, ref objectType, UmbracoObjectTypes.Member, id => _publishedSnapshotAccessor.PublishedSnapshot.Members.GetByProviderKey(guidUdi.Guid));
                        break;
                }

                if (multiNodeTreePickerItem != null && multiNodeTreePickerItem.ContentType.ItemType != PublishedItemType.Element) {
                    multiNodeTreePicker.Add(multiNodeTreePickerItem);
                    if (single) break;
                }

            }

            if (single) return multiNodeTreePicker.FirstOrDefault();
            return multiNodeTreePicker;

        }

        private static bool IsSingleNodePicker(IPublishedPropertyType propertyType) {
            return propertyType.DataType.ConfigurationAs<MultiNodePickerConfiguration>().MaxNumber == 1;
        }
        
        #endregion

    }

}