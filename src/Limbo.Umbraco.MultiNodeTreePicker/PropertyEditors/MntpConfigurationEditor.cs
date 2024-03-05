using System.Collections.Generic;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable 1591

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;

public class MntpConfigurationEditor : ConfigurationEditor<MntpConfiguration> {

    public MntpConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) {
        Field(nameof(MultiNodePickerConfiguration.TreeSource))
            .Config = new Dictionary<string, object> { { "idType", "udi" } };

        foreach (var field in Fields) {

            if (field.View is not null) field.View = field.View.Replace("{version}", MntpPackage.InformationalVersion);

            switch (field.Key) {

                case "itemConverter":
                    MntpUtils.PrependLinkToDescription(
                        field,
                        "See the documentation &rarr;",
                        "https://packages.limbo.works/163af59b"
                    );
                    break;

            }

        }


    }

    public override Dictionary<string, object> ToConfigurationEditor(MntpConfiguration? configuration) {

        var output = base.ToConfigurationEditor(configuration);

        output["multiPicker"] = configuration?.MaxNumber > 1;

        return output;
    }

    /// <inheritdoc />
    public override IDictionary<string, object> ToValueEditor(object? configuration) {
        var d = base.ToValueEditor(configuration);
        d["multiPicker"] = true;
        d["showEditButton"] = false;
        d["showPathOnHover"] = false;
        d["idType"] = "udi";
        return d;
    }

}