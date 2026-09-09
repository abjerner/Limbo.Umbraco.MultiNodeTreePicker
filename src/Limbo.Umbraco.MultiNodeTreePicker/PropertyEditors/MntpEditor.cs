using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable 1591

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;

/// <summary>
/// Property editor (schema) for the Limbo multinode treepicker. Extends the built-in multinode treepicker, so the
/// stored value format is the same as the built-in picker.
/// </summary>
[DataEditor(EditorAlias, ValueType = ValueTypes.Text)]
public class MntpEditor : MultiNodeTreePickerPropertyEditor {

    /// <summary>
    /// Gets the alias of the property editor.
    /// </summary>
    public const string EditorAlias = "Limbo.Umbraco.MultiNodeTreePicker";

    /// <summary>
    /// Gets the friendly name of the property editor.
    /// </summary>
    public const string EditorName = "Limbo Multinode Treepicker";

    /// <summary>
    /// Gets the icon of the property editor.
    /// </summary>
    public const string EditorIcon = "icon-page-add";

    private readonly IIOHelper _ioHelper;

    public override IPropertyIndexValueFactory PropertyIndexValueFactory => new MntpPropertyIndexValueFactory();

    public MntpEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper) : base(dataValueEditorFactory, ioHelper) {
        _ioHelper = ioHelper;
        SupportsReadOnly = true;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new MntpConfigurationEditor(_ioHelper);
    }

}
