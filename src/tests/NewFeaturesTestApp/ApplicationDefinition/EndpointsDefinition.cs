using Microsoft.AspNetCore.Builder;
using RzR.Infrastructure.BootstrapDefinition.AppAndServiceImplements;
using RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts;

namespace NewFeaturesTestApp.ApplicationDefinition;

/// <summary>
/// Demonstrates order=1 (runs after ServicesDefinition at -1 and AsyncWarmupDefinition at 0).
/// Maps the minimal HTTP endpoints via UseEndpoints so routing middleware (registered in
/// ServicesDefinition) is guaranteed to be present before endpoints are mapped.
/// </summary>
public class EndpointsDefinition : InitDefinitionDecorate
{
    public EndpointsDefinition()
    {
        base.InitializeOrder = 1;
    }

    /// <inheritdoc />
    public override void ApplicationConfiguration(ApplicationConfigurationContext context)
    {
        context.ApplicationBuilder.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", () => "NewFeaturesTestApp is running. Wave 1 context API active.");
            endpoints.MapGet("/health", () => "OK");
            endpoints.MapRazorPages();
        });
    }
}
