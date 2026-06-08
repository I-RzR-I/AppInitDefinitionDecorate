// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 08:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:47
//  ***********************************************************************
//  <copyright file="IServiceDefinition.cs" company="RzR SOFT & TECH">
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
    ///     Defines the contract for configuring application services during the Add
    ///     (ConfigureServices) phase.
    /// </summary>
    /// =================================================================================================
    public interface IServiceDefinition
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Configures application services synchronously.
        /// </summary>
        /// <param name="context">
        ///     Context carrying the service collection and optional supplementary objects.
        /// </param>
        /// =================================================================================================
        void ServiceConfiguration(ServiceConfigurationContext context);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Configures application services asynchronously. By default, implementations may delegate
        ///     to <see cref="ServiceConfiguration" /> when no async work is needed.
        /// </summary>
        /// <param name="context">
        ///     Context carrying the service collection and optional supplementary objects.
        /// </param>
        /// <param name="cancellationToken">(Optional) Token that can cancel the operation.</param>
        /// <returns>
        ///     A ValueTask.
        /// </returns>
        /// =================================================================================================
        ValueTask ServiceConfigurationAsync(ServiceConfigurationContext context, 
            CancellationToken cancellationToken = default);
    }
}