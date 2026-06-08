// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate.Tests
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 13:13
//  ***********************************************************************
//  <copyright file="DefinitionOrdererTests.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using AppInitDefinitionDecorate.Tests.Fixtures;
using RzR.Infrastructure.BootstrapDefinition.Abstraction;
using RzR.Infrastructure.BootstrapDefinition.AppAndServiceImplements;
using RzR.Infrastructure.BootstrapDefinition.Internal;
using Xunit;

#endregion

namespace AppInitDefinitionDecorate.Tests
{
    public class DefinitionOrdererTests
    {
        [Fact]
        public void Order_AscendingInitializeOrder_IsRespected()
        {
            var definitions = new List<IInitDefinitionDecorate>
            {
                new BetaDefinition(),
                new AlphaDefinition()
            };

            var result = DefinitionOrderer.Order(definitions);

            Assert.IsType<AlphaDefinition>(result[0]);
            Assert.IsType<BetaDefinition>(result[1]);
        }

        [Fact]
        public void Order_EqualOrder_StableFullNameTiebreak()
        {
            var definitions = new List<IInitDefinitionDecorate>
            {
                new ZetaEqualOrderDefinition(),
                new AardvarkEqualOrderDefinition()
            };

            var result = DefinitionOrderer.Order(definitions);

            // AardvarkEqualOrderDefinition has a FullName that sorts before ZetaEqualOrderDefinition
            Assert.IsType<AardvarkEqualOrderDefinition>(result[0]);
            Assert.IsType<ZetaEqualOrderDefinition>(result[1]);
        }

        [Fact]
        public void Order_DependsOn_ForcesDepBeforeDependent()
        {
            // DependentOnAlphaDefinition has order=0 same as Alpha, but [DependsOn(Alpha)] forces Alpha first.
            var definitions = new List<IInitDefinitionDecorate>
            {
                new DependentOnAlphaDefinition(),
                new AlphaDefinition()
            };

            var result = DefinitionOrderer.Order(definitions);

            var alphaIndex = result.ToList().FindIndex(d => d is AlphaDefinition);
            var depIndex = result.ToList().FindIndex(d => d is DependentOnAlphaDefinition);
            Assert.True(alphaIndex < depIndex,
                "AlphaDefinition must appear before DependentOnAlphaDefinition due to [DependsOn].");
        }

        [Fact]
        public void Order_Cycle_ThrowsInvalidOperationException()
        {
            var definitions = new List<IInitDefinitionDecorate>
            {
                new CycleAFixture(),
                new CycleBFixture()
            };

            var ex = Assert.Throws<InvalidOperationException>(() => DefinitionOrderer.Order(definitions));

            Assert.Contains("cycle", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Order_Cycle_MessageNamesTheCyclicTypes()
        {
            var definitions = new List<IInitDefinitionDecorate>
            {
                new CycleAFixture(),
                new CycleBFixture()
            };

            var ex = Assert.Throws<InvalidOperationException>(() => DefinitionOrderer.Order(definitions));

            Assert.Contains(nameof(CycleAFixture), ex.Message);
            Assert.Contains(nameof(CycleBFixture), ex.Message);
        }

        [Fact]
        public void Order_NoDependsOn_MatchesInitializeOrderThenFullName()
        {
            var definitions = new List<IInitDefinitionDecorate>
            {
                new ZetaEqualOrderDefinition(),
                new AardvarkEqualOrderDefinition(),
                new AlphaDefinition(),
                new BetaDefinition()
            };

            var result = DefinitionOrderer.Order(definitions);

            // Alpha (order=0) first, Beta (order=1) second, then Aardvark (order=5, full name before Zeta), Zeta last.
            Assert.IsType<AlphaDefinition>(result[0]);
            Assert.IsType<BetaDefinition>(result[1]);
            Assert.IsType<AardvarkEqualOrderDefinition>(result[2]);
            Assert.IsType<ZetaEqualOrderDefinition>(result[3]);
        }

        [Fact]
        public void Order_EmptyList_ReturnsEmpty()
        {
            var result = DefinitionOrderer.Order(new List<IInitDefinitionDecorate>());

            Assert.Empty(result);
        }

        [Fact]
        public void Order_NullArgument_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                DefinitionOrderer.Order(null!));
        }

        // Cycle fixtures are nested (non-public, non-visible outside this class) so the source
        // generator does not emit new T() calls for them — cyclic definitions must never appear
        // in the generated registration list because the orderer throws at startup.
        [DependsOn(typeof(CycleBFixture))]
        private sealed class CycleAFixture : InitDefinitionDecorate
        {
        }

        [DependsOn(typeof(CycleAFixture))]
        private sealed class CycleBFixture : InitDefinitionDecorate
        {
        }
    }
}