using System.Collections.Generic;
using System.Threading.Tasks;
using Limbo.Umbraco.MultiNodeTreePicker.Constants;
using Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;
using Skybrud.Essentials.Time;
using Skybrud.Essentials.Umbraco.Manifests.Extensions;
using Skybrud.Essentials.Umbraco.Manifests.Extensions.PropertyEditors;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

namespace Limbo.Umbraco.MultiNodeTreePicker.Manifests;

public class MntpPackageManifestReader : IPackageManifestReader {

    public const string Alias = MntpPackage.Alias;

    public const string Name = MntpPackage.Name;

    public async Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {

        List<PackageManifest> temp = [
            new() {
                Id = MntpPackage.Alias,
                Name = MntpPackage.Name,
                AllowTelemetry = true,
                Version = MntpPackage.InformationalVersion,
                Extensions = [..GetExtensions()],
                Importmap = new PackageManifestImportmap {
                    Imports = new Dictionary<string, string> {
                        {"@limbo/mntp/constants", $"/App_Plugins/{Alias}/Constants.js"},
                        {"@limbo/mntp/service", $"/App_Plugins/{Alias}/Service.js"},
                    }
                }
            }
        ];

        return await Task.FromResult(temp);

    }

    private IEnumerable<IExtension> GetExtensions() {

        yield return new PropertyEditorSchemaExtension {
            Alias = MntpPropertyEditor.EditorAlias,
            Name = $"{Name}: Multi Node Tree Picker Property Editor Schema",
            Meta = new PropertyEditorSchemaMeta {
                DefaultPropertyEditorUiAlias = MntpPropertyEditor.EditorUiAlias,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        new PropertyEditorSettingsProperty {
                            Alias = "advancedOptions",
                            Label = "Advanced options",
                            PropertyEditorUiAlias = MntpPropertyEditorUiAliases.Separator,
                            Config = [
                                new PropertyEditorConfigProperty { Alias = "first", Value = true }
                            ]
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "typeConverter",
                            Label = "Type converter",
                            Description = "Select a type converter to control the type returned by properties using this data type.\r\n\r\n" +
                                          CreateButton("https://packages.limbo.works/dbe1eade", "See the documentation"),
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
                            Description = "Specify the minimum number of items.",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.Integer",
                            Config = [
                                new PropertyEditorConfigProperty { Alias = "min", Value = 0 }
                            ]
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "maxNumber",
                            Label = "Maximum number of items",
                            Description = "Specify the maximum number of items. 0 means no limit.",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.Integer",
                            Config = [
                                new PropertyEditorConfigProperty { Alias = "min", Value = 0 }
                            ]
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "ignoreUserStartNodes",
                            Label = "Ignore user start nodes",
                            Description = "Selecting this option allows a user to choose nodes that they normally don't have access to.",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.Toggle",
                        },
                        new PropertyEditorSettingsProperty {
                            Alias = "startNode",
                            Label = "Node type",
                            Description = "Select the node type.",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.ContentPicker.Source",
                        }
                    ],
                    DefaultData = [
                        new PropertyEditorSettingsDefaultData {
                            Alias = "minNumber",
                            Value = 0
                        },
                        new PropertyEditorSettingsDefaultData {
                            Alias = "maxNumber",
                            Value = 0
                        }
                    ]
                }
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = MntpPropertyEditor.EditorUiAlias,
            Name = $"{Name}: Multi Node Tree Picker Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/Mntp.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo Multi Node Tree Picker",
                Icon = "icon-page-add",
                Group = "Limbo",
                Keywords = ["select", "page", "node", "reference", "related", "link", "pages", "content"],
                PropertyEditorSchemaAlias = MntpPropertyEditor.EditorAlias,
                SupportsReadOnly = true,
                Settings = new PropertyEditorSettings {
                    Properties = [
                        new PropertyEditorSettingsProperty {
                            Alias = "filter",
                            Label = "Accepted types",
                            Description = "Limit to specific types",
                            PropertyEditorUiAlias = "Umb.PropertyEditorUi.ContentPicker.SourceType"
                        }
                    ],
                },
            },
        };

        yield return new PropertyEditorUiExtension {
            Alias = MntpPropertyEditorUiAliases.TypeConverter,
            Name = $"{Name}: Type Converter Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/TypeConverter.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo MNTP Type Converter",
                Icon = "icon-list",
                Group = "Limbo"
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = MntpPropertyEditorUiAliases.CacheLevel,
            Name = $"{Name}: Cache Level Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/CacheLevel.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo MNTP Cache Level",
                Icon = "icon-list",
                Group = "Limbo"
            }
        };

        yield return new PropertyEditorUiExtension {
            Alias = MntpPropertyEditorUiAliases.Separator,
            Name = $"{Name}: Separator Property Editor UI",
            Element = $"/App_Plugins/{Alias}/Elements/Separator.js",
            Meta = new PropertyEditorUiMeta {
                Label = "Limbo MNTP Separator",
                Icon = "icon-list",
                Group = "Limbo"
            }
        };

    }

    private static string CreateButton(string url, string text) {

        // Must be written in a single line as Umbraco will replace all newlines with <br /> tags, which will break the button

        return $"""
                <a href="{url}" target="_blank" rel="noopener noreferrer"><uui-button look="outline" compact label="{text}">&nbsp;{text} &rarr;&nbsp;</uui-button></a>
                """;

    }

}