// ***********************************************************************
//  Assembly         : RzR.Shared.Application.TestWebApp
//  Author           : RzR
//  Created On       : 2023-02-06 08:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-08 00:10
// ***********************************************************************
//  <copyright file="EndPointsDefinition.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AppInitDefinitionDecorate.AppAndServiceImplements;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

#endregion

namespace WebApplication.ApplicationDefinition
{
    public class EndPointsDefinition : InitDefinitionDecorate
    {
        public EndPointsDefinition()
        {
            base.InitializeOrder = 1;
            base.IsDecorationEnable = true;
        }

        /// <inheritdoc />
        public override void ApplicationConfiguration(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}