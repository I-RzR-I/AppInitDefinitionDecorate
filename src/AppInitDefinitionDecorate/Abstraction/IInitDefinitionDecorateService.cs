// ***********************************************************************
//  Assembly         : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author           : RzR
//  Created On       : 2023-02-02 01:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-05-06 08:42
// ***********************************************************************
//  <copyright file="IInitDefinitionDecorateService.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace AppInitDefinitionDecorate.Abstraction
{
    /// <summary>
    ///     Application initialization decorate
    /// </summary>
    internal partial interface IInitDefinitionDecorate
    {
        /// <summary>
        ///     Configuration application services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <remarks></remarks>
        void ServiceConfiguration(IServiceCollection services);

        /// <summary>
        ///     Configuration application services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Application configuration</param>
        /// <remarks></remarks>
        void ServiceConfiguration(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        ///     Configuration application services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="builder"></param>
        /// <remarks></remarks>
        void ServiceConfiguration(IServiceCollection services, IApplicationBuilder builder);

        /// <summary>
        ///     Configuration application services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="builder">Application builder</param>
        /// <param name="configuration">Application configuration</param>
        /// <remarks></remarks>
        void ServiceConfiguration(
            IServiceCollection services, IApplicationBuilder builder,
            IConfiguration configuration);
    }
}