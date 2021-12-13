using System.Collections.Generic;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors {
    
    public class MntpConfigurationEditor : ConfigurationEditor<MntpConfiguration> {
        
        public MntpConfigurationEditor() {
            Field(nameof(MultiNodePickerConfiguration.TreeSource))
                .Config = new Dictionary<string, object> {{"idType", "udi"}};
        }

        public override Dictionary<string, object> ToConfigurationEditor(MntpConfiguration configuration) {
            // sanitize configuration
            var output = base.ToConfigurationEditor(configuration);

            output["multiPicker"] = configuration.MaxNumber > 1;

            return output;
        }

        /// <inheritdoc />
        public override IDictionary<string, object> ToValueEditor(object configuration) {
            var d = base.ToValueEditor(configuration);
            d["multiPicker"] = true;
            d["showEditButton"] = false;
            d["showPathOnHover"] = false;
            d["idType"] = "udi";
            return d;
        }

    }

}