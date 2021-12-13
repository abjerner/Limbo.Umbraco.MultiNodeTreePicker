using System.Linq;
using Limbo.Umbraco.MultiNodeTreePicker.Composers;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace Limbo.Umbraco.MultiNodeTreePicker.Controllers
{
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