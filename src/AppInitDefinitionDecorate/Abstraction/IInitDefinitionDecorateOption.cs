// ***********************************************************************
//  Assembly         : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author           : RzR
//  Created On       : 2023-05-06 08:29
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-05-06 08:42
// ***********************************************************************
//  <copyright file="IInitDefinitionDecorateOption.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace AppInitDefinitionDecorate.Abstraction
{
    /// <summary>
    ///     Application initialization decorate
    /// </summary>
    internal partial interface IInitDefinitionDecorate
    {
        /// <summary>
        ///     Gets a value indicating whether this instance will be included in application.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if this instance services will be added; otherwise, <see langword="false" />.
        /// </value>
        /// <remarks></remarks>
        bool IsDecorationEnable { get; }

        /// <summary>
        ///     Gets initialization order if exist.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        int InitializeOrder { get; }
    }
}