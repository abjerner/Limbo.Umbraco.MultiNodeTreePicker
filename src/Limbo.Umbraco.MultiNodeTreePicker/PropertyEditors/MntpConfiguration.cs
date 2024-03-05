using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;

/// <summary>
/// Class representing the configuration of a <see cref="MntpEditor"/> data type.
/// </summary>
public class MntpConfiguration : MultiNodePickerConfiguration {

    /// <summary>
    /// Gets whether the multinode treepicker is configured as a single picker.
    /// </summary>
    [IgnoreDataMember]
    public bool IsSinglePicker => base.MaxNumber == 1;

    /// <summary>
    /// Gets or sets an instance of <see cref="JObject"/> representing the information about the selected item converter.
    /// </summary>
    [ConfigurationField("itemConverter", "Item converter", "/App_Plugins/Limbo.Umbraco.MultiNodeTreePicker/Views/ItemConverter.html?v={version}", Description = "Select a item converter to control the type of the items returned by properties of this data type.")]
    public JToken? ItemConverter { get; set; }

}