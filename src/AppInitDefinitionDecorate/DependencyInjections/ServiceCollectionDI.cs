// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 12:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:52
//  ***********************************************************************
//  <copyright file="ServiceCollectionDI.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RzR.Infrastructure.BootstrapDefinition.Abstraction;
using RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts;
using RzR.Infrastructure.BootstrapDefinition.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace RzR.Infrastructure.BootstrapDefinition.DependencyInjections
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Extension methods that register application-definition decorates into the DI container.
    /// </summary>
    /// =================================================================================================
    public static partial class AppDefinitionDependencyInjection
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Discovers and registers definition decorates from the assemblies identified by <paramref name="entryPoints" />
        ///     . Each definition's <c>ServiceConfiguration(ServiceConfigurationContext)</c> is called
        ///     with a context that carries only the service collection.
        /// </summary>
        /// <param name="serviceCollection">Target service collection.</param>
        /// <param name="entryPoints">Types whose assemblies are scanned for definitions.</param>
        /// =================================================================================================
        public static void AddDefinitionDecorates(this IServiceCollection serviceCollection, params Type [] entryPoints)
        {
            var context = new ServiceConfigurationContext(serviceCollection);
            AddAndRegister(serviceCollection, context, entryPoints, null, null);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Discovers and registers definition decorates, making <paramref name="configuration" />
        ///     available inside each definition's service-configuration callback. Configuration is also
        ///     used to resolve the effective enabled state of each definition via the <c>
        ///     DefinitionDecorates:{FullName}:Enabled</c>
        /// </summary>
        /// <param name="serviceCollection">Target service collection.</param>
        /// <param name="configuration">Application configuration passed into the context.</param>
        /// <param name="entryPoints">Types whose assemblies are scanned for definitions.</param>
        /// =================================================================================================
        public static void AddDefinitionDecorates(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            params Type [] entryPoints)
        {
            var context = new ServiceConfigurationContext(serviceCollection, configuration);
            AddAndRegister(serviceCollection, context, entryPoints, configuration, null);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Discovers and registers definition decorates, making <paramref name="builder" />
        ///     available inside each definition's service-configuration callback.
        /// </summary>
        /// <param name="serviceCollection">Target service collection.</param>
        /// <param name="builder">Application builder passed into the context.</param>
        /// <param name="entryPoints">Types whose assemblies are scanned for definitions.</param>
        /// =================================================================================================
        public static void AddDefinitionDecorates(
            this IServiceCollection serviceCollection,
            IApplicationBuilder builder,
            params Type [] entryPoints)
        {
            var context = new ServiceConfigurationContext(serviceCollection, applicationBuilder: builder);
            AddAndRegister(serviceCollection, context, entryPoints, null, null);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Discovers and registers definition decorates, making both <paramref name="builder" /> and
        ///     <paramref name="configuration" /> available inside each definition's service-
        ///     configuration callback.
        ///     Configuration is also used to resolve the effective enabled state.
        /// </summary>
        /// <param name="serviceCollection">Target service collection.</param>
        /// <param name="builder">Application builder passed into the context.</param>
        /// <param name="configuration">Application configuration passed into the context.</param>
        /// <param name="entryPoints">Types whose assemblies are scanned for definitions.</param>
        /// =================================================================================================
        public static void AddDefinitionDecorates(
            this IServiceCollection serviceCollection,
            IApplicationBuilder builder,
            IConfiguration configuration,
            params Type [] entryPoints)
        {
            var context = new ServiceConfigurationContext(serviceCollection, configuration, builder);
            AddAndRegister(serviceCollection, context, entryPoints, configuration, null);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Discovers and registers definition decorates from explicitly provided <paramref name="assemblies" />
        ///     . An optional <paramref name="typeFilter" /> allowlist is applied before instantiation.
        ///     Configuration is used to resolve the effective enabled state.
        /// </summary>
        /// <remarks>
        ///     WARNING: Scanning an assembly instantiates every matching exported type via its public
        ///     parameterless constructor. Never point this at untrusted assemblies without providing a
        ///     restrictive <paramref name="typeFilter" /> allowlist.
        /// </remarks>
        /// <param name="serviceCollection">Target service collection.</param>
        /// <param name="configuration">
        ///     Optional application configuration, config-driven enable/disable.
        /// </param>
        /// <param name="typeFilter">
        ///     Optional predicate; only types for which it returns true are instantiated.
        /// </param>
        /// <param name="assemblies">Assemblies to scan for definitions.</param>
        /// =================================================================================================
        public static void AddDefinitionDecorates(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            Func<Type, bool> typeFilter,
            params Assembly [] assemblies)
        {
            var context = new ServiceConfigurationContext(serviceCollection, configuration);
            AddAndRegisterFromAssemblies(serviceCollection, context, assemblies, configuration, typeFilter);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Async variant of <see cref="AddDefinitionDecorates(IServiceCollection, Type[])" />. Each
        ///     definition's <c>ServiceConfigurationAsync</c> is awaited in sequence.
        /// </summary>
        /// <param name="serviceCollection">Target service collection.</param>
        /// <param name="cancellationToken">(Optional) Token that can cancel the operation.</param>
        /// <param name="entryPoints">Types whose assemblies are scanned for definitions.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        public static async Task AddDefinitionDecoratesAsync(
            this IServiceCollection serviceCollection,
            CancellationToken cancellationToken = default,
            params Type [] entryPoints)
        {
            var context = new ServiceConfigurationContext(serviceCollection);
            await AddAndRegisterAsync(serviceCollection, context, cancellationToken, entryPoints, null, null);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Async variant of <see cref="AddDefinitionDecorates(IServiceCollection, IConfiguration, Type[])" />
        ///     . Configuration is also used to resolve the effective enabled state.
        /// </summary>
        /// <param name="serviceCollection">Target service collection.</param>
        /// <param name="configuration">Application configuration passed into the context.</param>
        /// <param name="cancellationToken">(Optional) Token that can cancel the operation.</param>
        /// <param name="entryPoints">Types whose assemblies are scanned for definitions.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        public static async Task AddDefinitionDecoratesAsync(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            CancellationToken cancellationToken = default,
            params Type [] entryPoints)
        {
            var context = new ServiceConfigurationContext(serviceCollection, configuration);
            await AddAndRegisterAsync(serviceCollection, context, cancellationToken, entryPoints, configuration, null);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Registers a pre-instantiated list of definitions without reflection-based discovery.
        ///     Instances are run through the same toggle-filter, ordering, and registry-merge path as
        ///     the reflection-based overloads. Use this overload when you want to supply definitions
        ///     explicitly rather than relying on assembly scanning.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="serviceCollection">Target service collection.</param>
        /// <param name="configuration">
        ///     Optional configuration for config-driven enable/disable.
        /// </param>
        /// <param name="preInstantiated">
        ///     Pre-instantiated definitions to register. Must not be null.
        /// </param>
        /// =================================================================================================
        public static void AddDefinitionDecorates(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            IReadOnlyList<IInitDefinitionDecorate> preInstantiated)
        {
            if (preInstantiated == null) throw new ArgumentNullException(nameof(preInstantiated));

            var context = new ServiceConfigurationContext(serviceCollection, configuration);
            var filtered = ApplyToggle(preInstantiated, configuration, DiscoveryLog);
            var ordered = DefinitionOrderer.Order(filtered);

            foreach (var definition in ordered)
                definition.ServiceConfiguration(context);

            MergeIntoRegistry(serviceCollection, ordered);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds an and register.
        /// </summary>
        /// <param name="serviceCollection">Collection of services.</param>
        /// <param name="context">The context.</param>
        /// <param name="entryPoints">The entry points.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="typeFilter">A filter specifying the type.</param>
        /// =================================================================================================
        private static void AddAndRegister(
            IServiceCollection serviceCollection,
            ServiceConfigurationContext context,
            Type [] entryPoints,
            IConfiguration configuration,
            Func<Type, bool> typeFilter)
        {
            var discovered = DefinitionDiscovery.Discover(entryPoints, configuration, typeFilter, DiscoveryLog);

            foreach (var definition in discovered)
                definition.ServiceConfiguration(context);

            MergeIntoRegistry(serviceCollection, discovered);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds an and register from assemblies.
        /// </summary>
        /// <param name="serviceCollection">Collection of services.</param>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="typeFilter">A filter specifying the type.</param>
        /// =================================================================================================
        private static void AddAndRegisterFromAssemblies(
            IServiceCollection serviceCollection,
            ServiceConfigurationContext context,
            Assembly [] assemblies,
            IConfiguration configuration,
            Func<Type, bool> typeFilter)
        {
            var discovered = DefinitionDiscovery.DiscoverFromAssemblies(assemblies, configuration, typeFilter, DiscoveryLog);

            foreach (var definition in discovered)
                definition.ServiceConfiguration(context);

            MergeIntoRegistry(serviceCollection, discovered);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds an and register asynchronous.
        /// </summary>
        /// <param name="serviceCollection">Collection of services.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <param name="entryPoints">The entry points.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="typeFilter">A filter specifying the type.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        private static async Task AddAndRegisterAsync(
            IServiceCollection serviceCollection,
            ServiceConfigurationContext context,
            CancellationToken cancellationToken,
            Type [] entryPoints,
            IConfiguration configuration,
            Func<Type, bool> typeFilter)
        {
            var discovered = DefinitionDiscovery.Discover(entryPoints, configuration, typeFilter, DiscoveryLog);

            foreach (var definition in discovered)
                await definition.ServiceConfigurationAsync(context, cancellationToken);

            MergeIntoRegistry(serviceCollection, discovered);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Applies the toggle.
        /// </summary>
        /// <param name="definitions">The definitions.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="log">The log.</param>
        /// <returns>
        ///     A list of.
        /// </returns>
        /// =================================================================================================
        private static IReadOnlyList<IInitDefinitionDecorate> ApplyToggle(
            IReadOnlyList<IInitDefinitionDecorate> definitions,
            IConfiguration configuration,
            Action<string> log)
        {
            var result = new List<IInitDefinitionDecorate>();
            foreach (var definition in definitions)
            {
                var isEnabled = DefinitionToggle.IsEnabled(definition, configuration, out var configOverrideApplied);

                if (configOverrideApplied)
                    log($"DefinitionDiscovery: Config override for '{definition.GetType().FullName}': Enabled={isEnabled} (code default was IsDecorationEnable={definition.IsDecorationEnable}).");

                if (!isEnabled)
                {
                    log($"DefinitionDiscovery: Skipping '{definition.GetType().FullName}' (enabled=false).");
                    continue;
                }

                log($"DefinitionDiscovery: Discovered '{definition.GetType().FullName}' (order={definition.InitializeOrder}) [pre-instantiated].");
                result.Add(definition);
            }

            return result;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Discovery log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// =================================================================================================
        private static void DiscoveryLog(string message) =>
            // ILogger is not yet available during ConfigureServices, so write structured output
            // to the console in the same format ASP.NET Core uses for pre-build messages.
            Console.WriteLine($"info: RzR.Infrastructure.BootstrapDefinition.AppDefinitionDependencyInjection[0]{Environment.NewLine}      {message}");

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Merge into registry.
        /// </summary>
        /// <param name="serviceCollection">Collection of services.</param>
        /// <param name="discovered">The discovered.</param>
        /// =================================================================================================
        private static void MergeIntoRegistry(
            IServiceCollection serviceCollection,
            IReadOnlyList<IInitDefinitionDecorate> discovered)
        {
            DefinitionRegistry existingRegistry = null;
            foreach (var descriptor in serviceCollection)
            {
                if (descriptor.ServiceType == typeof(DefinitionRegistry) &&
                    descriptor.ImplementationInstance is DefinitionRegistry reg)
                {
                    existingRegistry = reg;
                    break;
                }
            }

            if (existingRegistry != null)
                existingRegistry.Merge(discovered);
            else
            {
                var registry = new DefinitionRegistry();
                registry.Merge(discovered);
                serviceCollection.AddSingleton(registry);
            }
        }
    }
}