using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.WebAssets;

namespace Limbo.Umbraco.MultiNodeTreePicker.Composers {

    internal sealed class MntpComposer : IComposer {

        public void Compose(IUmbracoBuilder builder) {
            builder.WithCollectionBuilder<MntpConverterCollectionBuilder>().Add(() => builder.TypeLoader.GetTypes<IMntpItemConverter>());
            builder.BackOfficeAssets().Append<BackOfficeJavaScriptAsset>();
        }

        public class BackOfficeJavaScriptAsset : IAssetFile {

            public AssetType DependencyType => AssetType.Javascript;

            public string FilePath { get; set; }

            public BackOfficeJavaScriptAsset() {
                FilePath = "/App_Plugins/Limbo.Umbraco.MultiNodeTreePicker/MntpConverter.js";
            }

        }

    }

}