using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

namespace Limbo.Umbraco.MultiNodeTreePicker.Composers
{
    internal sealed class MntpComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddUnique<MntpConverterCollection>();

            builder.PropertyValueConverters().Append<IMntpItemConverter>();
        }
    }

}