// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 08:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:46
//  ***********************************************************************
//  <copyright file="IApplicationDefinition.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace RzR.Infrastructure.BootstrapDefinition.Abstraction
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Defines the contract for configuring the request pipeline during the Use (Configure)
    ///     phase.
    /// </summary>
    /// =================================================================================================
    public interface IApplicationDefinition
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Configures the application request pipeline synchronously.
        /// </summary>
        /// <param name="context">
        ///     Context carrying the application builder and optional supplementary objects.
        /// </param>
        /// =================================================================================================
        void ApplicationConfiguration(ApplicationConfigurationContext context);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Configures the application request pipeline asynchronously. By default, implementations
        ///     may delegate to <see cref="ApplicationConfiguration" /> when no async work is needed.
        /// 
        /// </summary>
        /// <param name="context">
        ///     Context carrying the application builder and optional supplementary objects.
        /// </param>
        /// <param name="cancellationToken">(Optional) Token that can cancel the operation.</param>
        /// <returns>
        ///     A ValueTask.
        /// </returns>
        /// =================================================================================================
        ValueTask ApplicationConfigurationAsync(ApplicationConfigurationContext context,
            CancellationToken cancellationToken = default);
    }
}