// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate.Tests
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 13:13
//  ***********************************************************************
//  <copyright file="AsyncTests.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using AppInitDefinitionDecorate.Tests.Fixtures;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RzR.Infrastructure.BootstrapDefinition.Abstraction;
using RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts;
using RzR.Infrastructure.BootstrapDefinition.Internal;
using Xunit;

#endregion

namespace AppInitDefinitionDecorate.Tests
{
    /// <summary>
    ///     Tests covering the async execution paths in ServiceConfiguration and ApplicationConfiguration.
    ///     These tests invoke the definition methods directly to avoid scanning the entire test assembly
    ///     (which contains cycle fixtures that would cause an InvalidOperationException during ordering).
    /// </summary>
    public class AsyncTests
    {
        [Fact]
        public async Task ServiceConfigurationAsync_OverriddenMethod_IsAwaited()
        {
            AsyncServiceDefinition.AsyncCallCount = 0;
            var definition = new AsyncServiceDefinition();
            var services = new ServiceCollection();
            var context = new ServiceConfigurationContext(services);

            await definition.ServiceConfigurationAsync(context);

            Assert.Equal(1, AsyncServiceDefinition.AsyncCallCount);
        }

        [Fact]
        public async Task ServiceConfigurationAsync_DefaultBridge_InvokesSyncMethod()
        {
            AlphaDefinition.ServiceCallCount = 0;
            var definition = new AlphaDefinition();
            var services = new ServiceCollection();
            var context = new ServiceConfigurationContext(services);

            // AlphaDefinition only overrides ServiceConfiguration(context); the async default calls it.
            await definition.ServiceConfigurationAsync(context);

            Assert.Equal(1, AlphaDefinition.ServiceCallCount);
        }

        [Fact]
        public async Task ApplicationConfigurationAsync_OverriddenMethod_IsAwaited()
        {
            AsyncAppDefinition.AsyncAppCallCount = 0;

            var services = new ServiceCollection();
            var sp = services.BuildServiceProvider();
            var appBuilder = new ApplicationBuilder(sp);
            var context = new ApplicationConfigurationContext(appBuilder);

            var definition = new AsyncAppDefinition();
            await definition.ApplicationConfigurationAsync(context);

            Assert.Equal(1, AsyncAppDefinition.AsyncAppCallCount);
        }

        [Fact]
        public async Task DefinitionRegistry_MergedDefinitions_AsyncActivation_AwaitsAllDefinitions()
        {
            AsyncServiceDefinition.AsyncCallCount = 0;
            AlphaDefinition.ServiceCallCount = 0;

            var alpha = new AlphaDefinition();
            var asyncDef = new AsyncServiceDefinition();
            var registry = new DefinitionRegistry();
            registry.Merge(new List<IInitDefinitionDecorate>
            {
                alpha, asyncDef
            });

            var services = new ServiceCollection();
            var context = new ServiceConfigurationContext(services);

            foreach (var definition in registry.Definitions)
                await definition.ServiceConfigurationAsync(context);

            Assert.Equal(1, AlphaDefinition.ServiceCallCount);
            Assert.Equal(1, AsyncServiceDefinition.AsyncCallCount);
        }
    }
}