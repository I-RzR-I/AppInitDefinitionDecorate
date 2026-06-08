// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:45
//  ***********************************************************************
//  <copyright file="DependsOnAttribute.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System;

#endregion

namespace RzR.Infrastructure.BootstrapDefinition.Abstraction
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Declares that a definition depends on one or more other definitions, establishing a
    ///     directed ordering edge: the dependency runs before the annotated definition. Multiple
    ///     attributes may be applied to the same class to express several edges.
    /// </summary>
    /// <remarks>
    ///     When the bootstrap pipeline computes execution order it performs a topological sort over
    ///     all <see cref="DependsOnAttribute" /> edges. Within the topological constraints,
    ///     <see cref="IDefinitionMetadata.InitializeOrder" /> (ascending) and type full name
    ///     (ordinal ascending) are used as a stable tiebreak, so order is deterministic. If a cycle
    ///     exists among the declared edges an <see cref="InvalidOperationException" />
    ///     is thrown at startup naming the cycle — cycles are a configuration error.
    /// </remarks>
    /// <seealso cref="T:Attribute"/>
    /// =================================================================================================
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class DependsOnAttribute : Attribute
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new <see cref="DependsOnAttribute" /> declaring that the annotated
        ///     definition must run after <paramref name="dependency" />.
        /// </summary>
        /// <param name="dependency">
        ///     The definition type that must be executed before the annotated definition. Must implement
        ///     <see cref="IInitDefinitionDecorate" />.
        /// </param>
        /// =================================================================================================
        public DependsOnAttribute(Type dependency) 
            => Dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the definition type that must run before the annotated definition.
        /// </summary>
        /// <value>
        ///     The dependency.
        /// </value>
        /// =================================================================================================
        public Type Dependency { get; }
    }
}