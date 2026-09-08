using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Limbo.Umbraco.MultiNodeTreePicker.Api;

public class MntpSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions> {

    public void Configure(SwaggerGenOptions options) {

        options.SwaggerDoc(MntpApiConstants.Alias, new OpenApiInfo {
            Title = MntpApiConstants.Name,
            Version = MntpApiConstants.Version
        });

        options.OperationFilter<MntpSecurityFilter>();

    }

}