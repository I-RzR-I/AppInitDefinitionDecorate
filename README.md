> **Note** This repository is developed for .netstandard2.0+

[![NuGet Version](https://img.shields.io/nuget/v/AppInitDefinitionDecorate.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/AppInitDefinitionDecorate/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/AppInitDefinitionDecorate.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/AppInitDefinitionDecorate/)

The current repository allows you to maintain your start-up project class cleanly, and organize solution structure more clearly, simply readable, and reusable. As was said previously you can use the current repository if your project was written in netstandard 2.0+. 

Using this functionality allows you to safely add new dependence, adjust existing or remove/relocate it without pain and with a few adjustments.

For correct using functionalities, you must extend the base class where are definitions for `ConfigureServices` and `Configure`. On implementation, override specified methods in dependence on parameters from your code. More details about using you can find in using document(usage.md) reflected in the repository.

No additional components or packs are required for use. So, it only needs to be added/installed in the project and can be used instantly.

**In case you wish to use it in your project, u can install the package from <a href="https://www.nuget.org/packages/AppInitDefinitionDecorate" target="_blank">nuget.org</a>** or specify what version you want:


> `Install-Package AppInitDefinitionDecorate -Version x.x.x.x`

## Content
1. [USING](docs/usage.md)
1. [CHANGELOG](docs/CHANGELOG.md)
1. [BRANCH-GUIDE](docs/branch-guide.md)