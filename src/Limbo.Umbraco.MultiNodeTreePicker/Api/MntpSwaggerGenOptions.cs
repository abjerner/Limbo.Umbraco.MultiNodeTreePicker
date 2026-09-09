using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Limbo.Umbraco.MultiNodeTreePicker.Api;

/// <summary>
/// Registers a separate Swagger document for this package's Management API.
/// </summary>
public class MntpSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions> {

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options) {

        options.SwaggerDoc(MntpApiConstants.ApiName, new OpenApiInfo {
            Title = MntpApiConstants.ApiTitle,
            Version = MntpApiConstants.Version
        });

        options.OperationFilter<MntpSecurityRequirementsOperationFilter>();

    }

}
