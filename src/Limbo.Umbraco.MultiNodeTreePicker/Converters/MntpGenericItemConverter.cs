using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MultiNodeTreePicker.Converters
{

    public abstract class MntpGenericItemConverter<T> : IMntpItemConverter
    {

        public string Name { get; }

        protected Func<IPublishedContent, T> Callback { get; }

        protected MntpGenericItemConverter(string name, Func<IPublishedContent, T> callback)
        {
            Name = name;
            Callback = callback;
        }

        public object Convert(IPublishedPropertyType propertyType, object source)
        {

            if (source is IPublishedContent content) return Callback(content);

            return null;

        }

        public Type GetType(IPublishedPropertyType propertyType)
        {
            return typeof(T);
        }

        public bool IsConverter(IPublishedPropertyType propertyType)
        {
            throw new NotImplementedException();
        }

        public bool? IsValue(object value, PropertyValueLevel level)
        {
            throw new NotImplementedException();
        }

        public Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            throw new NotImplementedException();
        }

        public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            throw new NotImplementedException();
        }

        public object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            throw new NotImplementedException();
        }

        public object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            throw new NotImplementedException();
        }

        public object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            throw new NotImplementedException();
        }
    }

}