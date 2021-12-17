using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Skybrud.Umbraco.MultiNodeTreePicker.PropertyEditors
{
    [DataEditor(EditorAlias, "Skybrud Multinode Treepicker", EditorView, ValueType = ValueTypes.Text, Group = "Skybrud.dk", Icon = "icon-page-add color-skybrud")]
    public class MntpEditor : MultiNodeTreePickerPropertyEditor
    {

        internal const string EditorAlias = "Skybrud.Umbraco.MultiNodeTreePicker";

        internal const string EditorView = "contentpicker";
        private readonly IIOHelper iOHelper;

        public MntpEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper iOHelper) : base(dataValueEditorFactory, iOHelper)
        {
            this.iOHelper = iOHelper;
        }

        protected override IConfigurationEditor CreateConfigurationEditor()
        {
            return new MntpConfigurationEditor(iOHelper);
        }
    }
}
