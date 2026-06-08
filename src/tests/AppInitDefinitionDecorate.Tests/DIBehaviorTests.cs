// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate.Tests
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 13:13
//  ***********************************************************************
//  <copyright file="DIBehaviorTests.cs" company="RzR SOFT & TECH">
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
using RzR.Infrastructure.BootstrapDefinition.DependencyInjections;
using RzR.Infrastructure.BootstrapDefinition.Internal;
using Xunit;

#endregion

namespace AppInitDefinitionDecorate.Tests
{
    public class DIBehaviorTests
    {
        /// <summary>Type filter restricting assembly scan to only the named types.</summary>
        private static Func<Type, bool> Only(params Type[] types)
        {
            var set = new HashSet<Type>(types);
            return t => set.Contains(t);
        }

        [Fact]
        public void AddDefinitionDecorates_BuildsRegistry_InServiceCollection()
        {
            var services = new ServiceCollection();

            services.AddDefinitionDecorates(
                null,
                Only(typeof(AlphaDefinition)),
                typeof(AlphaDefinition).Assembly);

            var sp = services.BuildServiceProvider();
            var registry = sp.GetService<DefinitionRegistry>();
            Assert.NotNull(registry);
        }

        [Fact]
        public void AddDefinitionDecorates_MultipleCalls_AccumulateNotShadow()
        {
            var services = new ServiceCollection();

            services.AddDefinitionDecorates(
                null,
                Only(typeof(AlphaDefinition)),
                typeof(AlphaDefinition).Assembly);

            services.AddDefinitionDecorates(
                null,
                Only(typeof(BetaDefinition)),
                typeof(BetaDefinition).Assembly);

            var sp = services.BuildServiceProvider();
            var registry = sp.GetService<DefinitionRegistry>();

            Assert.NotNull(registry);
            var types = registry!.Definitions.Select(d => d.GetType()).ToList();
            Assert.Contains(typeof(AlphaDefinition), types);
            Assert.Contains(typeof(BetaDefinition), types);
        }

        [Fact]
        public void AddDefinitionDecorates_SameTypeTwice_DeduplicatesInRegistry()
        {
            var services = new ServiceCollection();

            services.AddDefinitionDecorates(
                null,
                Only(typeof(AlphaDefinition)),
                typeof(AlphaDefinition).Assembly);

            services.AddDefinitionDecorates(
                null,
                Only(typeof(AlphaDefinition)),
                typeof(AlphaDefinition).Assembly);

            var sp = services.BuildServiceProvider();
            var registry = sp.GetService<DefinitionRegistry>();

            Assert.NotNull(registry);
            var alphaCount = registry!.Definitions.Count(d => d is AlphaDefinition);
            Assert.Equal(1, alphaCount);
        }

        [Fact]
        public void UseDefinitionDecorates_WithoutAdd_ThrowsInvalidOperationException()
        {
            var services = new ServiceCollection();
            var sp = services.BuildServiceProvider();

            var appBuilder = new ApplicationBuilder(sp);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                appBuilder.UseDefinitionDecorates());

            Assert.Contains("AddDefinitionDecorates", ex.Message);
        }

        [Fact]
        public void AddDefinitionDecorates_ContextMethod_IsInvokedNotLegacy()
        {
            AlphaDefinition.ServiceCallCount = 0;
            var services = new ServiceCollection();

            services.AddDefinitionDecorates(
                null,
                Only(typeof(AlphaDefinition)),
                typeof(AlphaDefinition).Assembly);

            Assert.Equal(1, AlphaDefinition.ServiceCallCount);
        }

        [Fact]
        public void AddDefinitionDecorates_LegacyOnlyOverride_IsInvokedViaBridge()
        {
            LegacyOnlyDefinition.LegacyCallCount = 0;
            var services = new ServiceCollection();

            services.AddDefinitionDecorates(
                null,
                Only(typeof(LegacyOnlyDefinition)),
                typeof(LegacyOnlyDefinition).Assembly);

            Assert.Equal(1, LegacyOnlyDefinition.LegacyCallCount);
        }

        [Fact]
        public void AddDefinitionDecorates_DisabledDefinition_IsNotInvoked()
        {
            var services = new ServiceCollection();

            // DisabledDefinition.ServiceConfiguration throws if invoked.
            var exception = Record.Exception(() =>
                services.AddDefinitionDecorates(
                    null,
                    Only(typeof(DisabledDefinition)),
                    typeof(DisabledDefinition).Assembly));

            Assert.Null(exception);
        }
    }
}