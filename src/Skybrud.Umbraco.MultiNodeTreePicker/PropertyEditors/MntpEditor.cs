using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable 1591

namespace Skybrud.Umbraco.MultiNodeTreePicker.PropertyEditors {

    [DataEditor(EditorAlias, "Skybrud Multinode Treepicker", EditorView, ValueType = ValueTypes.Text, Group = "Skybrud.dk", Icon = "icon-page-add color-skybrud")]
    public class MntpEditor : MultiNodeTreePickerPropertyEditor {

        /// <summary>
        /// Gets the alias of the property editor.
        /// </summary>
        public const string EditorAlias = "Skybrud.Umbraco.MultiNodeTreePicker";

        /// <summary>
        /// Gets the view of the property editor.
        /// </summary>
        public const string EditorView = "contentpicker";

        private readonly IIOHelper _iOHelper;

        public MntpEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper iOHelper) : base(dataValueEditorFactory, iOHelper) {
            _iOHelper = iOHelper;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new MntpConfigurationEditor(_iOHelper);
        }

    }

}