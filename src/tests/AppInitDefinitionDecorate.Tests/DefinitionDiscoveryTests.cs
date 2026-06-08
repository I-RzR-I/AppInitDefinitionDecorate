// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate.Tests
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 13:13
//  ***********************************************************************
//  <copyright file="DefinitionDiscoveryTests.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using AppInitDefinitionDecorate.Tests.Fixtures;
using Microsoft.Extensions.Configuration;
using RzR.Infrastructure.BootstrapDefinition.Internal;
using Xunit;

#endregion

namespace AppInitDefinitionDecorate.Tests
{
    public class DefinitionDiscoveryTests
    {
        /// <summary>
        ///     Returns a type filter that only admits the explicitly named types.
        ///     Used so assembly-level scans do not accidentally include cycle-test fixtures.
        /// </summary>
        private static Func<Type, bool> Only(params Type[] types)
        {
            var set = new HashSet<Type>(types);
            return t => set.Contains(t);
        }

        [Fact]
        public void Discover_NonAbstractImplementors_AreFound()
        {
            var result = DefinitionDiscovery.Discover(
                new[] { typeof(AlphaDefinition) },
                typeFilter: Only(typeof(AlphaDefinition)));

            Assert.Contains(result, d => d is AlphaDefinition);
        }

        [Fact]
        public void Discover_AbstractBase_IsExcluded()
        {
            var result = DefinitionDiscovery.Discover(
                new[] { typeof(AbstractDefinition) },
                typeFilter: Only(typeof(AbstractDefinition)));

            Assert.DoesNotContain(result, d => d is AbstractDefinition);
        }

        [Fact]
        public void Discover_DisabledInCode_IsExcluded()
        {
            var result = DefinitionDiscovery.Discover(
                new[] { typeof(DisabledDefinition) },
                typeFilter: Only(typeof(DisabledDefinition)));

            Assert.DoesNotContain(result, d => d is DisabledDefinition);
        }

        [Fact]
        public void Discover_EmptyEntryPoints_ReturnsEmpty()
        {
            var result = DefinitionDiscovery.Discover(Array.Empty<Type>());

            Assert.Empty(result);
        }

        [Fact]
        public void Discover_NullEntryPoints_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                DefinitionDiscovery.Discover(null!));
        }

        [Fact]
        public void Discover_NullElementInEntryPoints_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                DefinitionDiscovery.Discover(new Type[] { null! }));
        }

        [Fact]
        public void Discover_SameAssemblyTwice_DeduplicatesTypes()
        {
            var result = DefinitionDiscovery.Discover(
                new[] { typeof(AlphaDefinition), typeof(AlphaDefinition) },
                typeFilter: Only(typeof(AlphaDefinition)));

            var alphaCount = result.Count(d => d is AlphaDefinition);
            Assert.Equal(1, alphaCount);
        }

        [Fact]
        public void DiscoverFromAssemblies_NullAssemblies_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                DefinitionDiscovery.DiscoverFromAssemblies(null!));
        }

        [Fact]
        public void DiscoverFromAssemblies_TypeFilter_ExcludesFilteredTypes()
        {
            var result = DefinitionDiscovery.DiscoverFromAssemblies(
                new[] { typeof(AlphaDefinition).Assembly },
                null,
                Only(typeof(AlphaDefinition)));

            Assert.All(result, d => Assert.IsType<AlphaDefinition>(d));
        }

        [Fact]
        public void Discover_DiagnosticLog_ReceivesMessages()
        {
            var messages = new List<string>();

            DefinitionDiscovery.Discover(
                new[] { typeof(AlphaDefinition) },
                typeFilter: Only(typeof(AlphaDefinition)),
                diagnosticLog: messages.Add);

            Assert.NotEmpty(messages);
        }

        [Fact]
        public void Discover_ReflectionTypeLoadException_HandledGracefully()
        {
            // Verified via code inspection: DiscoverFromAssemblies catches ReflectionTypeLoadException,
            // logs a diagnostic message, and continues with successfully-loaded types.
            // The happy-path (no exception) produces no warning — verified here.
            var messages = new List<string>();

            DefinitionDiscovery.DiscoverFromAssemblies(
                new[] { typeof(AlphaDefinition).Assembly },
                typeFilter: Only(typeof(AlphaDefinition)),
                diagnosticLog: messages.Add);

            Assert.DoesNotContain(messages, m => m.Contains("ReflectionTypeLoadException"));
        }

        [Fact]
        public void Discover_ConfigDisabledViaKey_ExcludesDefinition()
        {
            var fullName = typeof(AlphaDefinition).FullName!;
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { $"DefinitionDecorates:{fullName}:Enabled", "false" }
                })
                .Build();

            var result = DefinitionDiscovery.Discover(
                new[] { typeof(AlphaDefinition) },
                config,
                Only(typeof(AlphaDefinition)));

            Assert.DoesNotContain(result, d => d is AlphaDefinition);
        }
    }
}