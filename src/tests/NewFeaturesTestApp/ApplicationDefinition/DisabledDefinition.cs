using RzR.Infrastructure.BootstrapDefinition.AppAndServiceImplements;
using RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts;

namespace NewFeaturesTestApp.ApplicationDefinition;

/// <summary>
/// Demonstrates IsDecorationEnable=false. The framework skips both ServiceConfiguration and
/// ApplicationConfiguration for this definition entirely. The bodies below must never execute;
/// they throw to make it immediately obvious if the skip logic regresses.
/// </summary>
public class DisabledDefinition : InitDefinitionDecorate
{
    public DisabledDefinition()
    {
        base.InitializeOrder = 99;
        base.IsDecorationEnable = false;
    }

    /// <inheritdoc />
    public override void ServiceConfiguration(ServiceConfigurationContext context)
    {
        throw new System.InvalidOperationException(
            "DisabledDefinition.ServiceConfiguration must never be called because IsDecorationEnable=false.");
    }

    /// <inheritdoc />
    public override void ApplicationConfiguration(ApplicationConfigurationContext context)
    {
        throw new System.InvalidOperationException(
            "DisabledDefinition.ApplicationConfiguration must never be called because IsDecorationEnable=false.");
    }
}
