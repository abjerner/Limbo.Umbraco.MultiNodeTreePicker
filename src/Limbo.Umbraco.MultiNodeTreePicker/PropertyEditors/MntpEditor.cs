using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable 1591

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors {

    [DataEditor(EditorAlias, "Limbo Multinode Treepicker", EditorView, ValueType = ValueTypes.Text, Group = "Limbo", Icon = "icon-page-add color-limbo")]
    public class MntpEditor : MultiNodeTreePickerPropertyEditor {

        /// <summary>
        /// Gets the alias of the property editor.
        /// </summary>
        public const string EditorAlias = "Limbo.Umbraco.MultiNodeTreePicker";

        /// <summary>
        /// Gets the view of the property editor.
        /// </summary>
        public const string EditorView = "contentpicker";

        private readonly IIOHelper _ioHelper;
        private readonly IEditorConfigurationParser _editorConfigurationParser;

        public MntpEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(dataValueEditorFactory, ioHelper, editorConfigurationParser) {
            _ioHelper = ioHelper;
            _editorConfigurationParser = editorConfigurationParser;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new MntpConfigurationEditor(_ioHelper, _editorConfigurationParser);
        }

    }

}