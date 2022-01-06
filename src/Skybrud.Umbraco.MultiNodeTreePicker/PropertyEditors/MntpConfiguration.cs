using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.PropertyEditors;

namespace Skybrud.Umbraco.MultiNodeTreePicker.PropertyEditors {

    /// <summary>
    /// Class representing the configuration of a <see cref="MntpEditor"/> data type.
    /// </summary>
    public class MntpConfiguration : MultiNodePickerConfiguration {

        /// <summary>
        /// Gets or sets an instance of <see cref="JObject"/> representing the information about the selected item converter.
        /// </summary>
        [ConfigurationField("itemConverter", "Item converter", "/App_Plugins/Skybrud.Umbraco.MultiNodeTreePicker/MntpConverter.html", Description = "Select an item converter.")]
        public JObject ItemConverter { get; set; }

    }

}