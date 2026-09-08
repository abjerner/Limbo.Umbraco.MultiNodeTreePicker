using System.Runtime.Serialization;
using Limbo.Umbraco.MultiNodeTreePicker.Models;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;

/// <summary>
/// Class representing the configuration of a <see cref="MntpPropertyEditor"/> data type.
/// </summary>
public class MntpConfiguration : MultiNodePickerConfiguration {

    /// <summary>
    /// Gets whether the multinode treepicker is configured as a single picker.
    /// </summary>
    [IgnoreDataMember]
    public bool IsSinglePicker => MaxNumber == 1;

    /// <summary>
    /// Gets or sets an instance of <see cref="JObject"/> representing the information about the selected type converter.
    /// </summary>
    [ConfigurationField("typeConverter")]
    public MntpTypeConverter? TypeConverter { get; set; }

    /// <summary>
    /// Gets or sets the property cache level of the underlying property value converter. Defaults to <see cref="PropertyCacheLevel.Snapshot"/> if not specified.
    /// </summary>
    [ConfigurationField("cacheLevel")]
    public PropertyCacheLevel? CacheLevel { get; set; }

}