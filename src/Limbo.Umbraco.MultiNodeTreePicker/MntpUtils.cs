using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MultiNodeTreePicker {

    internal static class MntpUtils {

        public static void PrependLinkToDescription(ConfigurationField field, string text, string url) {
            string a = $"<a href=\"{url}\" class=\"btn btn-primary btn-xs limbo-multinode-treepicker-button\" target=\"_blank\" rel=\"noreferrer noopener\">{text}</a>";
            field.Description = $"{a}\r\n{field.Description}";
        }

    }

}