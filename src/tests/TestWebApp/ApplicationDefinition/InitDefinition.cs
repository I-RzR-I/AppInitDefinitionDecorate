// ***********************************************************************
//  Assembly         : RzR.Shared.Application.TestWebApp
//  Author           : RzR
//  Created On       : 2023-02-06 21:06
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-08 00:10
// ***********************************************************************
//  <copyright file="InitDefinition.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AppInitDefinitionDecorate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

namespace TestWebApp.ApplicationDefinition
{
    public class InitDefinition : InitDefinitionDecorate
    {
        public InitDefinition()
        {
            base.InitializeOrder = -1;
            base.IsDecorationEnable = true;
        }

        /// <inheritdoc />
        public override void ServiceConfiguration(IServiceCollection services)
        {
            services.AddRazorPages();
        }

        /// <inheritdoc />
        public override void ApplicationConfiguration(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
        }
    }
}