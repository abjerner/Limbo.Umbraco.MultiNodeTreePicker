using System;
using Limbo.Umbraco.MultiNodeTreePicker.Constants;
using Skybrud.Essentials.Common;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.MultiNodeTreePicker.Extensions;

public static class TreeSourceExtensions {

    public static MultiNodePickerConfigurationTreeSource SetObjectType(this MultiNodePickerConfigurationTreeSource treeSource, string objectType) {
        treeSource.ObjectType = objectType;
        return treeSource;
    }

    public static MultiNodePickerConfigurationTreeSource AddDynamicRoot(this MultiNodePickerConfigurationTreeSource treeSource, string originAlias) {
        treeSource.DynamicRoot = new DynamicRoot { OriginAlias = originAlias };
        return treeSource;
    }

    public static MultiNodePickerConfigurationTreeSource AddQueryStep(this MultiNodePickerConfigurationTreeSource treeSource, string alias, params Guid[] anyOfDocTypeKeys) {
        if (treeSource.DynamicRoot is null) throw new PropertyNotSetException(nameof(treeSource.DynamicRoot));
        treeSource.DynamicRoot.QuerySteps = [
            .. treeSource.DynamicRoot.QuerySteps,
            new QueryStep { Alias = alias, AnyOfDocTypeKeys = anyOfDocTypeKeys }
        ];
        return treeSource;
    }

    public static MultiNodePickerConfigurationTreeSource AddQueryStepNearestAncestorOrSelf(this MultiNodePickerConfigurationTreeSource treeSource, params Guid[] anyOfDocTypeKeys) {
        return treeSource.AddQueryStep(DynamicRootAliases.NearestAncestorOrSelf, anyOfDocTypeKeys);
    }

    public static MultiNodePickerConfigurationTreeSource AddQueryStepNearestDescendantOrSelf(this MultiNodePickerConfigurationTreeSource treeSource, params Guid[] anyOfDocTypeKeys) {
        return treeSource.AddQueryStep(DynamicRootAliases.NearestDescendantOrSelf, anyOfDocTypeKeys);
    }

}