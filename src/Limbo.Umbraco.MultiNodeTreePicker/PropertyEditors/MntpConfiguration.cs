using Newtonsoft.Json.Linq;

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors
{

    public class MntpConfiguration : MultiNodePickerConfiguration
    {

        [ConfigurationField("itemConverter", "Item converter", "/App_Plugins/Limbo.MultiNodeTreePicker/MntpConverter.html", Description = "Select an item converter.")]
        public JObject ItemConverter { get; set; }

    }

}