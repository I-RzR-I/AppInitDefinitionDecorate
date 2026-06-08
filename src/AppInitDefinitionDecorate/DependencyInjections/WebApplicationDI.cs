// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:54
//  ***********************************************************************
//  <copyright file="WebApplicationDI.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#if NET6_0_OR_GREATER

#region U S I N G

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts;
using RzR.Infrastructure.BootstrapDefinition.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace RzR.Infrastructure.BootstrapDefinition.DependencyInjections
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Extension methods providing first-class support for the .NET 6+ minimal-hosting model (<see cref="WebApplicationBuilder" />
    ///     and <see cref="WebApplication" />). These overloads automatically wire <c>
    ///     builder.Services</c>, <c>builder.Configuration</c>,
    ///     <c>app.Environment</c>, <c>app.Services</c>, and <c>app.Lifetime</c> into the context
    ///     so callers do not have to pass them explicitly.
    /// </summary>
    /// =================================================================================================
    public static class WebApplicationDefinitionDependencyInjection
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Discovers and registers definition decorates from the assemblies identified by
        ///     <paramref name="entryPoints" />, automatically supplying <c>builder.Services</c> and
        ///     <c>builder.Configuration</c> to the service-configuration context.
        ///     Configuration is also used to resolve the effective enabled state of each definition via
        ///     the <c>DefinitionDecorates:{FullName}:Enabled</c> key (F2).
        /// </summary>
        /// <param name="builder">
        ///     The <see cref="WebApplicationBuilder" /> for the current application.
        /// </param>
        /// <param name="entryPoints">Types whose assemblies are scanned for definitions.</param>
        /// =================================================================================================
        public static void AddDefinitionDecorates(
            this WebApplicationBuilder builder,
            params Type [] entryPoints)
            => builder.Services.AddDefinitionDecorates(builder.Configuration, entryPoints);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Async variant of <see cref="AddDefinitionDecorates(WebApplicationBuilder, Type[])" />.
        ///     Each definition's <c>ServiceConfigurationAsync</c> is awaited in sequence.
        /// </summary>
        /// <param name="builder">
        ///     The <see cref="WebApplicationBuilder" /> for the current application.
        /// </param>
        /// <param name="cancellationToken">(Optional) Token that can cancel the operation.</param>
        /// <param name="entryPoints">Types whose assemblies are scanned for definitions.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        public static async Task AddDefinitionDecoratesAsync(
            this WebApplicationBuilder builder,
            CancellationToken cancellationToken = default,
            params Type [] entryPoints)
            => await builder.Services.AddDefinitionDecoratesAsync(
                builder.Configuration, cancellationToken, entryPoints);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Activates all registered definition decorates using the built <see cref="WebApplication" />
        ///     , automatically supplying <c>app.Environment</c>, <c>app.Services</c>, and <c>
        ///     app.Lifetime</c>
        ///     to the application-configuration context.
        /// </summary>
        /// <param name="app">The built <see cref="WebApplication" />.</param>
        /// =================================================================================================
        public static void UseDefinitionDecorates(this WebApplication app)
        {
            var context = BuildApplicationContext(app);
            UseAndActivate(app, context);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Async variant of <see cref="UseDefinitionDecorates(WebApplication)" />. Each definition's
        ///     <c>ApplicationConfigurationAsync</c> is awaited in sequence.
        /// </summary>
        /// <param name="app">The built <see cref="WebApplication" />.</param>
        /// <param name="cancellationToken">(Optional) Token that can cancel the operation.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        public static async Task UseDefinitionDecoratesAsync(
            this WebApplication app,
            CancellationToken cancellationToken = default)
        {
            var context = BuildApplicationContext(app);
            await UseAndActivateAsync(app, context, cancellationToken);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds application context.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>
        ///     An ApplicationConfigurationContext.
        /// </returns>
        /// =================================================================================================
        private static ApplicationConfigurationContext BuildApplicationContext(WebApplication app) 
            => new(app, app.Environment, app.Services, app.Lifetime);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Use and activate.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="context">The context.</param>
        /// =================================================================================================
        private static void UseAndActivate(
            WebApplication app,
            ApplicationConfigurationContext context)
        {
            var registry = ResolveWebRegistry(app);
            var logger = TryGetWebLogger(app);

            foreach (var definition in registry.Definitions)
            {
                logger?.LogInformation(
                    "UseDefinitionDecorates(WebApplication): Activating '{TypeName}' (order={Order}).",
                    definition.GetType().FullName,
                    definition.InitializeOrder);

                definition.ApplicationConfiguration(context);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Use and activate asynchronous.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        private static async Task UseAndActivateAsync(
            WebApplication app,
            ApplicationConfigurationContext context,
            CancellationToken cancellationToken)
        {
            var registry = ResolveWebRegistry(app);
            var logger = TryGetWebLogger(app);

            foreach (var definition in registry.Definitions)
            {
                logger?.LogInformation(
                    "UseDefinitionDecoratesAsync(WebApplication): Activating '{TypeName}' (order={Order}).",
                    definition.GetType().FullName,
                    definition.InitializeOrder);

                await definition.ApplicationConfigurationAsync(context, cancellationToken);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Resolve web registry.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        /// <param name="app">The application.</param>
        /// <returns>
        ///     A DefinitionRegistry.
        /// </returns>
        /// =================================================================================================
        private static DefinitionRegistry ResolveWebRegistry(WebApplication app)
        {
            var registry = app.Services.GetService(typeof(DefinitionRegistry)) as DefinitionRegistry;
            if (registry == null)
            {
                throw new InvalidOperationException(
                    "UseDefinitionDecorates requires AddDefinitionDecorates(...) to be called on the WebApplicationBuilder first.");
            }

            return registry;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attempts to get a web logger, returning a default value rather than throwing an exception
        ///     if it fails.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>
        ///     An ILogger?
        /// </returns>
        /// =================================================================================================
        private static ILogger TryGetWebLogger(WebApplication app)
        {
            try
            {
                var factory = app.Services.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
                return factory?.CreateLogger("RzR.Infrastructure.BootstrapDefinition.AppDefinitionDependencyInjection");
            }
            catch
            {
                return null;
            }
        }
    }
}

#endif