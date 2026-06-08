using Microsoft.Extensions.Logging;
using RzR.Infrastructure.BootstrapDefinition.AppAndServiceImplements;
using RzR.Infrastructure.BootstrapDefinition.Abstraction;
using RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts;

namespace NewFeaturesTestApp.ApplicationDefinition;

/// <summary>
/// Demonstrates F4 (DependsOn / DAG ordering).
/// This definition explicitly depends on <see cref="ServicesDefinition"/> via
/// <see cref="DependsOnAttribute"/>, guaranteeing it runs after ServicesDefinition
/// regardless of <see cref="RzR.Infrastructure.BootstrapDefinition.Abstraction.IDefinitionMetadata.InitializeOrder"/>.
/// It maps the additional /wave2 endpoint which requires routing middleware registered by ServicesDefinition.
/// </summary>
[DependsOn(typeof(ServicesDefinition))]
public class RoutingMiddlewareDefinition : InitDefinitionDecorate
{
    public RoutingMiddlewareDefinition()
    {
        // Same order as ServicesDefinition (-1), so without [DependsOn] the DAG tiebreak
        // would sort by full name. With [DependsOn] the edge forces ServicesDefinition first.
        base.InitializeOrder = -1;
    }

    /// <inheritdoc />
    public override void ApplicationConfiguration(ApplicationConfigurationContext context)
    {
        var logger = context.ServiceProvider?
            .GetService<ILogger<RoutingMiddlewareDefinition>>();

        logger?.LogInformation(
            "RoutingMiddlewareDefinition.ApplicationConfiguration: running after ServicesDefinition " +
            "(DependsOn enforced). env={Environment}",
            context.Environment?.EnvironmentName ?? "unknown");

        context.ApplicationBuilder.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/wave2", () =>
                "Wave 2 endpoint: F1 WebApplicationBuilder extensions, F2 config-disable, " +
                "F3 assembly+filter, F4 DependsOn ordering.");
        });
    }
}
