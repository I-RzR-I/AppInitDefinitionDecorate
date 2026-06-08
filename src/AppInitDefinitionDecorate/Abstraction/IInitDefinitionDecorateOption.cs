// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 08:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:47
//  ***********************************************************************
//  <copyright file="IInitDefinitionDecorateOption.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

namespace RzR.Infrastructure.BootstrapDefinition.Abstraction
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Unified public contract for application bootstrap definitions. Combines metadata, service-
    ///     configuration, and application-configuration capabilities.
    /// </summary>
    /// =================================================================================================
    public interface IInitDefinitionDecorate : IDefinitionMetadata, IServiceDefinition, IApplicationDefinition
    {
    }
}