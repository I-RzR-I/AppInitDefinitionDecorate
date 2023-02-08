// ***********************************************************************
//  Assembly         : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author           : RzR
//  Created On       : 2023-02-02 01:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-03 01:09
// ***********************************************************************
//  <copyright file="IInitDefinitionDecorate.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

namespace AppInitDefinitionDecorate.Abstraction
{
    /// <summary>
    ///     Application initialization decorate
    /// </summary>
    internal interface IInitDefinitionDecorate
    {
        /// <summary>
        ///     Gets a value indicating whether this instance will be included in application.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if this instance services will be added; otherwise, <see langword="false" />.
        /// </value>
        /// <remarks></remarks>
        bool IsDecorationEnable { get; }

        /// <summary>
        ///     Gets initialization order if exist.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        int InitializeOrder { get; }

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
        void ServiceConfiguration(IServiceCollection services, IApplicationBuilder builder,
            IConfiguration configuration);

        /// <summary>
        ///     Configuration application
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <remarks></remarks>
        void ApplicationConfiguration(IApplicationBuilder app);

        /// <summary>
        ///     Configuration application
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Application host environment</param>
        /// <remarks></remarks>
        void ApplicationConfiguration(IApplicationBuilder app, IHostEnvironment env);

        /// <summary>
        ///     Configuration application
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Application host environment</param>
        /// <param name="applicationLifetime">Host application lifetime</param>
        /// <remarks></remarks>
        void ApplicationConfiguration(IApplicationBuilder app, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime);

        /// <summary>
        ///     Configuration application
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Application host environment</param>
        /// <param name="applicationLifetime">Host application lifetime</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <remarks></remarks>
        void ApplicationConfiguration(IApplicationBuilder app, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime,
            IServiceProvider serviceProvider);
    }
}