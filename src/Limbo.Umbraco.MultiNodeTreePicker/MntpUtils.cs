using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MultiNodeTreePicker {

    internal static class MntpUtils {

        public static string? GetTypeName(Type type) {
            return type.AssemblyQualifiedName is { } name ? GetTypeName(name) : null;
        }

        [return: NotNullIfNotNull("typeName")]
        public static string? GetTypeName(string? typeName) {
            return typeName?.Split(',').Take(2).Join(",");
        }

        public static void PrependLinkToDescription(ConfigurationField field, string text, string url) {
            string a = $"<a href=\"{url}\" class=\"btn btn-primary btn-xs limbo-multinode-treepicker-button\" target=\"_blank\" rel=\"noreferrer noopener\">{text}</a>";
            field.Description = $"{a}\r\n{field.Description}";
        }

    }

}