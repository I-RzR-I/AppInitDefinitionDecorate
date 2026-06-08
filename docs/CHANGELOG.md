# CHANGELOG

### **v2.0.0.7683** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 08-06-2026
### Breaking changes from 1.x:

- [DEV] - (RzR) -> The `IInitDefinitionDecorate` interface is now composed of `IDefinitionMetadata`, `IServiceDefinition`, and `IApplicationDefinition`. Implementations that previously relied on the single interface are source-compatible if they extend `InitDefinitionDecorate`; direct implementations need the new split.
- [DEV] - (RzR) -> `ServiceConfiguration` and `ApplicationConfiguration` overloads that take raw `IServiceCollection` / `IApplicationBuilder` are marked `[Obsolete]`. They still work via a bridge. Migrate to the context-based overloads (`ServiceConfigurationContext`, `ApplicationConfigurationContext`).
- [DEV] - (RzR) -> `DefinitionRegistry` (internal) is now a singleton accumulator; multiple `AddDefinitionDecorates` calls accumulate rather than shadow — this may change registration order if you relied on last-call-wins.



- [DEV] - (RzR) -> Context objects (`ServiceConfigurationContext`, `ApplicationConfigurationContext`) carrying all optional parameters in a single argument; single method dispatch eliminates the overload-pick trap.
- [DEV] - (RzR) -> Async hooks: `ServiceConfigurationAsync` / `ApplicationConfigurationAsync`; async `AddDefinitionDecoratesAsync` / `UseDefinitionDecoratesAsync`.
- [DEV] - (RzR) -> Minimal-hosting extensions: `WebApplicationBuilder.AddDefinitionDecorates` / `WebApplication.UseDefinitionDecorates` (requires net6.0+).
- [DEV] - (RzR) -> Config-driven enable/disable: `DefinitionDecorates:{Type.FullName}:Enabled` in `appsettings.json` overrides `IsDecorationEnable`.
- [DEV] - (RzR) -> Assembly/type-filter scanning): `AddDefinitionDecorates(IServiceCollection, IConfiguration, Func<Type,bool>, params Assembly[])`.
- [DEV] - (RzR) -> Topological ordering via `[DependsOn]`: annotate a definition class to guarantee it runs after its dependency, regardless of `InitializeOrder`.
- [DEV] - (RzR) -> xUnit test suite covering Discovery, Toggle, Ordering, DI behaviour, and async paths.

### Migration from 1.x:

- [FIX] - (RzR) -> Replace `ServiceConfiguration(IServiceCollection)` overrides with `ServiceConfiguration(ServiceConfigurationContext)`.
- [FIX] - (RzR) -> Replace `ApplicationConfiguration(IApplicationBuilder, ...)` overrides with `ApplicationConfiguration(ApplicationConfigurationContext)`.
- [FIX] - (RzR) -> Update `Startup.ConfigureServices` to pass `configuration` if you want config-driven toggling.
- [FIX] - (RzR) -> Optionally adopt `WebApplicationBuilder` / `WebApplication` extensions for .NET 6+ projects.

## **1.1.3.0753**
-> Fix wrong modification.<br />

## **1.1.2.0753**
-> Add option to sign the new version of the files.<br />

## **1.1.1.0553**
-> Clean up code.<br />
-> Reorganize project structure.<br />
-> Add new extension for application builder.<br />
