// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:49
//  ***********************************************************************
//  <copyright file="InitDefinitionDecorateOption.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using RzR.Infrastructure.BootstrapDefinition.Abstraction;

#endregion

// ReSharper disable RedundantExtendsListEntry

namespace RzR.Infrastructure.BootstrapDefinition.AppAndServiceImplements
{
    /// <inheritdoc cref="IInitDefinitionDecorate" />
    public abstract partial class InitDefinitionDecorate : IInitDefinitionDecorate
    {
        /// <inheritdoc />
        public virtual bool IsDecorationEnable { get; protected set; } = true;

        /// <inheritdoc />
        public virtual int InitializeOrder { get; protected set; } = 0;
    }
}