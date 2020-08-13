using ClientDependency.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace Skybrud.Umbraco.MultiNodeTreePicker.PropertyEditors {

    [DataEditor(EditorAlias, "Skybrud Multinode Treepicker", EditorView, ValueType = ValueTypes.Text, Group = "Skybrud.dk", Icon = "icon-page-add color-skybrud")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Skybrud.MultiNodeTreePicker/MntpConverter.js")]
    public class MntpEditor : MultiNodeTreePickerPropertyEditor {

        internal const string EditorAlias = "Skybrud.Umbraco.MultiNodeTreePicker";

        internal const string EditorView = "contentpicker";

        public MntpEditor(ILogger logger) : base(logger) { }

        protected override IConfigurationEditor CreateConfigurationEditor() => new MntpConfigurationEditor();

    }
}
