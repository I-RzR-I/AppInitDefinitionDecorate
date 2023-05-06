// ***********************************************************************
//  Assembly         : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author           : RzR
//  Created On       : 2023-05-05 18:36
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-05-05 18:37
// ***********************************************************************
//  <copyright file="InitDefinitionDecorateOption.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AppInitDefinitionDecorate.Abstraction;
// ReSharper disable RedundantExtendsListEntry

#endregion

namespace AppInitDefinitionDecorate.AppAndServiceImplements
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