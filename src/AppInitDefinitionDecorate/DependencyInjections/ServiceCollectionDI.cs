// ***********************************************************************
//  Assembly         : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author           : RzR
//  Created On       : 2023-02-02 20:17
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-03 01:29
// ***********************************************************************
//  <copyright file="ServiceCollectionDI.cs" company="">
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace AppInitDefinitionDecorate.DependencyInjections
{
    /// <summary>
    ///     Service collection dependency injection
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class ServiceCollectionDI
    {
        /// <summary>
        ///     Add application definition decorates
        /// </summary>
        /// <param name="serviceCollection">Service collection</param>
        /// <param name="entryPoints">Assembly entry point</param>
        /// <remarks></remarks>
        public static void AddDefinitionDecorates(this IServiceCollection serviceCollection, params Type[] entryPoints)
        {
            var data = GetApplicationDefinitionDecorates(entryPoints);
            foreach (var item in data)
            {
                item.ServiceConfiguration(serviceCollection);
            }
            serviceCollection.AddSingleton(data);
        }

        /// <summary>
        ///     Add application definition decorates
        /// </summary>
        /// <param name="serviceCollection">Service collection</param>
        /// <param name="configuration">Application configuration</param>
        /// <param name="entryPoints">Assembly entry point</param>
        /// <remarks></remarks>
        public static void AddDefinitionDecorates(this IServiceCollection serviceCollection,
            IConfiguration configuration,
            params Type[] entryPoints)
        {
            var data = GetApplicationDefinitionDecorates(entryPoints);
            foreach (var item in data)
            {
                item.ServiceConfiguration(serviceCollection, configuration);
            }

            serviceCollection.AddSingleton(data);
        }

        /// <summary>
        ///     Add application definition decorates
        /// </summary>
        /// <param name="serviceCollection">Service collection</param>
        /// <param name="builder">Application builder</param>
        /// <param name="entryPoints">Assembly entry point</param>
        /// <remarks></remarks>
        public static void AddDefinitionDecorates(this IServiceCollection serviceCollection,
            IApplicationBuilder builder,
            params Type[] entryPoints)
        {
            var data = GetApplicationDefinitionDecorates(entryPoints);
            foreach (var item in data)
            {
                item.ServiceConfiguration(serviceCollection, builder);
            }

            serviceCollection.AddSingleton(data);
        }

        /// <summary>
        ///     Add application definition decorates
        /// </summary>
        /// <param name="serviceCollection">Service collection</param>
        /// <param name="builder">Application builder</param>
        /// <param name="configuration">Application configuration</param>
        /// <param name="entryPoints">Assembly entry point</param>
        /// <remarks></remarks>
        public static void AddDefinitionDecorates(this IServiceCollection serviceCollection,
            IApplicationBuilder builder,
            IConfiguration configuration, params Type[] entryPoints)
        {
            var data = GetApplicationDefinitionDecorates(entryPoints);
            foreach (var item in data)
            {
                item.ServiceConfiguration(serviceCollection, builder, configuration);
            }

            serviceCollection.AddSingleton(data);
        }

        /// <summary>
        ///     Get application definitions
        /// </summary>
        /// <param name="entryPoints">Entry assembly points</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static IReadOnlyCollection<IInitDefinitionDecorate> GetApplicationDefinitionDecorates(params Type[] entryPoints)
        {
            var result = new List<IInitDefinitionDecorate>();
            foreach (var entryPoint in entryPoints)
            {
                var types = entryPoint
                    .Assembly
                    .ExportedTypes
                    .Where(type => !type.IsAbstract && typeof(IInitDefinitionDecorate).IsAssignableFrom(type));

                result
                    .AddRange(types
                        .Select(Activator.CreateInstance)
                        .Cast<IInitDefinitionDecorate>()
                        .Where(x => x.IsDecorationEnable)
                        .OrderBy(x => x.InitializeOrder));
            }

            return result;
        }
    }
}