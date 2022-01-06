using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.MultiNodeTreePicker.Converters {

    public abstract class MntpItemConverterBase<T> : IMntpItemConverter {

        public string Name { get; }

        protected MntpItemConverterBase(string name) {
            Name = name;
        }

        public abstract object Convert(IPublishedPropertyType propertyType, IPublishedContent source);

        public virtual Type GetType(IPublishedPropertyType propertyType) {
            return typeof(T);
        }

    }

}