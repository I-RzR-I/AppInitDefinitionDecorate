// ***********************************************************************
//  Assembly         : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author           : RzR
//  Created On       : 2023-02-02 01:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-08 01:49
// ***********************************************************************
//  <copyright file="InitDefinitionDecorate.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using AppInitDefinitionDecorate.Abstraction;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

namespace AppInitDefinitionDecorate
{
    /// <inheritdoc cref="IInitDefinitionDecorate" />
    public abstract class InitDefinitionDecorate : IInitDefinitionDecorate
    {
        /// <inheritdoc />
        public virtual bool IsDecorationEnable { get; protected set; } = true;

        /// <inheritdoc />
        public virtual int InitializeOrder { get; protected set; } = 0;

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
        public virtual void ServiceConfiguration(IServiceCollection services, IApplicationBuilder builder,
            IConfiguration configuration)
        {
        }

        /// <inheritdoc />
        public virtual void ApplicationConfiguration(IApplicationBuilder app)
        {
        }

        /// <inheritdoc />
        public virtual void ApplicationConfiguration(IApplicationBuilder app, IHostEnvironment env)
        {
        }

        /// <inheritdoc />
        public virtual void ApplicationConfiguration(IApplicationBuilder app, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime)
        {
        }

        /// <inheritdoc />
        public virtual void ApplicationConfiguration(IApplicationBuilder app, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime,
            IServiceProvider serviceProvider)
        {
        }
    }
}