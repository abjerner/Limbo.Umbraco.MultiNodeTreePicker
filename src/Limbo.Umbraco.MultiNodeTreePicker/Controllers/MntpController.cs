using System.Linq;
using Limbo.Umbraco.MultiNodeTreePicker.Composers;

namespace Limbo.Umbraco.MultiNodeTreePicker.Controllers
{

    [JsonOnlyConfiguration]
    [PluginController("Limbo")]
    public class MntpController : UmbracoAuthorizedApiController
    {

        private readonly MntpConverterCollection _converterCollection;

        public MntpController(MntpConverterCollection converterCollection)
        {
            _converterCollection = converterCollection;
        }

        public object GetTypes()
        {
            return _converterCollection.ToArray().Select(x => new
            {
                assembly = x.GetType().Assembly.FullName,
                key = x.GetType().AssemblyQualifiedName,
                name = x.Name
            });
        }

    }

}