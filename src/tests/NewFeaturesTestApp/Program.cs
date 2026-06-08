using RzR.Infrastructure.BootstrapDefinition.DependencyInjections;

// F1: Use the WebApplicationBuilder extension so Services + Configuration are
// wired automatically. Configuration also drives F2 (config-driven enable/disable).
var builder = WebApplication.CreateBuilder(args);

builder.AddDefinitionDecorates(typeof(Program));

var app = builder.Build();

// F1: Use the WebApplication extension so Environment + Services + Lifetime
// are supplied to the context automatically.
app.UseDefinitionDecorates();

app.Run();

// Ensures typeof(Program) resolves when using top-level statements.
public partial class Program { }
