// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 08:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:44
//  ***********************************************************************
//  <copyright file="ServiceConfigurationContext.cs" company="RzR SOFT & TECH">
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
using System;

#endregion

namespace RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Carries all objects available to a definition during the service-configuration
    ///     (ConfigureServices) phase. Collapses the previous family of overloaded <c>
    ///     ServiceConfiguration</c> methods into a single context.
    /// </summary>
    /// =================================================================================================
    public sealed class ServiceConfigurationContext
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new <see cref="ServiceConfigurationContext" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="services">
        ///     Required service collection. Must not be <see langword="null" />.
        /// </param>
        /// <param name="configuration">
        ///     (Optional) Optional application configuration, available when the caller passes it.
        /// </param>
        /// <param name="applicationBuilder">
        ///     (Optional) Optional application builder, available when it is known at ConfigureServices
        ///     time.
        /// </param>
        /// =================================================================================================
        public ServiceConfigurationContext(
            IServiceCollection services,
            IConfiguration configuration = null,
            IApplicationBuilder applicationBuilder = null)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Configuration = configuration;
            ApplicationBuilder = applicationBuilder;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the application service collection.
        /// </summary>
        /// <value>
        ///     The services.
        /// </value>
        /// =================================================================================================
        public IServiceCollection Services { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the application configuration, or <see langword="null" /> when not provided.
        /// </summary>
        /// <value>
        ///     The configuration.
        /// </value>
        /// =================================================================================================
        public IConfiguration Configuration { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the application builder, or <see langword="null" /> when not provided. Only
        ///     available in the rare overloads where the builder is known during ConfigureServices.
        /// 
        /// </summary>
        /// <value>
        ///     The application builder.
        /// </value>
        /// =================================================================================================
        public IApplicationBuilder ApplicationBuilder { get; }
    }
}