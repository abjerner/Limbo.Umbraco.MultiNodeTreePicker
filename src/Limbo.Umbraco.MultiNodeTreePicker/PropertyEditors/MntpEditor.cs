using System;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Infrastructure.WebAssets;

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors
{
    [DataEditor(EditorAlias, "Limbo Multinode Treepicker", EditorView, ValueType = ValueTypes.Text, Group = "Limbo.works", Icon = "icon-page-add color-limbo")]
    public class MntpEditor : MultiNodeTreePickerPropertyEditor
    {

        internal const string EditorAlias = "Limbo.Umbraco.MultiNodeTreePicker";

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
