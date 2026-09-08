using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable 1591

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;

[DataEditor(EditorAlias, ValueType = EditorValueType)]
public class MntpPropertyEditor : MultiNodeTreePickerPropertyEditor {

    /// <summary>
    /// Gets the alias of the property editor.
    /// </summary>
    public const string EditorAlias = "Limbo.Umbraco.MultiNodeTreePicker";

    public const string EditorUiAlias = "Limbo.Umbraco.MultiNodeTreePicker.PropertyEditorUi";

    public const string EditorName = "Limbo Multinode Treepicker";

    public const string EditorValueType = ValueTypes.Text;

    public const string EditorIcon = "icon-page-add";

    private readonly IIOHelper _ioHelper;

    public override IPropertyIndexValueFactory PropertyIndexValueFactory => new MntpPropertyIndexValueFactory();

    public MntpPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper) : base(dataValueEditorFactory, ioHelper) {
        _ioHelper = ioHelper;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new MntpConfigurationEditor(_ioHelper);
    }

}