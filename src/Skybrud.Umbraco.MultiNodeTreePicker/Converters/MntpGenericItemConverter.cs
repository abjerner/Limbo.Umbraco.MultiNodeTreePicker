using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.MultiNodeTreePicker.Converters {

    /// <summary>
    /// Generic item converter implementing the <see cref="IMntpItemConverter"/>.
    ///
    /// The item converter works by specifying a callback method which then will be used for converting the items.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    public abstract class MntpGenericItemConverter<T> : IMntpItemConverter {
        
        /// <summary>
        /// Gets the friendly name of the item converter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a reference to the callback function used for converting the items.
        /// </summary>
        protected Func<IPublishedContent, T> Callback { get; }

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="name"/> and <paramref name="callback"/>.
        /// </summary>
        /// <param name="name">The friendly name of the item converter.</param>
        /// <param name="callback">The callback function used for converting the items.</param>
        protected MntpGenericItemConverter(string name, Func<IPublishedContent, T> callback) {
            Name = name;
            Callback = callback;
        }
        
        /// <summary>
        /// Returns the converted item based on <paramref name="source"/>.
        /// </summary>
        /// <param name="propertyType">The property type.</param>
        /// <param name="source">The source <see cref="IPublishedContent"/>.</param>
        /// <returns>The converted item.</returns>
        public virtual object Convert(IPublishedPropertyType propertyType, IPublishedContent source) {
            return source is null ? null : Callback(source);
        }
        
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