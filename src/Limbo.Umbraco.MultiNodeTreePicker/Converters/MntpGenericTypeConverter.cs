using System;
using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;
using Skybrud.Essentials.Collections.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Limbo.Umbraco.MultiNodeTreePicker.Converters;

/// <summary>
/// Generic type converter implementing the <see cref="IMntpTypeConverter"/> interface.
///
/// The type converter works by specifying a callback method which then will be used for converting the items.
/// </summary>
/// <typeparam name="T">The type of the items.</typeparam>
public abstract class MntpGenericTypeConverter<T> : IMntpTypeConverter {

    #region Properties

    /// <summary>
    /// Gets the friendly name of the item converter.
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Gets the icon of the item converter.
    /// </summary>
    public string? Icon { get; protected set; }

    /// <summary>
    /// Gets a reference to the callback function used for converting the items.
    /// </summary>
    protected Func<IPublishedContent, T> Callback { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="name"/> and <paramref name="callback"/>.
    /// </summary>
    /// <param name="name">The friendly name of the item converter.</param>
    /// <param name="callback">The callback function used for converting the items.</param>
    protected MntpGenericTypeConverter(string name, Func<IPublishedContent, T> callback) {
        Name = name;
        Callback = callback;
    }

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="name"/> and <paramref name="callback"/>.
    /// </summary>
    /// <param name="name">The friendly name of the item converter.</param>
    /// <param name="icon">The icon of the converter.</param>
    /// <param name="callback">The callback function used for converting the items.</param>
    protected MntpGenericTypeConverter(string name, string icon, Func<IPublishedContent, T> callback) {
        Name = name;
        Icon = icon;
        Callback = callback;
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
    public object? Convert(IPublishedElement owner, IPublishedPropertyType propertyType, IEnumerable<IPublishedContent> source, MntpConfiguration config, bool preview) {

        // Handle if the picker is configured as a single picker
        if (config.IsSinglePicker) return source.FirstOrDefault() is {} first ? Callback(first) : null;

        // Get the type of each time
        Type itemType = typeof(T);

        // Convert the items
        return source
            .Select(Callback)
            .Cast(itemType)
            .ToList(itemType);

    }


    /// <summary>
    /// Returns the <see cref="Type"/> of the items returned by this item converter.
    /// </summary>
    /// <param name="propertyType">The property type.</param>
    /// <param name="config">The configuration of the multinode treepicker.</param>
    /// <returns>The <see cref="Type"/> of the value returned by this type converter.</returns>
    public Type GetType(IPublishedPropertyType propertyType, MntpConfiguration config) {
        return config.IsSinglePicker ? typeof(T) : typeof(IEnumerable<>).MakeGenericType(typeof(T));
    }

    #endregion

}