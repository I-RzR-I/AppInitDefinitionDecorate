# USING

## Quick start (minimal hosting — .NET 6+)

In `Program.cs` with the minimal hosting model:

```csharp
using RzR.Infrastructure.BootstrapDefinition.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);

// Discovers all IInitDefinitionDecorate implementations in the Program assembly.
// Configuration is automatically wired in (used for config-driven enable/disable).
builder.AddDefinitionDecorates(typeof(Program));

var app = builder.Build();

// Activates definitions; Environment, Services, and Lifetime are auto-wired.
app.UseDefinitionDecorates();

app.Run();
```

## Classic Startup class

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDefinitionDecorates(typeof(Startup));
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseDefinitionDecorates(env);
}
```

## New context-based API (recommended)

Override `ServiceConfiguration(ServiceConfigurationContext)` and/or
`ApplicationConfiguration(ApplicationConfigurationContext)` in new code.
The context objects carry all optional parameters; you no longer need to pick
from a family of overloads:

```csharp
public class MyDefinition : InitDefinitionDecorate
{
    public override void ServiceConfiguration(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IMyService, MyService>();
        // context.Configuration is available when passed by the caller
    }

    public override void ApplicationConfiguration(ApplicationConfigurationContext context)
    {
        context.ApplicationBuilder.UseRouting();
        // context.Environment, context.ServiceProvider, context.ApplicationLifetime also available
    }
}
```

### Async hooks

Override the async variants when you need awaitable work at startup:

```csharp
public class MyAsyncDefinition : InitDefinitionDecorate
{
    public override async ValueTask ServiceConfigurationAsync(
        ServiceConfigurationContext context,
        CancellationToken cancellationToken = default)
    {
        await SomeDatabaseInitAsync(cancellationToken);
    }
}
```

Use the corresponding async DI methods:

```csharp
await services.AddDefinitionDecoratesAsync(configuration, entryPoints: typeof(Startup));
await app.UseDefinitionDecoratesAsync();
```

## Legacy overloads (still supported, but obsolete)

The original overloads are marked `[Obsolete]` and still work through a bridge.
Migrate to the context API at your convenience:

```csharp
// Before (still works, produces CS0672 warning):
public override void ServiceConfiguration(IServiceCollection services) { ... }

// After (recommended):
public override void ServiceConfiguration(ServiceConfigurationContext context)
{
    context.Services.AddSingleton<IMyService, MyService>();
}
```

## Metadata: order and enable/disable

```csharp
public class MyDefinition : InitDefinitionDecorate
{
    public MyDefinition()
    {
        InitializeOrder = 10; // lower runs first; default 0
        IsDecorationEnable = true; // set false to skip this definition entirely
    }
}
```

### Config-driven enable/disable

You can override `IsDecorationEnable` from configuration at runtime without changing code.
The configuration key is:

```text
DefinitionDecorates:{Type.FullName}:Enabled
```

Example `appsettings.json`:

```json
{
  "DefinitionDecorates": {
    "MyApp.ApplicationDefinition.DiagnosticsDefinition": {
      "Enabled": false
    }
  }
}
```

The config value (a bool string `"true"` / `"false"`) takes precedence over the
code-level `IsDecorationEnable`. When no config key is present the code default is used.

## Ordering with `[DependsOn]`

When `InitializeOrder` alone is insufficient to express ordering constraints, annotate
your class with `[DependsOn]`:

```csharp
// Routing must run after Services even if they share the same InitializeOrder.
[DependsOn(typeof(ServicesDefinition))]
public class RoutingDefinition : InitDefinitionDecorate { ... }
```

Multiple `[DependsOn]` attributes are allowed on one class. The pipeline performs
a topological sort over the dependency edges; `InitializeOrder` and then type full
name (ordinal) are used as the stable tiebreak within topological constraints.

If a cycle is declared an `InvalidOperationException` is thrown at startup naming
the types involved.

## Assembly / type-filter scanning

Scan one or more explicit assemblies instead of (or in addition to) entry-point types:

```csharp
services.AddDefinitionDecorates(
    configuration: builder.Configuration,
    typeFilter: t => t.Namespace?.StartsWith("MyApp.Definitions") == true,
    assemblies: Assembly.GetExecutingAssembly());
```

**Security note:** Assembly scanning instantiates every matching exported type via its
public parameterless constructor. Never scan untrusted assemblies without a restrictive
`typeFilter` allowlist.

## Available AddDefinitionDecorates overloads

**ConfigureServices / IServiceCollection:**

| Overload | Description |
| --- | --- |
| `AddDefinitionDecorates(IServiceCollection, params Type[])` | Scan assemblies by entry-point types |
| `AddDefinitionDecorates(IServiceCollection, IConfiguration, params Type[])` | + config for toggle |
| `AddDefinitionDecorates(IServiceCollection, IApplicationBuilder, params Type[])` | + app builder in context |
| `AddDefinitionDecorates(IServiceCollection, IApplicationBuilder, IConfiguration, params Type[])` | + both |
| `AddDefinitionDecorates(IServiceCollection, IConfiguration?, Func<Type,bool>?, params Assembly[])` | Assembly scan with filter |
| `AddDefinitionDecorates(IServiceCollection, IConfiguration?, IReadOnlyList<IInitDefinitionDecorate>)` | Pre-instantiated list (explicit registration without assembly scanning) |
| `AddDefinitionDecoratesAsync(...)` | Async variants of the above |

**Minimal hosting / WebApplicationBuilder (.NET 6+):**

| Overload | Description |
| --- | --- |
| `builder.AddDefinitionDecorates(params Type[])` | Services + Configuration auto-wired |
| `builder.AddDefinitionDecoratesAsync(...)` | Async variant |

**Configure / IApplicationBuilder:**

| Overload | Description |
| --- | --- |
| `UseDefinitionDecorates(this IApplicationBuilder)` | Basic |
| `UseDefinitionDecorates(this IApplicationBuilder, IHostEnvironment)` | + env |
| `UseDefinitionDecorates(this IApplicationBuilder, IHostEnvironment, IServiceProvider)` | + env + provider |
| `UseDefinitionDecorates(this IApplicationBuilder, IHostEnvironment, IHostApplicationLifetime)` | + env + lifetime |
| `UseDefinitionDecorates(this IApplicationBuilder, IHostEnvironment, IHostApplicationLifetime, IServiceProvider)` | all |
| `UseDefinitionDecoratesAsync(...)` | Async variants |

**Minimal hosting / WebApplication (.NET 6+):**

| Overload | Description |
| --- | --- |
| `app.UseDefinitionDecorates()` | Environment + Services + Lifetime auto-wired |
| `app.UseDefinitionDecoratesAsync(...)` | Async variant |

**For detailed examples see the `NewFeaturesTestApp` project in the solution.**
