using System.Collections.Generic;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.MultiNodeTreePicker.Manifest {

    /// <inheritdoc />
    public class MntpManifestFilter : IManifestFilter {

        /// <inheritdoc />
        public void Filter(List<PackageManifest> manifests) {
            manifests.Add(new PackageManifest {
                PackageName = MntpPackage.Alias.ToKebabCase(),
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