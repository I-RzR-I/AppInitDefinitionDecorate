// ***********************************************************************
//  Assembly         : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author           : RzR
//  Created On       : 2023-05-06 08:31
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-05-06 08:42
// ***********************************************************************
//  <copyright file="IInitDefinitionDecorateApplication.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

#endregion

namespace AppInitDefinitionDecorate.Abstraction
{
    /// <summary>
    ///     Application initialization decorate
    /// </summary>
    internal partial interface IInitDefinitionDecorate
    {
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
        /// <param name="serviceProvider">Service provider</param>
        /// <remarks></remarks>
        void ApplicationConfiguration(IApplicationBuilder app, IHostEnvironment env, IServiceProvider serviceProvider);

        /// <summary>
        ///     Configuration application
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Application host environment</param>
        /// <param name="applicationLifetime">Host application lifetime</param>
        /// <remarks></remarks>
        void ApplicationConfiguration(
            IApplicationBuilder app, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime);

        /// <summary>
        ///     Configuration application
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Application host environment</param>
        /// <param name="applicationLifetime">Host application lifetime</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <remarks></remarks>
        void ApplicationConfiguration(
            IApplicationBuilder app, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime,
            IServiceProvider serviceProvider);
    }
}