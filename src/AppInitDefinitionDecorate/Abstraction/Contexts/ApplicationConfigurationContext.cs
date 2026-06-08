// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 08:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:44
//  ***********************************************************************
//  <copyright file="ApplicationConfigurationContext.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;

#endregion

namespace RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Carries all objects available to a definition during the application-configuration
    ///     (Configure) phase. Collapses the previous family of overloaded <c>
    ///     ApplicationConfiguration</c> methods into a single context.
    /// </summary>
    /// =================================================================================================
    public sealed class ApplicationConfigurationContext
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new <see cref="ApplicationConfigurationContext" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="applicationBuilder">
        ///     Required application builder. Must not be <see langword="null" />.
        /// </param>
        /// <param name="environment">(Optional) Optional host environment.</param>
        /// <param name="serviceProvider">
        ///     (Optional) Optional service provider; exposes resolved services during the Configure
        ///     phase.
        /// </param>
        /// <param name="applicationLifetime">
        ///     (Optional) Optional host application lifetime for responding to startup/shutdown events.
        /// </param>
        /// =================================================================================================
        public ApplicationConfigurationContext(
            IApplicationBuilder applicationBuilder,
            IHostEnvironment environment = null,
            IServiceProvider serviceProvider = null,
            IHostApplicationLifetime applicationLifetime = null)
        {
            ApplicationBuilder = applicationBuilder ?? throw new ArgumentNullException(nameof(applicationBuilder));
            Environment = environment;
            ServiceProvider = serviceProvider;
            ApplicationLifetime = applicationLifetime;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the application request-pipeline builder.
        /// </summary>
        /// <value>
        ///     The application builder.
        /// </value>
        /// =================================================================================================
        public IApplicationBuilder ApplicationBuilder { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the host environment, or <see langword="null" /> when not provided.
        /// </summary>
        /// <value>
        ///     The environment.
        /// </value>
        /// =================================================================================================
        public IHostEnvironment Environment { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the service provider, or <see langword="null" /> when not provided. Prefer using <see cref="IApplicationBuilder.ApplicationServices" />
        ///     as the authoritative provider;
        ///     this property is a convenience when the caller passes a separate provider.
        /// </summary>
        /// <value>
        ///     The service provider.
        /// </value>
        /// =================================================================================================
        public IServiceProvider ServiceProvider { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the host application lifetime, or <see langword="null" /> when not provided.
        /// </summary>
        /// <value>
        ///     The application lifetime.
        /// </value>
        /// =================================================================================================
        public IHostApplicationLifetime ApplicationLifetime { get; }
    }
}