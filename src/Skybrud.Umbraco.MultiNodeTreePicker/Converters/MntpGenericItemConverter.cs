using System;
using Umbraco.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.MultiNodeTreePicker.Converters {
    
    public abstract class MntpGenericItemConverter<T> : IMntpItemConverter {
        
        public string Name { get; }

        protected Func<IPublishedContent, T> Callback { get; }

        protected MntpGenericItemConverter(string name, Func<IPublishedContent, T> callback) {
            Name = name;
            Callback = callback;
        }

        public object Convert(IPublishedPropertyType propertyType, object source) {

            if (source is IPublishedContent content) return Callback(content);

            return null;

        }

        public Type GetType(IPublishedPropertyType propertyType)  {
            return typeof(T);
        }

    }

}