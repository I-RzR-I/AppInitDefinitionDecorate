using Microsoft.Extensions.Logging;
using RzR.Infrastructure.BootstrapDefinition.AppAndServiceImplements;
using RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts;

namespace NewFeaturesTestApp.ApplicationDefinition;

/// <summary>
/// Demonstrates order=0 and the async hooks. Both async overrides are shown:
/// <see cref="ServiceConfigurationAsync"/> for an async service-registration step and
/// <see cref="ApplicationConfigurationAsync"/> for an async pipeline-setup step (e.g. a warmup task).
/// The base synchronous methods are intentionally NOT overridden to verify that the default
/// bridge (sync-to-async delegation) works correctly.
/// </summary>
public class AsyncWarmupDefinition : InitDefinitionDecorate
{
    public AsyncWarmupDefinition()
    {
        base.InitializeOrder = 0;
    }

    /// <inheritdoc />
    public override async ValueTask ServiceConfigurationAsync(
        ServiceConfigurationContext context,
        CancellationToken cancellationToken = default)
    {
        // Simulate an async service-registration step (e.g. fetching remote config).
        await Task.Delay(1, cancellationToken);

        context.Services.AddSingleton<WarmupMarker>();
    }

    /// <inheritdoc />
    public override async ValueTask ApplicationConfigurationAsync(
        ApplicationConfigurationContext context,
        CancellationToken cancellationToken = default)
    {
        // Simulate an async warmup step before the pipeline is fully active.
        await Task.Delay(1, cancellationToken);

        var logger = context.ApplicationBuilder.ApplicationServices
            .GetService<ILogger<AsyncWarmupDefinition>>();

        logger?.LogInformation(
            "AsyncWarmupDefinition.ApplicationConfigurationAsync: warmup complete (env={Environment}).",
            context.Environment?.EnvironmentName ?? "unknown");
    }
}
