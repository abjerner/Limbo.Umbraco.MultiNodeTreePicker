using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.MultiNodeTreePicker.Converters {

    public abstract class MntpGenericItemConverter<T> : IMntpItemConverter {

        public string Name { get; }

        protected Func<IPublishedContent, T> Callback { get; }

        protected MntpGenericItemConverter(string name, Func<IPublishedContent, T> callback) {
            Name = name;
            Callback = callback;
        }

        public virtual object Convert(IPublishedPropertyType propertyType, IPublishedContent source) {
            return source is null ? null : Callback(source);
        }

        public virtual Type GetType(IPublishedPropertyType propertyType) {
            return typeof(T);
        }

    }

}