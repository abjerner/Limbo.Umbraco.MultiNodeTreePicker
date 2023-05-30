using System.Collections.Generic;
using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.MultiNodeTreePicker.Manifest {

    /// <inheritdoc />
    public class MntpManifestFilter : IManifestFilter {

        /// <inheritdoc />
        public void Filter(List<PackageManifest> manifests) {
            manifests.Add(new PackageManifest {
                AllowPackageTelemetry = true,
                PackageName = MntpPackage.Name,
                Version = MntpPackage.InformationalVersion,
                BundleOptions = BundleOptions.Independent,
                Scripts = new[] {
                    $"/App_Plugins/{MntpPackage.Alias}/Scripts/Controllers/ItemConverter.js",
                    $"/App_Plugins/{MntpPackage.Alias}/Scripts/Controllers/ItemConverterOverlay.js"
                },
                Stylesheets = new[] {
                    $"/App_Plugins/{MntpPackage.Alias}/Styles/Styles.css"
                }
            });
        }

    }

}