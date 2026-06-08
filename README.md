> **Note** Multi-targets `netstandard2.0`, `netstandard2.1`, `net6.0`, and `net8.0`. Works with both the classic `Startup` model and the modern minimal-hosting (`WebApplicationBuilder`) model.

[![NuGet Version](https://img.shields.io/nuget/v/RzR.Infrastructure.BootstrapDefinition.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.Infrastructure.BootstrapDefinition/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/RzR.Infrastructure.BootstrapDefinition.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.Infrastructure.BootstrapDefinition/)

<details>

  <summary>Old version</summary>
  
[![NuGet Version](https://img.shields.io/nuget/v/AppInitDefinitionDecorate.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/AppInitDefinitionDecorate/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/AppInitDefinitionDecorate.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/AppInitDefinitionDecorate/)

</details>

<br />

The current repository allows you to maintain your start-up project class cleanly, and organize solution structure more clearly, simply readable, and reusable. As was said previously you can use the current repository if your project was written in netstandard 2.0+. 

Using this functionality allows you to safely add new dependence, adjust existing or remove/relocate it without pain and with a few adjustments.

For correct using functionalities, you must extend the base class where are definitions for `ConfigureServices` and `Configure`. On implementation, override specified methods in dependence on parameters from your code. More details about using you can find in using document(usage.md) reflected in the repository.

No additional components or packs are required for use. So, it only needs to be added/installed in the project and can be used instantly.

**In case you wish to use it in your project, u can install the package from <a href="https://www.nuget.org/packages/RzR.Infrastructure.BootstrapDefinition" target="_blank">nuget.org</a>** or specify what version you want:


> `Install-Package RzR.Infrastructure.BootstrapDefinition -Version x.x.x.x`

## Highlights (v2.0)

- **Context-based API** — override `ServiceConfiguration(ServiceConfigurationContext)` / `ApplicationConfiguration(ApplicationConfigurationContext)`; a single argument carries all optional parameters (no more overload guessing). Legacy overloads remain available but are `[Obsolete]` and bridged.
- **Minimal-hosting support** — first-class `builder.AddDefinitionDecorates(...)` / `app.UseDefinitionDecorates()` for `WebApplicationBuilder`/`WebApplication` on .NET 6+.
- **Async hooks** — `ServiceConfigurationAsync` / `ApplicationConfigurationAsync` plus async `Add`/`Use` extensions.
- **Config-driven enable/disable** — toggle a definition from `appsettings.json` via `DefinitionDecorates:{Type.FullName}:Enabled` (overrides the code default).
- **`[DependsOn]` ordering** — declare that one definition must run after another; resolved by a topological sort with cycle detection, with `InitializeOrder` then type name as the stable tiebreak.
- **Plugin-safe scanning** — assembly + `Func<Type,bool>` allow-list overloads (with a security note about scanning untrusted assemblies).
- **Startup diagnostics** — logs which definitions were discovered, skipped (and why), and their activation order.
- **Robustness fixes** — `ReflectionTypeLoadException` handling, null guards, type de-duplication, deterministic ordering, accumulating (non-shadowing) registration, and actionable errors.

See [usage.md](docs/usage.md) for full examples and the runnable `NewFeaturesTestApp` sample.

## Content
1. [USING](docs/usage.md)
2. [CHANGELOG](docs/CHANGELOG.md)
3. [BRANCH-GUIDE](docs/branch-guide.md)