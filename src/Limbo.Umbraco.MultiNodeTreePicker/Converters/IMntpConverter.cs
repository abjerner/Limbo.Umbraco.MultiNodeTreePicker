using Newtonsoft.Json;

namespace Limbo.Umbraco.MultiNodeTreePicker.Converters;

/// <summary>
/// Interface describing a MNTP converter.
/// </summary>
public interface IMntpConverter {

    /// <summary>
    /// Gets the alias of the converter.
    /// </summary>
    public sealed string? Alias => MntpUtils.GetTypeAlias(GetType());

    /// <summary>
    /// Gets the friendly name of the converter.
    /// </summary>
    [JsonProperty("name")]
    string Name { get; }

    /// <summary>
    /// Gets the icon of the converter.
    /// </summary>
    [JsonProperty("icon")]
    public string? Icon => null;

}