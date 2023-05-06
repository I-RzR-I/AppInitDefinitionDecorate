// ***********************************************************************
//  Assembly         : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author           : RzR
//  Created On       : 2023-05-05 18:31
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-05-05 18:37
// ***********************************************************************
//  <copyright file="InitDefinitionDecorateService.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AppInitDefinitionDecorate.Abstraction;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable RedundantExtendsListEntry

#endregion

namespace AppInitDefinitionDecorate.AppAndServiceImplements
{
    /// <inheritdoc cref="IInitDefinitionDecorate" />
    public abstract partial class InitDefinitionDecorate : IInitDefinitionDecorate
    {
        /// <inheritdoc />
        public virtual void ServiceConfiguration(IServiceCollection services)
        {
        }

        /// <inheritdoc />
        public virtual void ServiceConfiguration(IServiceCollection services, IConfiguration configuration)
        {
        }

        /// <inheritdoc />
        public virtual void ServiceConfiguration(IServiceCollection services, IApplicationBuilder builder)
        {
        }

        /// <inheritdoc />
        public virtual void ServiceConfiguration(
            IServiceCollection services, IApplicationBuilder builder,
            IConfiguration configuration)
        {
        }
    }
}