// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:49
//  ***********************************************************************
//  <copyright file="InitDefinitionDecorateService.cs" company="RzR SOFT & TECH">
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
using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

// ReSharper disable RedundantExtendsListEntry

namespace RzR.Infrastructure.BootstrapDefinition.AppAndServiceImplements
{
    /// -------------------------------------------------------------------------------------------------
    /// <inheritdoc cref="IInitDefinitionDecorate"/>
    /// <summary>
    ///     An initialize definition decorate.
    /// </summary>
    /// =================================================================================================
    public abstract partial class InitDefinitionDecorate : IInitDefinitionDecorate
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Configures application services using the context object. The default implementation
        ///     forwards to the appropriate legacy overload so that existing subclasses that override a
        ///     legacy method continue to work without modification. Override this method in new code
        ///     instead any of the legacy overloads.
        /// </summary>
        /// <param name="context">
        ///     Context carrying the service collection and optional supplementary objects.
        /// </param>
        /// =================================================================================================
#pragma warning disable 618
        public virtual void ServiceConfiguration(ServiceConfigurationContext context)
        {
            // Forward to legacy overloads so existing subclass overrides are still called.
            // Suppress the obsolete warning because this is the designated bridge location.
            ServiceConfiguration(context.Services);

            if (context.Configuration != null)
                ServiceConfiguration(context.Services, context.Configuration);

            if (context.ApplicationBuilder != null)
                ServiceConfiguration(context.Services, context.ApplicationBuilder);

            if (context.ApplicationBuilder != null && context.Configuration != null)
                ServiceConfiguration(context.Services, context.ApplicationBuilder, context.Configuration);
        }
#pragma warning restore 618

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Configures application services asynchronously. The default implementation calls <see cref="ServiceConfiguration(ServiceConfigurationContext)" />
        ///     and returns a completed task, so subclasses that only need synchronous work do not have
        ///     to override this.
        /// </summary>
        /// <param name="context">
        ///     Context carrying the service collection and optional supplementary objects.
        /// </param>
        /// <param name="cancellationToken">(Optional) Token that can cancel the operation.</param>
        /// <returns>
        ///     A ValueTask.
        /// </returns>
        /// =================================================================================================
        public virtual ValueTask ServiceConfigurationAsync(ServiceConfigurationContext context,
            CancellationToken cancellationToken = default)
        {
            ServiceConfiguration(context);

            return default;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Legacy overload — override <see cref="ServiceConfiguration(ServiceConfigurationContext)" />
        ///     in new code.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// =================================================================================================
        [Obsolete("Override ServiceConfiguration(ServiceConfigurationContext) instead. This overload exists for backward compatibility only.")]
        public virtual void ServiceConfiguration(IServiceCollection services)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Legacy overload — override <see cref="ServiceConfiguration(ServiceConfigurationContext)" />
        ///     in new code.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Application configuration.</param>
        /// =================================================================================================
        [Obsolete("Override ServiceConfiguration(ServiceConfigurationContext) instead. This overload exists for backward compatibility only.")]
        public virtual void ServiceConfiguration(IServiceCollection services, IConfiguration configuration)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Legacy overload — override <see cref="ServiceConfiguration(ServiceConfigurationContext)" />
        ///     in new code.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="builder">Application builder.</param>
        /// =================================================================================================
        [Obsolete("Override ServiceConfiguration(ServiceConfigurationContext) instead. This overload exists for backward compatibility only.")]
        public virtual void ServiceConfiguration(IServiceCollection services, IApplicationBuilder builder)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Legacy overload — override <see cref="ServiceConfiguration(ServiceConfigurationContext)" />
        ///     in new code.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="builder">Application builder.</param>
        /// <param name="configuration">Application configuration.</param>
        /// =================================================================================================
        [Obsolete("Override ServiceConfiguration(ServiceConfigurationContext) instead. This overload exists for backward compatibility only.")]
        public virtual void ServiceConfiguration(
            IServiceCollection services, IApplicationBuilder builder,
            IConfiguration configuration)
        {
        }
    }
}