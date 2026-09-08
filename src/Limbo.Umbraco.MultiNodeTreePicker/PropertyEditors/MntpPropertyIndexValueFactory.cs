using System;
using System.Collections.Generic;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;

public class MntpPropertyIndexValueFactory : IPropertyIndexValueFactory {

    public IEnumerable<IndexValue> GetIndexValues(IProperty property, string? culture, string? segment, bool published) {

        // Get the source value from the property
        object? source = property.GetValue(culture, segment, published);

        // Validate the source value
        if (source is not string str) yield break;

        // Add the property value (JSON serialized string) to the index
        yield return Create(property.Alias, culture, new object?[] { str });

        // The saved value is a list of UDIs, which isn't really that good for searching. UDIs aren't necessarily GUID
        // UDIs, but in this case we know they are, so we can parse them, and then grab only the GUID part, which
        // (if indexed without the hyphens) are a lot more search friendly
        List<string> guids = [];
        foreach (string udi in str.Split(',')) {
            if (UdiParser.TryParse(udi, out GuidUdi? result)) guids.Add($"{result.Guid:N}");
        }

        // Add a field with the search friendly GUID keys
        yield return Create($"{property.Alias}_search", culture, guids);

    }

    public IEnumerable<IndexValue> GetIndexValues(IProperty property, string? culture, string? segment, bool published,
        IEnumerable<string> availableCultures, IDictionary<Guid, IContentType> contentTypeDictionary) {
        throw new NotImplementedException();
    }

    private static IndexValue Create(string fieldName, string? culture, object? value) {
        if (!string.IsNullOrWhiteSpace(culture)) fieldName = $"{fieldName}_{culture}";
        return new IndexValue { FieldName = fieldName, Culture = culture, Values = value is null ? [] : [value] };
    }

}