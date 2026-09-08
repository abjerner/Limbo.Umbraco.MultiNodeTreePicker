using Umbraco.Cms.Api.Management.OpenApi;

namespace Limbo.Umbraco.MultiNodeTreePicker.Api;

internal class MntpSecurityFilter : BackOfficeSecurityRequirementsOperationFilterBase {

    protected override string ApiName => MntpApiConstants.Name;

}