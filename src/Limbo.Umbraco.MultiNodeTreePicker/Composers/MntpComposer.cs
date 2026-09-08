using Limbo.Umbraco.MultiNodeTreePicker.Api;
using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Limbo.Umbraco.MultiNodeTreePicker.Manifests;
using Microsoft.Extensions.DependencyInjection;
using Skybrud.Essentials.Umbraco.Composing;
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

        // Register the custom package manifest reader
        builder.AddPackageManifestReader<MntpPackageManifestReader>();

        // Register the SwaggerGen options for the API
        builder.Services.ConfigureOptions<MntpSwaggerGenOptions>();

    }

}