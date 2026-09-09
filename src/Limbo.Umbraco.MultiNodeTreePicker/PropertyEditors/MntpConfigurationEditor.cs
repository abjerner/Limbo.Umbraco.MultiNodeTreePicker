using System.Collections.Generic;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;

#pragma warning disable 1591

namespace Limbo.Umbraco.MultiNodeTreePicker.PropertyEditors;

public class MntpConfigurationEditor : ConfigurationEditor<MntpConfiguration> {

    /// <summary>
    /// The configuration key used by the v13 version of this package for the selected converter.
    /// </summary>
    internal const string LegacyItemConverterKey = "itemConverter";

    /// <summary>
    /// The configuration key used for the selected converter.
    /// </summary>
    internal const string TypeConverterKey = "typeConverter";

    public MntpConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

    /// <inheritdoc />
    public override object ToConfigurationObject(IDictionary<string, object> configuration, IConfigurationEditorJsonSerializer configurationEditorJsonSerializer) {

        // Data types upgraded from v13 still hold the selected converter under "itemConverter", so we fall back to
        // that value if "typeConverter" hasn't been set yet
        if (!HasValue(configuration, TypeConverterKey) && configuration.TryGetValue(LegacyItemConverterKey, out object? legacy) && legacy is not null) {
            configuration = new Dictionary<string, object>(configuration) { [TypeConverterKey] = legacy };
        }

        return base.ToConfigurationObject(configuration, configurationEditorJsonSerializer);

    }

    private static bool HasValue(IDictionary<string, object> configuration, string key) {
        return configuration.TryGetValue(key, out object? value) && value is not null && value is not "";
    }

}
