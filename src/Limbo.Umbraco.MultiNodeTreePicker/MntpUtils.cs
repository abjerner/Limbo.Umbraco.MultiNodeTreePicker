using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Skybrud.Essentials.Strings.Extensions;

namespace Limbo.Umbraco.MultiNodeTreePicker;

internal static class MntpUtils {

    public static string? GetTypeAlias(Type type) {
        return type.AssemblyQualifiedName is { } name ? GetTypeAlias(name) : null;
    }

    [return: NotNullIfNotNull("typeName")]
    public static string? GetTypeAlias(string? typeName) {
        return typeName?.Split(',').Take(2).Select(x => x.Trim()).Join(", ");
    }

}
