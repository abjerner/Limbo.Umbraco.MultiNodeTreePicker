using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Limbo.Umbraco.MultiNodeTreePicker.Manifest;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Limbo.Umbraco.MultiNodeTreePicker.Composers;

internal sealed class MntpComposer : IComposer {

    public void Compose(IUmbracoBuilder builder) {

        builder
            .WithCollectionBuilder<MntpTypeConverterCollectionBuilder>()
            .Add(() => builder.TypeLoader.GetTypes<IMntpTypeConverter>());

        builder
            .WithCollectionBuilder<MntpConverterCollectionBuilder>()
            .Add(() => builder.TypeLoader.GetTypes<IMntpItemConverter>());

        builder
            .ManifestFilters()
            .Append<MntpManifestFilter>();

    }

}