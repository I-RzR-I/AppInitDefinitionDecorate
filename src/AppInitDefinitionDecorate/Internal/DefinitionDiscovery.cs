// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:56
//  ***********************************************************************
//  <copyright file="DefinitionDiscovery.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using Microsoft.Extensions.Configuration;
using RzR.Infrastructure.BootstrapDefinition.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace RzR.Infrastructure.BootstrapDefinition.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the reflection-based discovery of <see cref="IInitDefinitionDecorate" />
    ///     implementations from a set of assemblies. Centralises logic that was previously
    ///     duplicated between
    ///     <c>ServiceCollectionDI</c> and <c>ApplicationBuilderDI</c>.
    /// </summary>
    /// =================================================================================================
    internal static class DefinitionDiscovery
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Discovers enabled <see cref="IInitDefinitionDecorate" /> implementations from the
        ///     assemblies identified by <paramref name="entryPoints" />, de-duplicates by type, applies
        ///     the optional configuration-driven enable/disable, optional type filter, and
        ///     returns definitions sorted via topological + tiebreak ordering.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="entryPoints">
        ///     Types whose assemblies are scanned. Must not be null or contain nulls.
        /// </param>
        /// <param name="configuration">
        ///     (Optional) Optional configuration for config-driven enable/disable.
        /// </param>
        /// <param name="typeFilter">
        ///     (Optional) Optional predicate; only types for which it returns true are kept.
        /// </param>
        /// <param name="diagnosticLog">(Optional) Optional sink for diagnostic messages.</param>
        /// <returns>
        ///     Ordered, deduplicated, enabled definitions ready for registration.
        /// </returns>
        /// =================================================================================================
        internal static IReadOnlyList<IInitDefinitionDecorate> Discover(
            Type [] entryPoints,
            IConfiguration configuration = null,
            Func<Type, bool> typeFilter = null,
            Action<string> diagnosticLog = null)
        {
            if (entryPoints == null) throw new ArgumentNullException(nameof(entryPoints));

            var assemblies = new HashSet<Assembly>();
            foreach (var ep in entryPoints)
            {
                if (ep == null)
                    throw new ArgumentException("Entry points must not contain null elements.", nameof(entryPoints));
                assemblies.Add(ep.Assembly);
            }

            return DiscoverFromAssemblies(assemblies, configuration, typeFilter, diagnosticLog);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Discovers enabled <see cref="IInitDefinitionDecorate" /> implementations directly from
        ///     <paramref name="assemblies" />, de-duplicates by type, applies the optional configuration-
        ///     driven
        ///     enable/disable, optional type filter, and returns definitions sorted via
        ///     topological + tiebreak ordering.
        /// </summary>
        /// <remarks>
        ///     WARNING: Scanning an assembly instantiates every matching exported type via its public
        ///     parameterless constructor. Never point this at untrusted assemblies without providing a
        ///     restrictive <paramref name="typeFilter" /> allowlist.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        /// <param name="assemblies">Assemblies to scan. Must not be null or contain nulls.</param>
        /// <param name="configuration">
        ///     (Optional) Optional configuration for config-driven enable/disable.
        /// </param>
        /// <param name="typeFilter">
        ///     (Optional) Optional predicate; only types for which it returns true are kept.
        /// </param>
        /// <param name="diagnosticLog">(Optional) Optional sink for diagnostic messages.</param>
        /// <returns>
        ///     Ordered, deduplicated, enabled definitions ready for registration.
        /// </returns>
        /// =================================================================================================
        internal static IReadOnlyList<IInitDefinitionDecorate> DiscoverFromAssemblies(
            IEnumerable<Assembly> assemblies,
            IConfiguration configuration = null,
            Func<Type, bool> typeFilter = null,
            Action<string> diagnosticLog = null)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            var seen = new HashSet<Type>();
            var all = new List<IInitDefinitionDecorate>();

            foreach (var assembly in assemblies)
            {
                if (assembly == null)
                    throw new ArgumentException("Assemblies must not contain null elements.", nameof(assemblies));

                IEnumerable<Type> exportedTypes;
                try
                {
                    exportedTypes = assembly.GetExportedTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    exportedTypes = ex.Types.Where(t => t != null)!;
                    diagnosticLog?.Invoke(
                        $"DefinitionDiscovery: ReflectionTypeLoadException while scanning assembly '{assembly.FullName}'. " +
                        $"Continuing with {exportedTypes.Count()} successfully loaded type(s).");
                }

                var candidates = exportedTypes
                    .Where(t => !t.IsAbstract && typeof(IInitDefinitionDecorate).IsAssignableFrom(t));

                foreach (var candidate in candidates)
                {
                    if (!seen.Add(candidate))
                        continue;

                    if (typeFilter != null && !typeFilter(candidate))
                    {
                        diagnosticLog?.Invoke(
                            $"DefinitionDiscovery: Filtered out '{candidate.FullName}' by typeFilter.");
                        continue;
                    }

                    IInitDefinitionDecorate instance;
                    try
                    {
                        instance = (IInitDefinitionDecorate)Activator.CreateInstance(candidate)!;
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            $"DefinitionDiscovery: Failed to create an instance of '{candidate.FullName}'. " +
                            "Ensure the type has a public parameterless constructor.", ex);
                    }

                    var isEnabled = DefinitionToggle.IsEnabled(instance, configuration, out var configOverrideApplied);

                    if (configOverrideApplied)
                    {
                        diagnosticLog?.Invoke(
                            $"DefinitionDiscovery: Config override for '{candidate.FullName}': Enabled={isEnabled} " +
                            $"(code default was IsDecorationEnable={instance.IsDecorationEnable}).");
                    }

                    if (!isEnabled)
                    {
                        diagnosticLog?.Invoke($"DefinitionDiscovery: Skipping '{candidate.FullName}' (enabled=false).");
                        continue;
                    }

                    diagnosticLog?.Invoke($"DefinitionDiscovery: Discovered '{candidate.FullName}' (order={instance.InitializeOrder}).");
                    all.Add(instance);
                }
            }

            return DefinitionOrderer.Order(all);
        }
    }
}