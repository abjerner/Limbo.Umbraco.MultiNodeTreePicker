using Limbo.Umbraco.MultiNodeTreePicker.Api;
using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Limbo.Umbraco.MultiNodeTreePicker.Manifests;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Infrastructure.Manifest;

namespace Limbo.Umbraco.MultiNodeTreePicker.Composers;

internal sealed class MntpComposer : IComposer {

    public void Compose(IUmbracoBuilder builder) {

        builder
            .WithCollectionBuilder<MntpTypeConverterCollectionBuilder>()
            .Add(() => builder.TypeLoader.GetTypes<IMntpTypeConverter>());

        builder
            .WithCollectionBuilder<MntpConverterCollectionBuilder>()
            .Add(() => builder.TypeLoader.GetTypes<IMntpItemConverter>());

        // Register the package manifest (property editor schema, property editor UIs and import map)
        builder.Services.AddSingleton<IPackageManifestReader, MntpPackageManifestReader>();

        // Register a separate Swagger document for the package's Management API
        builder.Services.ConfigureOptions<MntpSwaggerGenOptions>();

    }

}
