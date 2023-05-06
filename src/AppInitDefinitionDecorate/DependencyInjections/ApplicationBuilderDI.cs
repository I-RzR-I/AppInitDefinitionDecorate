// ***********************************************************************
//  Assembly         : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author           : RzR
//  Created On       : 2023-02-02 20:17
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-03 01:44
// ***********************************************************************
//  <copyright file="ApplicationBuilderDI.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using AppInitDefinitionDecorate.Abstraction;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

namespace AppInitDefinitionDecorate.DependencyInjections
{
    /// <summary>
    ///     Application builder dependency injection
    /// </summary>
    public static partial class AppDefinitionDependencyInjection
    {
        /// <summary>
        ///     Use added definition decorates
        /// </summary>
        /// <param name="applicationBuilder">Application builder</param>
        /// <remarks></remarks>
        public static void UseDefinitionDecorates(this IApplicationBuilder applicationBuilder)
        {
            foreach (var item in GetApplicationDefinitionDecorates(applicationBuilder))
                item.ApplicationConfiguration(applicationBuilder);
        }

        /// <summary>
        ///     Use added definition decorates
        /// </summary>
        /// <param name="applicationBuilder">Application builder</param>
        /// <param name="env">Host environment</param>
        /// <remarks></remarks>
        public static void UseDefinitionDecorates(this IApplicationBuilder applicationBuilder, IHostEnvironment env)
        {
            foreach (var item in GetApplicationDefinitionDecorates(applicationBuilder))
                item.ApplicationConfiguration(applicationBuilder, env);
        }

        /// <summary>
        ///     Use added definition decorates
        /// </summary>
        /// <param name="applicationBuilder">Application builder</param>
        /// <param name="env">Host environment</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <remarks></remarks>
        public static void UseDefinitionDecorates(this IApplicationBuilder applicationBuilder, IHostEnvironment env,
            IServiceProvider serviceProvider)
        {
            foreach (var item in GetApplicationDefinitionDecorates(applicationBuilder))
                item.ApplicationConfiguration(applicationBuilder, env, serviceProvider);
        }

        /// <summary>
        ///     Use added definition decorates
        /// </summary>
        /// <param name="applicationBuilder">Application builder</param>
        /// <param name="env">Host environment</param>
        /// <param name="applicationLifetime">Host application lifetime</param>
        /// <remarks></remarks>
        public static void UseDefinitionDecorates(
            this IApplicationBuilder applicationBuilder, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime)
        {
            foreach (var item in GetApplicationDefinitionDecorates(applicationBuilder))
                item.ApplicationConfiguration(applicationBuilder, env, applicationLifetime);
        }

        /// <summary>
        ///     Use added definition decorates
        /// </summary>
        /// <param name="applicationBuilder">Application builder</param>
        /// <param name="env">Host environment</param>
        /// <param name="applicationLifetime">Host application lifetime</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <remarks></remarks>
        public static void UseDefinitionDecorates(
            this IApplicationBuilder applicationBuilder, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider)
        {
            foreach (var item in GetApplicationDefinitionDecorates(applicationBuilder))
                item.ApplicationConfiguration(applicationBuilder, env, applicationLifetime, serviceProvider);
        }

        /// <summary>
        ///     Get application definition decorates
        /// </summary>
        /// <param name="applicationBuilder">Application builder</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static IReadOnlyCollection<IInitDefinitionDecorate> GetApplicationDefinitionDecorates(
            IApplicationBuilder applicationBuilder)
        => applicationBuilder
            .ApplicationServices
            .GetRequiredService<IReadOnlyCollection<IInitDefinitionDecorate>>()
            .Where(x => x.IsDecorationEnable)
            .OrderBy(x => x.InitializeOrder)
            .ToList();
    }
}