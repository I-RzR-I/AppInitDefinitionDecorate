// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 12:58
//  ***********************************************************************
//  <copyright file="DefinitionOrderer.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using RzR.Infrastructure.BootstrapDefinition.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace RzR.Infrastructure.BootstrapDefinition.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Computes the final execution order for a set of discovered definitions. Performs a
    ///     topological sort honoring <see cref="DependsOnAttribute" /> edges. Within topological
    ///     constraints the stable tiebreak is
    ///     <see cref="IDefinitionMetadata.InitializeOrder" /> ascending then type full name ordinal
    ///     ascending, matching the behaviour when no <see cref="DependsOnAttribute" /> attributes
    ///     exist.
    /// </summary>
    /// =================================================================================================
    internal static class DefinitionOrderer
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the definitions in the correct execution order. Throws <see cref="InvalidOperationException" />
        ///     if a dependency cycle is detected, naming the types involved.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        /// <param name="definitions">All discovered, enabled definitions to order.</param>
        /// <returns>
        ///     A new list with the same items in execution order.
        /// </returns>
        /// =================================================================================================
        internal static IReadOnlyList<IInitDefinitionDecorate> Order(
            IReadOnlyList<IInitDefinitionDecorate> definitions)
        {
            if (definitions == null)
                throw new ArgumentNullException(nameof(definitions));
            if (definitions.Count == 0)
                return Array.Empty<IInitDefinitionDecorate>();

            var byType = definitions.ToDictionary(d => d.GetType());

            // Build adjacency: edges[T] = set of T's direct dependencies (must run before T).
            var dependencies = new Dictionary<Type, HashSet<Type>>();
            foreach (var definition in definitions)
            {
                var type = definition.GetType();
                if (!dependencies.ContainsKey(type))
                    dependencies [type] = new HashSet<Type>();

                var attrs = (DependsOnAttribute [])Attribute.GetCustomAttributes(type, typeof(DependsOnAttribute), false);
                foreach (var attr in attrs)
                {
                    // Only honour edges to types actually present in this batch.
                    if (byType.ContainsKey(attr.Dependency))
                        dependencies [type].Add(attr.Dependency);
                }
            }

            var inDegree = definitions.ToDictionary(d => d.GetType(), _ => 0);
            foreach (var kvp in dependencies)
            {
                foreach (var dep in kvp.Value)
                    inDegree [kvp.Key]++;
            }

            var successors = new Dictionary<Type, List<Type>>();
            foreach (var type in byType.Keys)
                successors [type] = new List<Type>();

            foreach (var kvp in dependencies)
            foreach (var dep in kvp.Value)
                successors [dep].Add(kvp.Key);

            // Seed the queue with all nodes that have no outstanding dependencies,
            // sorted by tiebreak so the relative order is deterministic.
            var ready = new SortedSet<IInitDefinitionDecorate>(DefinitionTiebreakComparer.Instance);
            foreach (var definition in definitions)
            {
                if (inDegree [definition.GetType()] == 0)
                    ready.Add(definition);
            }

            var result = new List<IInitDefinitionDecorate>(definitions.Count);
            while (ready.Count > 0)
            {
                var first = ready.Min!;
                ready.Remove(first);
                result.Add(first);

                foreach (var successorType in successors [first.GetType()])
                {
                    inDegree [successorType]--;
                    if (inDegree [successorType] == 0)
                        ready.Add(byType [successorType]);
                }
            }

            if (result.Count != definitions.Count)
            {
                // At least one cycle — find and name it.
                var cycleTypes = byType.Keys
                    .Where(t => inDegree [t] > 0)
                    .Select(t => t.FullName ?? t.Name);

                var sb = new StringBuilder(
                    "DefinitionOrderer: Dependency cycle detected among definition types: ");
                sb.Append(string.Join(", ", cycleTypes));
                sb.Append(". Review [DependsOn] attributes on the listed types and remove circular dependencies.");

                throw new InvalidOperationException(sb.ToString());
            }

            return result;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A definition tiebreak comparer. This class cannot be inherited.
        /// </summary>
        /// <seealso cref="T:System.Collections.Generic.IComparer{RzR.Infrastructure.BootstrapDefinition.Abstraction.IInitDefinitionDecorate}"/>
        /// =================================================================================================
        private sealed class DefinitionTiebreakComparer : IComparer<IInitDefinitionDecorate>
        {
            /// -------------------------------------------------------------------------------------------------
            /// <summary>
            ///     (Immutable) the instance.
            /// </summary>
            /// =================================================================================================
            public static readonly DefinitionTiebreakComparer Instance = new();

            /// -------------------------------------------------------------------------------------------------
            /// <summary>
            ///     Compares two IInitDefinitionDecorate? objects to determine their relative ordering.
            /// </summary>
            /// <param name="x">I initialize definition decorate? to be compared.</param>
            /// <param name="y">I initialize definition decorate? to be compared.</param>
            /// <returns>
            ///     Negative if 'x' is less than 'y', 0 if they are equal, or positive if it is greater.
            /// </returns>
            /// =================================================================================================
            public int Compare(IInitDefinitionDecorate x, IInitDefinitionDecorate y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (x == null) return -1;
                if (y == null) return 1;

                var orderCmp = x.InitializeOrder.CompareTo(y.InitializeOrder);
                if (orderCmp != 0)
                    return orderCmp;

                return string.CompareOrdinal(x.GetType().FullName, y.GetType().FullName);
            }
        }
    }
}