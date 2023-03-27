// ***********************************************************************
//  Assembly         : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author           : RzR
//  Created On       : 2023-02-02 00:27
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-02 01:02
// ***********************************************************************
//  <copyright file="GeneralAssemblyInfo.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Reflection;
using System.Resources;

#endregion

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("RzR ®")]
[assembly: AssemblyProduct("Application initialize definition decorate")]
[assembly: AssemblyCopyright("Copyright © 2023 RzR All rights reserved.")]
[assembly: AssemblyTrademark("® RzR™")]
[assembly: AssemblyDescription("The current repository allows you to maintain your start-up project class cleanly, and organize solution structure more clearly, simply readable, and reusable. And with the possibility to order initialize and use it or not. Using this functionality allows you to safely add new dependence, adjust existing or remove/relocate it without pain and with a few adjustments.")]

#if NET45_OR_GREATER || NET || NETSTANDARD
[assembly: AssemblyMetadata("TermsOfService", "")]

[assembly: AssemblyMetadata("ContactUrl", "")]
[assembly: AssemblyMetadata("ContactName", "RzR")]
[assembly: AssemblyMetadata("ContactEmail", "ddpRzR@hotmail.com")]
#endif

#if NETSTANDARD1_6_OR_GREATER || NET35_OR_GREATER
[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]
#endif

[assembly: AssemblyVersion("1.0.1.1707")]
[assembly: AssemblyFileVersion("1.0.1.1707")]
[assembly: AssemblyInformationalVersion("1.0.1.x")]