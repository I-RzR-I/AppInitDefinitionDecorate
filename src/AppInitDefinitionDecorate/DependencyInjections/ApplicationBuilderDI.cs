// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:50
//  ***********************************************************************
//  <copyright file="ApplicationBuilderDI.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    ///     Extension methods that activate registered application-definition decorates during the
    ///     Configure phase.
    /// </summary>
    /// =================================================================================================
    public static partial class AppDefinitionDependencyInjection
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Activates all registered definition decorates using only the application builder.
        /// </summary>
        /// <param name="applicationBuilder">Application builder.</param>
        /// =================================================================================================
        public static void UseDefinitionDecorates(this IApplicationBuilder applicationBuilder)
        {
            var context = new ApplicationConfigurationContext(applicationBuilder);
            UseAndActivate(applicationBuilder, context);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Activates all registered definition decorates, making <paramref name="env" /> available
        ///     in each context.
        /// </summary>
        /// <param name="applicationBuilder">Application builder.</param>
        /// <param name="env">Host environment.</param>
        /// =================================================================================================
        public static void UseDefinitionDecorates(this IApplicationBuilder applicationBuilder, IHostEnvironment env)
        {
            var context = new ApplicationConfigurationContext(applicationBuilder, env);
            UseAndActivate(applicationBuilder, context);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Activates all registered definition decorates, making <paramref name="env" /> and
        ///     <paramref name="serviceProvider" /> available in each context.
        /// </summary>
        /// <param name="applicationBuilder">Application builder.</param>
        /// <param name="env">Host environment.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// =================================================================================================
        public static void UseDefinitionDecorates(
            this IApplicationBuilder applicationBuilder, IHostEnvironment env,
            IServiceProvider serviceProvider)
        {
            var context = new ApplicationConfigurationContext(applicationBuilder, env, serviceProvider);
            UseAndActivate(applicationBuilder, context);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Activates all registered definition decorates, making <paramref name="env" /> and
        ///     <paramref name="applicationLifetime" /> available in each context.
        /// </summary>
        /// <param name="applicationBuilder">Application builder.</param>
        /// <param name="env">Host environment.</param>
        /// <param name="applicationLifetime">Host application lifetime.</param>
        /// =================================================================================================
        public static void UseDefinitionDecorates(
            this IApplicationBuilder applicationBuilder, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime)
        {
            var context = new ApplicationConfigurationContext(applicationBuilder, env, applicationLifetime: applicationLifetime);
            UseAndActivate(applicationBuilder, context);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Activates all registered definition decorates, making all supplementary objects available
        ///     in each context.
        /// </summary>
        /// <param name="applicationBuilder">Application builder.</param>
        /// <param name="env">Host environment.</param>
        /// <param name="applicationLifetime">Host application lifetime.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// =================================================================================================
        public static void UseDefinitionDecorates(
            this IApplicationBuilder applicationBuilder, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider)
        {
            var context = new ApplicationConfigurationContext(applicationBuilder, env, serviceProvider, applicationLifetime);
            UseAndActivate(applicationBuilder, context);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Async variant of <see cref="UseDefinitionDecorates(IApplicationBuilder)" />. Each
        ///     definition's <c>ApplicationConfigurationAsync</c> is awaited in sequence.
        /// </summary>
        /// <param name="applicationBuilder">Application builder.</param>
        /// <param name="cancellationToken">(Optional) Token that can cancel the operation.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        public static async Task UseDefinitionDecoratesAsync(
            this IApplicationBuilder applicationBuilder,
            CancellationToken cancellationToken = default)
        {
            var context = new ApplicationConfigurationContext(applicationBuilder);
            await UseAndActivateAsync(applicationBuilder, context, cancellationToken);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Async variant of <see cref="UseDefinitionDecorates(IApplicationBuilder, IHostEnvironment)" />
        ///     .
        /// </summary>
        /// <param name="applicationBuilder">Application builder.</param>
        /// <param name="env">Host environment.</param>
        /// <param name="cancellationToken">(Optional) Token that can cancel the operation.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        public static async Task UseDefinitionDecoratesAsync(
            this IApplicationBuilder applicationBuilder, IHostEnvironment env,
            CancellationToken cancellationToken = default)
        {
            var context = new ApplicationConfigurationContext(applicationBuilder, env);
            await UseAndActivateAsync(applicationBuilder, context, cancellationToken);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Use and activate.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <param name="context">The context.</param>
        /// =================================================================================================
        private static void UseAndActivate(IApplicationBuilder applicationBuilder, ApplicationConfigurationContext context)
        {
            var registry = ResolveRegistry(applicationBuilder);
            var logger = TryGetLogger(applicationBuilder);

            foreach (var definition in registry.Definitions)
            {
                if (!definition.IsDecorationEnable)
                {
                    logger?.LogInformation(
                        "UseDefinitionDecorates: Skipping '{TypeName}' (IsDecorationEnable=false).",
                        definition.GetType().FullName);
                    continue;
                }

                logger?.LogInformation(
                    "UseDefinitionDecorates: Activating '{TypeName}' (order={Order}).",
                    definition.GetType().FullName,
                    definition.InitializeOrder);

                definition.ApplicationConfiguration(context);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Use and activate asynchronous.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        private static async Task UseAndActivateAsync(
            IApplicationBuilder applicationBuilder,
            ApplicationConfigurationContext context,
            CancellationToken cancellationToken)
        {
            var registry = ResolveRegistry(applicationBuilder);
            var logger = TryGetLogger(applicationBuilder);

            foreach (var definition in registry.Definitions)
            {
                if (!definition.IsDecorationEnable)
                {
                    logger?.LogInformation(
                        "UseDefinitionDecoratesAsync: Skipping '{TypeName}' (IsDecorationEnable=false).",
                        definition.GetType().FullName);
                    continue;
                }

                logger?.LogInformation(
                    "UseDefinitionDecoratesAsync: Activating '{TypeName}' (order={Order}).",
                    definition.GetType().FullName,
                    definition.InitializeOrder);

                await definition.ApplicationConfigurationAsync(context, cancellationToken);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Resolve registry.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <returns>
        ///     A DefinitionRegistry.
        /// </returns>
        /// =================================================================================================
        private static DefinitionRegistry ResolveRegistry(IApplicationBuilder applicationBuilder)
        {
            var registry = applicationBuilder.ApplicationServices.GetService<DefinitionRegistry>();
            if (registry == null)
            {
                throw new InvalidOperationException(
                    "UseDefinitionDecorates requires AddDefinitionDecorates(...) to be called first in ConfigureServices.");
            }

            return registry;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Attempts to get a logger, returning a default value rather than throwing an exception if
        ///     it fails.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <returns>
        ///     An ILogger?
        /// </returns>
        /// =================================================================================================
        private static ILogger TryGetLogger(IApplicationBuilder applicationBuilder)
        {
            try
            {
                var factory = applicationBuilder.ApplicationServices.GetService<ILoggerFactory>();
                return factory?.CreateLogger("RzR.Infrastructure.BootstrapDefinition.AppDefinitionDependencyInjection");
            }
            catch
            {
                return null;
            }
        }
    }
}