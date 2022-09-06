using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Limbo.Umbraco.MultiNodeTreePicker.Converters {

    /// <summary>
    /// Generic item converter base implementing the <see cref="IMntpItemConverter"/>.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    public abstract class MntpItemConverterBase<T> : IMntpItemConverter {
        
        /// <summary>
        /// Gets the friendly name of the item converter.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The friendly name of the item converter.</param>
        protected MntpItemConverterBase(string name) {
            Name = name;
        }
        
        /// <summary>
        /// Returns the converted item based on <paramref name="source"/>.
        /// </summary>
        /// <param name="propertyType">The property type.</param>
        /// <param name="source">The source <see cref="IPublishedContent"/>.</param>
        /// <returns>The converted item.</returns>
        public abstract object Convert(IPublishedPropertyType propertyType, IPublishedContent source);
        
        /// <summary>
        /// Returns the <see cref="Type"/> of the items returned by this item converter.
        /// </summary>
        /// <param name="propertyType">The property type.</param>
        /// <returns>The <see cref="Type"/> of the items returned by this item converter.</returns>
        public virtual Type GetType(IPublishedPropertyType propertyType) {
            return typeof(T);
        }

    }

}