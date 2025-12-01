using Limbo.Umbraco.MultiNodeTreePicker.Constants;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.MultiNodeTreePicker.Models;

public static class TreeSource {

    public static MultiNodePickerConfigurationTreeSource Create(string? objectType) {
        return new MultiNodePickerConfigurationTreeSource { ObjectType = objectType };
    }

    public static MultiNodePickerConfigurationTreeSource CreateWithContent() {
        return new MultiNodePickerConfigurationTreeSource { ObjectType = TreeSourceObjectTypes.Content };
    }

}