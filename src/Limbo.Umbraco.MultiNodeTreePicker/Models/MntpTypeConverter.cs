using System;
using System.Diagnostics.CodeAnalysis;
using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Limbo.Umbraco.MultiNodeTreePicker.Json.Newtonsoft;
using Newtonsoft.Json;
using Skybrud.Essentials.Exceptions;

namespace Limbo.Umbraco.MultiNodeTreePicker.Models;

/// <summary>
/// Class describing a selected item converter.
/// </summary>
[JsonConverter(typeof(MntpItemConverterJsonConverter))]
public class MntpTypeConverter {

    private static readonly string[] _separator = [", Version"];

    /// <summary>
    /// Gets or sets the alias of the item converter type.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Initializes a new instance with the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The alias of the item converter type.</param>
    [SetsRequiredMembers]
    public MntpTypeConverter(string type) {
        Type = type;
    }

    /// <summary>
    /// Gets the name (identifier) of the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type of the converter.</param>
    /// <returns>The name of the type.</returns>
    protected static string GetTypeName(Type type) {
        if (type.AssemblyQualifiedName == null) throw new ComputerSaysNoException("Not supposed to be null.");
        return type.AssemblyQualifiedName!.Split(_separator, StringSplitOptions.None)[0];
    }

    /// <summary>
    /// Gets the name (identifier) of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of the converter.</typeparam>
    /// <returns>The name of the type.</returns>
    protected static string GetTypeName<T>() where T : IMntpConverter {
        return GetTypeName(typeof(T));
    }

    /// <summary>
    /// Creates a new instance based on the specified <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the converter.</typeparam>
    /// <returns>A new instance of <see cref="MntpTypeConverter"/> describing a converter of type <typeparamref name="T"/>.</returns>
    public static MntpTypeConverter Create<T>() where T : IMntpConverter {
        return new MntpTypeConverter(GetTypeName<T>());
    }

}