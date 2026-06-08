// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 13:00
//  ***********************************************************************
//  <copyright file="DefinitionToggle.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using Microsoft.Extensions.Configuration;
using RzR.Infrastructure.BootstrapDefinition.Abstraction;

#endregion

namespace RzR.Infrastructure.BootstrapDefinition.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Determines whether a definition is enabled, honouring an optional <see cref="IConfiguration" />
    ///     override before falling back to the instance's <see cref="IDefinitionMetadata.IsDecorationEnable" />
    ///     . Convention: <c>DefinitionDecorates:{Type.FullName}:Enabled</c> (bool string).
    /// </summary>
    /// =================================================================================================
    internal static class DefinitionToggle
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the section root.
        /// </summary>
        /// =================================================================================================
        private const string SectionRoot = "DefinitionDecorates";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns <see langword="true" /> when the definition should participate in the pipeline.
        ///     When <paramref name="configuration" /> is provided, looks up
        ///     <c>DefinitionDecorates:{fullName}:Enabled</c>; if found and parseable as <see cref="bool" />
        ///     that value wins. Otherwise the definition's own <see cref="IDefinitionMetadata.IsDecorationEnable" />
        ///     is the answer.
        /// </summary>
        /// <param name="definition">The definition whose enabled-state is resolved.</param>
        /// <param name="configuration">
        ///     Optional configuration source. Pass <see langword="null" /> to skip config lookup.
        /// </param>
        /// <param name="configOverrideApplied">
        ///     [out] Set to <see langword="true" /> when a config key was found and used; <see langword="false" />
        ///     when the code default was used. Callers may log at Information only when this is <see langword="true" />
        ///     .
        /// </param>
        /// <returns>
        ///     Effective enabled state.
        /// </returns>
        /// =================================================================================================
        internal static bool IsEnabled(IInitDefinitionDecorate definition,
            IConfiguration configuration, out bool configOverrideApplied)
        {
            configOverrideApplied = false;

            if (configuration == null)
                return definition.IsDecorationEnable;

            var fullName = definition.GetType().FullName;
            if (fullName == null)
                return definition.IsDecorationEnable;

            var configValue = configuration [$"{SectionRoot}:{fullName}:Enabled"];
            if (configValue != null && bool.TryParse(configValue, out var parsed))
            {
                configOverrideApplied = true;
                return parsed;
            }

            return definition.IsDecorationEnable;
        }
    }
}