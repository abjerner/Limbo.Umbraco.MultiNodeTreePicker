using System;
using System.Collections.Generic;
using Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Limbo.Umbraco.MultiNodeTreePicker.Converters;

/// <summary>
/// Interface describing a type converter.
/// </summary>
public interface IMntpTypeConverter : IMntpConverter {

    /// <summary>
    /// Returns the converted value based on <paramref name="source"/>.
    /// </summary>
    /// <param name="owner">The <see cref="IPublishedElement"/> or <see cref="IPublishedContent"/> the property belongs to.</param>
    /// <param name="propertyType">The property type.</param>
    /// <param name="source">The source <see cref="IPublishedContent"/>.</param>
    /// <param name="config">The configuration of the multi node tree picker.</param>
    /// <param name="preview"></param>
    /// <returns>The converted item.</returns>
    object? Convert(IPublishedElement owner, IPublishedPropertyType propertyType, IEnumerable<IPublishedContent> source, MntpConfiguration config, bool preview);

    /// <summary>
    /// Returns the <see cref="Type"/> of the items returned by this item converter.
    /// </summary>
    /// <param name="propertyType">The property type.</param>
    /// <param name="config">The configuration of the multi node tree picker.</param>
    /// <returns>The <see cref="Type"/> of the items returned by this item converter.</returns>
    Type GetType(IPublishedPropertyType propertyType, MntpConfiguration config);

}