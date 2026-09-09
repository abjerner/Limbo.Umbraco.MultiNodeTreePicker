using Umbraco.Cms.Api.Management.OpenApi;

namespace Limbo.Umbraco.MultiNodeTreePicker.Api;

/// <summary>
/// Operation filter adding the backoffice security requirements to the Swagger document of this package's API.
/// </summary>
public class MntpSecurityRequirementsOperationFilter : BackOfficeSecurityRequirementsOperationFilterBase {

    /// <inheritdoc />
    protected override string ApiName => MntpApiConstants.ApiName;

}
