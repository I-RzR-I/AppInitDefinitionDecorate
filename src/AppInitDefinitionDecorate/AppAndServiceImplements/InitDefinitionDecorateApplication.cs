// ***********************************************************************
//  Assembly         : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author           : RzR
//  Created On       : 2023-05-05 18:33
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-05-05 18:38
// ***********************************************************************
//  <copyright file="InitDefinitionDecorateApplication.cs" company="">
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
using Microsoft.Extensions.Hosting;
// ReSharper disable RedundantExtendsListEntry

#endregion

namespace AppInitDefinitionDecorate.AppAndServiceImplements
{
    /// <inheritdoc cref="IInitDefinitionDecorate" />
    public abstract partial class InitDefinitionDecorate : IInitDefinitionDecorate
    {
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
            IServiceProvider serviceProvider)
        {
        }

        /// <inheritdoc />
        public virtual void ApplicationConfiguration(
            IApplicationBuilder app, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime)
        {
        }

        /// <inheritdoc />
        public virtual void ApplicationConfiguration(
            IApplicationBuilder app, IHostEnvironment env,
            IHostApplicationLifetime applicationLifetime,
            IServiceProvider serviceProvider)
        {
        }
    }
}