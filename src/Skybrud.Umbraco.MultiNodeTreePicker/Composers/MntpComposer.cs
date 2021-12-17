using Skybrud.Umbraco.MultiNodeTreePicker.Converters;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Skybrud.Umbraco.MultiNodeTreePicker.Composers
{
    internal sealed class MntpComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            //builder.Services.AddUnique<MntpConverterCollection>();

            builder.WithCollectionBuilder<MntpConverterCollectionBuilder>().Add(() => builder.TypeLoader.GetTypes<IMntpItemConverter>());
        }
    }

}