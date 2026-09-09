using System.Collections.Generic;
using System.Threading.Tasks;
using Limbo.Umbraco.MultiNodeTreePicker.Constants;
using Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;
using Skybrud.Essentials.Umbraco.Manifests.Extensions;
using Skybrud.Essentials.Umbraco.Manifests.Extensions.PropertyEditors;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

namespace Limbo.Umbraco.MultiNodeTreePicker.Manifests;

/// <summary>
/// Package manifest reader registering the property editor schema, the property editor UIs and the import map of
/// this package with the backoffice.
/// </summary>
public class MntpPackageManifestReader : IPackageManifestReader {

    private const string PluginPath = $"/App_Plugins/{MntpPackage.Alias}";

    private const string DocumentationUrl = "https://packages.limbo.works/73a7c52f";

    /// <inheritdoc />
    public Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {

        PackageManifest manifest = new() {
            Id = MntpPackage.Alias,
            Name = MntpPackage.Name,
            Version = MntpPackage.InformationalVersion,
            AllowTelemetry = true,
            Extensions = [
                GetSchemaExtension(),
                GetPickerUiExtension(),
                GetTypeConverterUiExtension(),
                GetCacheLevelUiExtension(),
                GetSeparatorUiExtension()
            ],
            Importmap = new PackageManifestImportmap {
                Imports = new Dictionary<string, string> {
                    { "@limbo/mntp/constants", $"{PluginPath}/Constants.js" },
                    { "@limbo/mntp/service", $"{PluginPath}/Service.js" }
                }
            }
        };

        return Task.FromResult<IEnumerable<PackageManifest>>([manifest]);

    }

    private static IExtension GetSchemaExtension() {

        return new PropertyEditorSchemaExtension {
            Alias = MntpEditor.EditorAlias,
            Name = $"{MntpPackage.Name}: Property Editor Schema",
            Meta = new PropertyEditorSchemaMeta {
                DefaultPropertyEditorUiAlias = MntpPropertyEditorUiAliases.MultiNodeTreePicker,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        new PropertyEditorSettingsProperty {
                            Alias = "advancedOptions",
                            Label = "Advanced options",
                            PropertyEditorUiAlias = MntpPropertyEditorUiAliases.Separator
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = MntpConfigurationEditor.TypeConverterKey,
                            Label = "Type converter",
                            Description = $"Select a type converter to control the type returned by properties using this data type.\n\n[See the documentation &rarr;]({DocumentationUrl})",
                            PropertyEditorUiAlias = MntpPropertyEditorUiAliases.TypeConverter
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "cacheLevel",
                            Label = "Cache level",
                            Description = "Select the cache level of the underlying property value converter.",
                            PropertyEditorUiAlias = MntpPropertyEditorUiAliases.CacheLevel
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "defaultOptions",
                            Label = "Default options",
                            PropertyEditorUiAlias = MntpPropertyEditorUiAliases.Separator
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "minNumber",
                            Label = "Minimum number of items",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.Integer",
                            Config = [
                                new PropertyEditorConfigProperty { Alias = "min", Value = 0 }
                            ]
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "maxNumber",
                            Label = "Maximum number of items",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.Integer",
                            Config = [
                                new PropertyEditorConfigProperty { Alias = "min", Value = 0 }
                            ]
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "ignoreUserStartNodes",
                            Label = "Ignore user start nodes",
                            Description = "Selecting this option allows a user to choose nodes that they normally don't have access to.",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.Toggle"
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "startNode",
                            Label = "Node type",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.ContentPicker.Source"
                        }
                    ],
                    DefaultData = [
                        new PropertyEditorSettingsDefaultData { Alias = "minNumber", Value = 0 },
                        new PropertyEditorSettingsDefaultData { Alias = "maxNumber", Value = 0 }
                    ]
                }
            }
        };

    }

    private static IExtension GetPickerUiExtension() {

        return new PropertyEditorUiExtension {
            Alias = MntpPropertyEditorUiAliases.MultiNodeTreePicker,
            Name = $"{MntpPackage.Name}: Property Editor UI",
            Element = $"{PluginPath}/Elements/Mntp.js",
            ElementName = "limbo-mntp",
            Meta = new PropertyEditorUiMeta {
                Label = MntpEditor.EditorName,
                Icon = MntpEditor.EditorIcon,
                Group = "Limbo",
                Keywords = ["select", "page", "node", "reference", "related", "link", "pages", "content", "converter", "limbo"],
                PropertyEditorSchemaAlias = MntpEditor.EditorAlias,
                SupportsReadOnly = true,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        new PropertyEditorSettingsProperty {
                            Alias = "filter",
                            Label = "Allow items of type",
                            Description = "Select the applicable types",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.ContentPicker.SourceType"
                        }
                    ]
                }
            }
        };

    }

    private static IExtension GetTypeConverterUiExtension() {

        return new PropertyEditorUiExtension {
            Alias = MntpPropertyEditorUiAliases.TypeConverter,
            Name = $"{MntpPackage.Name}: Type Converter Property Editor UI",
            Element = $"{PluginPath}/Elements/TypeConverter.js",
            ElementName = "limbo-mntp-type-converter",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo MNTP Type Converter",
                Icon = "icon-box",
                Group = "Limbo"
            }
        };

    }

    private static IExtension GetCacheLevelUiExtension() {

        return new PropertyEditorUiExtension {
            Alias = MntpPropertyEditorUiAliases.CacheLevel,
            Name = $"{MntpPackage.Name}: Cache Level Property Editor UI",
            Element = $"{PluginPath}/Elements/CacheLevel.js",
            ElementName = "limbo-mntp-cache-level",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo MNTP Cache Level",
                Icon = "icon-layers",
                Group = "Limbo"
            }
        };

    }

    private static IExtension GetSeparatorUiExtension() {

        return new PropertyEditorUiExtension {
            Alias = MntpPropertyEditorUiAliases.Separator,
            Name = $"{MntpPackage.Name}: Separator Property Editor UI",
            Element = $"{PluginPath}/Elements/Separator.js",
            ElementName = "limbo-mntp-separator",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo MNTP Separator",
                Icon = "icon-navigation-horizontal",
                Group = "Limbo"
            }
        };

    }

}
