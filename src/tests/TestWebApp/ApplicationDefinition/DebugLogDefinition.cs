// ***********************************************************************
//  Assembly         : RzR.Shared.Application.TestWebApp
//  Author           : RzR
//  Created On       : 2023-02-06 20:45
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-08 00:10
// ***********************************************************************
//  <copyright file="DebugLogDefinition.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using AppInitDefinitionDecorate.AppAndServiceImplements;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace TestWebApp.ApplicationDefinition
{
    public class DebugLogDefinition : InitDefinitionDecorate
    {
        public DebugLogDefinition()
        {
            base.InitializeOrder = 2;
            base.IsDecorationEnable = false;
        }

        /// <inheritdoc />
        public override void ServiceConfiguration(IServiceCollection services)
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<InitDefinitionDecorate>>();
            logger?.LogInformation($"DebugLogDefinition -> ApplicationConfiguration, {DateTime.Now:D}");
        }
    }
}