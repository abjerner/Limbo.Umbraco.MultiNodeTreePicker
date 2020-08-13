using Newtonsoft.Json.Linq;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace Skybrud.Umbraco.MultiNodeTreePicker.PropertyEditors {
    
    public class MntpConfiguration : MultiNodePickerConfiguration {

        [ConfigurationField("itemConverter", "Item converter", "/App_Plugins/Skybrud.MultiNodeTreePicker/MntpConverter.html", Description = "Select an item converter.")]
        public JObject ItemConverter { get; set; }

    }

}