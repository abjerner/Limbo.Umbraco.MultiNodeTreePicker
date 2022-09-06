using Newtonsoft.Json;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Limbo.Umbraco.MultiNodeTreePicker.Converters {

    /// <summary>
    /// Interface describing an item converter.
    /// </summary>
    public interface IMntpItemConverter {

        /// <summary>
        /// Gets the friendly name of the item converter.
        /// </summary>
        [JsonProperty("name")]
        string Name { get; }

        /// <summary>
        /// Gets the icon of the item converter.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon => null;

        /// <summary>
        /// Returns the converted item based on <paramref name="source"/>.
        /// </summary>
        /// <param name="propertyType">The property type.</param>
        /// <param name="source">The source <see cref="IPublishedContent"/>.</param>
        /// <returns>The converted item.</returns>
        object Convert(IPublishedPropertyType propertyType, IPublishedContent source);

        /// <summary>
        /// Returns the <see cref="Type"/> of the items returned by this item converter.
        /// </summary>
        /// <param name="propertyType">The property type.</param>
        /// <returns>The <see cref="Type"/> of the items returned by this item converter.</returns>
        Type GetType(IPublishedPropertyType propertyType);

    }

}