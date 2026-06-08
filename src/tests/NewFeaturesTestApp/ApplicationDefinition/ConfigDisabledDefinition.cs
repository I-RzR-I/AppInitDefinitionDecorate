using Microsoft.Extensions.Logging;
using RzR.Infrastructure.BootstrapDefinition.AppAndServiceImplements;
using RzR.Infrastructure.BootstrapDefinition.Abstraction;
using RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts;

namespace NewFeaturesTestApp.ApplicationDefinition;

/// <summary>
/// Demonstrates F2 (configuration-driven enable/disable).
/// IsDecorationEnable is true in code, but appsettings.json sets
/// DefinitionDecorates:NewFeaturesTestApp.ApplicationDefinition.ConfigDisabledDefinition:Enabled = false,
/// so the framework skips it. The bodies throw to make a regression immediately visible.
/// </summary>
public class ConfigDisabledDefinition : InitDefinitionDecorate
{
    public ConfigDisabledDefinition()
    {
        base.InitializeOrder = 50;
        base.IsDecorationEnable = true;
    }

    /// <inheritdoc />
    public override void ServiceConfiguration(ServiceConfigurationContext context)
    {
        throw new InvalidOperationException(
            "ConfigDisabledDefinition.ServiceConfiguration must never be called " +
            "because it is disabled via DefinitionDecorates config key.");
    }

    /// <inheritdoc />
    public override void ApplicationConfiguration(ApplicationConfigurationContext context)
    {
        throw new InvalidOperationException(
            "ConfigDisabledDefinition.ApplicationConfiguration must never be called " +
            "because it is disabled via DefinitionDecorates config key.");
    }
}
