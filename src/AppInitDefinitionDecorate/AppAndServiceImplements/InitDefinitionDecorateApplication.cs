// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:48
//  ***********************************************************************
//  <copyright file="InitDefinitionDecorateApplication.cs" company="RzR SOFT & TECH">
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
        ///     Configures the application request pipeline using the context object. The default
        ///     implementation forwards to the appropriate legacy overload so that existing subclasses
        ///     that override a legacy method continue to work without modification. Override this method
        ///     in new code instead of any the legacy overloads.
        /// </summary>
        /// <param name="context">
        ///     Context carrying the application builder and optional supplementary objects.
        /// </param>
        /// =================================================================================================
#pragma warning disable 618
        public virtual void ApplicationConfiguration(ApplicationConfigurationContext context)
        {
            // Forward to legacy overloads so existing subclass overrides are still called.
            // Suppress the obsolete warning because this is the designated bridge location.
            ApplicationConfiguration(context.ApplicationBuilder);

            if (context.Environment != null)
                ApplicationConfiguration(context.ApplicationBuilder, context.Environment);

            if (context.Environment != null && context.ServiceProvider != null)
                ApplicationConfiguration(context.ApplicationBuilder, context.Environment, context.ServiceProvider);

            if (context.Environment != null && context.ApplicationLifetime != null)
                ApplicationConfiguration(context.ApplicationBuilder, context.Environment, context.ApplicationLifetime);

            if (context.Environment != null && context.ApplicationLifetime != null && context.ServiceProvider != null)
                ApplicationConfiguration(context.ApplicationBuilder, context.Environment, context.ApplicationLifetime, context.ServiceProvider);
        }
#pragma warning restore 618

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Configures the application request pipeline asynchronously. The default implementation
        ///     calls <see cref="ApplicationConfiguration(ApplicationConfigurationContext)" />
        ///     and returns a completed task, so subclasses that only need synchronous work do not have
        ///     to override this.
        /// </summary>
        /// <param name="context">
        ///     Context carrying the application builder and optional supplementary objects.
        /// </param>
        /// <param name="cancellationToken">(Optional) Token that can cancel the operation.</param>
        /// <returns>
        ///     A ValueTask.
        /// </returns>
        /// =================================================================================================
        public virtual ValueTask ApplicationConfigurationAsync(ApplicationConfigurationContext context, 
            CancellationToken cancellationToken = default)
        {
            ApplicationConfiguration(context);

            return default;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Legacy overload — override <see cref="ApplicationConfiguration(ApplicationConfigurationContext)" />
        ///     in new code.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// =================================================================================================
        [Obsolete("Override ApplicationConfiguration(ApplicationConfigurationContext) instead. This overload exists for backward compatibility only.")]
        public virtual void ApplicationConfiguration(IApplicationBuilder app)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Legacy overload — override <see cref="ApplicationConfiguration(ApplicationConfigurationContext)" />
        ///     in new code.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Host environment.</param>
        /// =================================================================================================
        [Obsolete("Override ApplicationConfiguration(ApplicationConfigurationContext) instead. This overload exists for backward compatibility only.")]
        public virtual void ApplicationConfiguration(IApplicationBuilder app, IHostEnvironment env)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Legacy overload — override <see cref="ApplicationConfiguration(ApplicationConfigurationContext)" />
        ///     in new code.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Host environment.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// =================================================================================================
        [Obsolete("Override ApplicationConfiguration(ApplicationConfigurationContext) instead. This overload exists for backward compatibility only.")]
        public virtual void ApplicationConfiguration(
            IApplicationBuilder app, IHostEnvironment env,
            IServiceProvider serviceProvider)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Legacy overload — override <see cref="ApplicationConfiguration(ApplicationConfigurationContext)" />
        ///     in new code.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Host environment.</param>
        /// <param name="applicationLifetime">Host application lifetime.</param>
        /// =================================================================================================
        [Obsolete("Override ApplicationConfiguration(ApplicationConfigurationContext) instead. This overload exists for backward compatibility only.")]
        public virtual void ApplicationConfiguration(
            IApplicationBuilder app, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Legacy overload — override <see cref="ApplicationConfiguration(ApplicationConfigurationContext)" />
        ///     in new code.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Host environment.</param>
        /// <param name="applicationLifetime">Host application lifetime.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// =================================================================================================
        [Obsolete("Override ApplicationConfiguration(ApplicationConfigurationContext) instead. This overload exists for backward compatibility only.")]
        public virtual void ApplicationConfiguration(
            IApplicationBuilder app, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime,
            IServiceProvider serviceProvider)
        {
        }
    }
}