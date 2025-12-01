using System.Collections.Generic;
using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.MultiNodeTreePicker.Manifest;

/// <inheritdoc />
public class MntpManifestFilter : IManifestFilter {

    /// <inheritdoc />
    public void Filter(List<PackageManifest> manifests) {

        // Initialize a new manifest filter for this package
        PackageManifest manifest = new() {
            AllowPackageTelemetry = true,
            PackageId = MntpPackage.Alias,
            PackageName = MntpPackage.Name,
            Version = MntpPackage.InformationalVersion,
            BundleOptions = BundleOptions.Independent,
            Scripts = [
                $"/App_Plugins/{MntpPackage.Alias}/Scripts/Controllers/CacheLevel.js",
                $"/App_Plugins/{MntpPackage.Alias}/Scripts/Controllers/ItemConverter.js",
                $"/App_Plugins/{MntpPackage.Alias}/Scripts/Controllers/ItemConverterOverlay.js"
            ],
            Stylesheets = [
                $"/App_Plugins/{MntpPackage.Alias}/Styles/Styles.css"
            ]
        };

        // Append the manifest
        manifests.Add(manifest);

    }

}