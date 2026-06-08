// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 08:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:46
//  ***********************************************************************
//  <copyright file="IDefinitionMetadata.cs" company="RzR SOFT & TECH">
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
    ///     Exposes metadata that controls whether a definition participates in the bootstrap
    ///     pipeline and in which order it runs.
    /// </summary>
    /// =================================================================================================
    public interface IDefinitionMetadata
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a value indicating whether this definition is active and should be invoked. When <see langword="false" />
        ///     the definition is skipped entirely during Add and Use phases.
        /// </summary>
        /// <value>
        ///     True if this object is decoration enable, false if not.
        /// </value>
        /// =================================================================================================
        bool IsDecorationEnable { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the execution order for this definition relative to others. Lower values run first;
        ///     definitions with equal order are sorted by type full name.
        /// </summary>
        /// <value>
        ///     The initialize order.
        /// </value>
        /// =================================================================================================
        int InitializeOrder { get; }
    }
}