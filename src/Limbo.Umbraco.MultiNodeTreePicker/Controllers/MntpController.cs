using System;
using Limbo.Umbraco.MultiNodeTreePicker.Composers;
using System.Linq;
using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

#pragma warning disable 1591

namespace Limbo.Umbraco.MultiNodeTreePicker.Controllers {

    [PluginController("Limbo")]
    public class MntpController : UmbracoAuthorizedApiController {

        private readonly MntpConverterCollection _converterCollection;

        public MntpController(MntpConverterCollection converterCollection) {
            _converterCollection = converterCollection;
        }

        public object GetTypes() {
            return _converterCollection.ToArray().Select(Map);
        }

        private static JObject Map(IMntpItemConverter converter) {

            Type type = converter.GetType();

            JObject json = new() {
                { "assembly", type.Assembly.FullName },
                { "type", MntpUtils.GetTypeName(type) },
                { "icon", $"{converter.Icon ?? "icon-box"} color-{type.Assembly.FullName?.Split('.')[0].ToLower()}" },
                { "name", converter.Name },
                { "description", type.AssemblyQualifiedName?.Split(new[] { ", Version" }, StringSplitOptions.None)[0] + ".dll" }
            };

            return json;

        }

    }

}