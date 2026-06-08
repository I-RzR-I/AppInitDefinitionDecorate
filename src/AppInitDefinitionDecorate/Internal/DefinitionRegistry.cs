// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:59
//  ***********************************************************************
//  <copyright file="DefinitionRegistry.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using RzR.Infrastructure.BootstrapDefinition.Abstraction;
using System.Collections.Generic;

#endregion

namespace RzR.Infrastructure.BootstrapDefinition.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Accumulates discovered <see cref="IInitDefinitionDecorate" /> instances across multiple
    ///     <c>AddDefinitionDecorates</c> calls. Registered as a singleton in the service collection
    ///     so that Use-phase resolution always sees the complete, globally-ordered set.
    /// </summary>
    /// =================================================================================================
    internal sealed class DefinitionRegistry
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The definitions.
        /// </summary>
        /// =================================================================================================
        private List<IInitDefinitionDecorate> _definitions = new();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the current ordered list of registered definitions.
        /// </summary>
        /// <value>
        ///     The definitions.
        /// </value>
        /// =================================================================================================
        public IReadOnlyList<IInitDefinitionDecorate> Definitions => _definitions;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Merges <paramref name="incoming" /> definitions into the registry, de-duplicating by type,
        ///     then re-applies the full topological + tiebreak ordering via <see cref="DefinitionOrderer" />
        ///     .
        /// </summary>
        /// <param name="incoming">Newly discovered definitions to merge in.</param>
        /// =================================================================================================
        internal void Merge(IEnumerable<IInitDefinitionDecorate> incoming)
        {
            foreach (var definition in incoming)
            {
                var definitionType = definition.GetType();
                var alreadyPresent = false;
                foreach (var existing in _definitions)
                {
                    if (existing.GetType() == definitionType)
                    {
                        alreadyPresent = true;
                        break;
                    }
                }

                if (!alreadyPresent)
                    _definitions.Add(definition);
            }

            _definitions = new List<IInitDefinitionDecorate>(DefinitionOrderer.Order(_definitions));
        }
    }
}