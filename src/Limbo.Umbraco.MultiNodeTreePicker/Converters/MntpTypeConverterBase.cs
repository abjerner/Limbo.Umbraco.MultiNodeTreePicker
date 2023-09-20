using System;
using Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using System.Linq;
using Skybrud.Essentials.Collections.Extensions;

namespace Limbo.Umbraco.MultiNodeTreePicker.Converters;

/// <summary>
/// Generic type converter base implementing the <see cref="IMntpItemConverter"/>.
/// </summary>
/// <typeparam name="T">The type of the items.</typeparam>
public abstract class MntpTypeConverterBase<T> : IMntpTypeConverter {

    #region Properties

    /// <summary>
    /// Gets the friendly name of the type converter.
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Gets the icon of the type converter.
    /// </summary>
    public string? Icon { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The friendly name of the type converter.</param>
    protected MntpTypeConverterBase(string name) {
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="name"/> and <paramref name="icon"/>.
    /// </summary>
    /// <param name="name">The friendly name of the type converter.</param>
    /// <param name="icon">The icon to be shown for the type converter.</param>
    protected MntpTypeConverterBase(string name, string? icon) {
        Name = name;
        Icon = icon;
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns the converted value based on <paramref name="source"/>.
    /// </summary>
    /// <param name="owner">The <see cref="IPublishedElement"/> or <see cref="IPublishedContent"/> the property belongs to.</param>
    /// <param name="propertyType">The property type.</param>
    /// <param name="source">The source <see cref="IPublishedContent"/>.</param>
    /// <param name="config">The configuration of the multinode treepicker.</param>
    /// <param name="preview"></param>
    /// <returns>The converted value.</returns>
    public virtual object? Convert(IPublishedElement owner, IPublishedPropertyType propertyType, IEnumerable<IPublishedContent> source, MntpConfiguration config, bool preview) {

        // Handle if the picker is configured as a single picker
        if (config.IsSinglePicker) return source.FirstOrDefault() is { } first ? ConvertItem(owner, propertyType, first, config, preview) : null;

        // Convert the list
        return ConvertList(owner, propertyType, source, config, preview);

    }

    /// <summary>
    /// Method responsible for converting a list of <see cref="IPublishedContent"/>.
    /// </summary>
    /// <param name="owner">The <see cref="IPublishedElement"/> or <see cref="IPublishedContent"/> the property belongs to.</param>
    /// <param name="propertyType">The property type.</param>
    /// <param name="source">The source <see cref="IPublishedContent"/>.</param>
    /// <param name="config">The configuration of the multinode treepicker.</param>
    /// <param name="preview"></param>
    /// <returns>The converted value.</returns>
    public virtual object ConvertList(IPublishedElement owner, IPublishedPropertyType propertyType, IEnumerable<IPublishedContent> source, MntpConfiguration config, bool preview) {

        // Get the type of each time
        Type itemType = typeof(T);

        return source
            .Select(x => ConvertItem(owner, propertyType, x, config, preview))
            .ToList(itemType);

    }

    /// <summary>
    /// Method responsible for converting a single <see cref="IPublishedContent"/> to <typeparamref name="T"/>.
    /// </summary>
    /// <param name="owner">The <see cref="IPublishedElement"/> or <see cref="IPublishedContent"/> the property belongs to.</param>
    /// <param name="propertyType">The property type.</param>
    /// <param name="source">The source <see cref="IPublishedContent"/>.</param>
    /// <param name="config">The configuration of the multinode treepicker.</param>
    /// <param name="preview"></param>
    /// <returns>The converted value.</returns>
    public abstract T ConvertItem(IPublishedElement owner, IPublishedPropertyType propertyType, IPublishedContent source, MntpConfiguration config, bool preview);

    /// <summary>
    /// Returns the <see cref="Type"/> of the value returned by this type converter.
    /// </summary>
    /// <param name="propertyType">The property type.</param>
    /// <param name="config">The configuration of the multinode treepicker.</param>
    /// <returns>The <see cref="Type"/> of the value returned by this type converter.</returns>
    public virtual Type GetType(IPublishedPropertyType propertyType, MntpConfiguration config) {
        return config.IsSinglePicker ? typeof(T) : typeof(IEnumerable<>).MakeGenericType(typeof(T));
    }

    #endregion

}