using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using RzR.Infrastructure.BootstrapDefinition.AppAndServiceImplements;
using RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts;

namespace NewFeaturesTestApp.ApplicationDefinition;

/// <summary>
/// Demonstrates order=-1 (runs first): registers framework services during ConfigureServices,
/// then wires the core middleware pipeline during Configure.
/// Uses <see cref="ServiceConfigurationContext.Configuration"/> to show it flows through.
/// </summary>
public class ServicesDefinition : InitDefinitionDecorate
{
    public ServicesDefinition()
    {
        base.InitializeOrder = -1;
    }

    /// <inheritdoc />
    public override void ServiceConfiguration(ServiceConfigurationContext context)
    {
        context.Services.AddRazorPages();

        // Demonstrate that Configuration flows through — read a key and store it for the log below.
        _appName = context.Configuration?["ApplicationSettings:Name"] ?? "NewFeaturesTestApp";
    }

    /// <inheritdoc />
    public override void ApplicationConfiguration(ApplicationConfigurationContext context)
    {
        var app = context.ApplicationBuilder;

        if (context.Environment?.IsDevelopment() is true)
            app.UseDeveloperExceptionPage();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        // Demonstrate that Environment and ServiceProvider flow through the context.
        var logger = context.ServiceProvider?
            .GetService<ILogger<ServicesDefinition>>();

        logger?.LogInformation(
            "ServicesDefinition.ApplicationConfiguration: appName={AppName} environment={Environment}",
            _appName,
            context.Environment?.EnvironmentName ?? "unknown");
    }

    private string _appName = "NewFeaturesTestApp";
}
