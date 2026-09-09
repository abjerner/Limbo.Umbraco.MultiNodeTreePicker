using System.Text.Json.Serialization;
using Limbo.Umbraco.MultiNodeTreePicker.Models;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;

/// <summary>
/// Class representing the configuration of a <see cref="MntpEditor"/> data type.
/// </summary>
public class MntpConfiguration : MultiNodePickerConfiguration {

    /// <summary>
    /// Gets whether the multinode treepicker is configured as a single picker.
    /// </summary>
    [JsonIgnore]
    public bool IsSinglePicker => MaxNumber == 1;

    /// <summary>
    /// Gets or sets the selected type converter (or item converter).
    /// </summary>
    [ConfigurationField("typeConverter")]
    public MntpTypeConverter? TypeConverter { get; set; }

    /// <summary>
    /// Gets or sets the property cache level of the underlying property value converter. Defaults to
    /// <see cref="PropertyCacheLevel.Elements"/> if not specified.
    /// </summary>
    [ConfigurationField("cacheLevel")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PropertyCacheLevel? CacheLevel { get; set; }

}
