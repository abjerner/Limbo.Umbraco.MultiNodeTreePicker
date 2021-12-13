namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors
{

    [DataEditor(EditorAlias, "Limbo Multinode Treepicker", EditorView, ValueType = ValueTypes.Text, Group = "Limbo.dk", Icon = "icon-page-add color-limbo")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Limbo.MultiNodeTreePicker/MntpConverter.js")]
    public class MntpEditor : MultiNodeTreePickerPropertyEditor
    {

        internal const string EditorAlias = "Limbo.Umbraco.MultiNodeTreePicker";

        internal const string EditorView = "contentpicker";

        public MntpEditor(ILogger logger) : base(logger) { }

        protected override IConfigurationEditor CreateConfigurationEditor() => new MntpConfigurationEditor();

    }
}
