using System.Diagnostics.CodeAnalysis;
using Limbo.Umbraco.MultiNodeTreePicker.Json.Newtonsoft;
using Newtonsoft.Json;

namespace Limbo.Umbraco.MultiNodeTreePicker.Models;

/// <summary>
/// Class describing a selected item converter.
/// </summary>
[JsonConverter(typeof(MntpItemConverterJsonConverter))]
public class MntpItemConverter {

    /// <summary>
    /// Gets or sets the alias of the item converter type.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Initializes a new instance with the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The alias of the item converter type.</param>
    [SetsRequiredMembers]
    public MntpItemConverter(string type) {
        Type = type;
    }

}