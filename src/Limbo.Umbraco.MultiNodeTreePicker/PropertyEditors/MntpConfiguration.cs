using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors
{

    public class MntpConfiguration : MultiNodePickerConfiguration
    {

        [ConfigurationField("itemConverter", "Item converter", "/App_Plugins/Limbo.Umbraco.MultiNodeTreePicker/MntpConverter.html", Description = "Select an item converter.")]
        public JObject ItemConverter { get; set; }

    }

}