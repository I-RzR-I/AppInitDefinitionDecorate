// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate.Tests
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 13:13
//  ***********************************************************************
//  <copyright file="DefinitionToggleTests.cs" company="RzR SOFT & TECH">
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
    public class DefinitionToggleTests
    {
        private static IConfiguration BuildConfig(string key, string value)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { { key, value } })
                .Build();
        }

        [Fact]
        public void IsEnabled_NoConfig_ReturnsCodeDefault_True()
        {
            var definition = new AlphaDefinition();

            var result = DefinitionToggle.IsEnabled(definition, null, out var overrideApplied);

            Assert.True(result);
            Assert.False(overrideApplied);
        }

        [Fact]
        public void IsEnabled_NoConfig_ReturnsCodeDefault_False()
        {
            var definition = new DisabledDefinition();

            var result = DefinitionToggle.IsEnabled(definition, null, out var overrideApplied);

            Assert.False(result);
            Assert.False(overrideApplied);
        }

        [Fact]
        public void IsEnabled_ConfigFalse_OverridesCodeTrue()
        {
            var definition = new AlphaDefinition();
            var fullName = definition.GetType().FullName!;
            var config = BuildConfig($"DefinitionDecorates:{fullName}:Enabled", "false");

            var result = DefinitionToggle.IsEnabled(definition, config, out var overrideApplied);

            Assert.False(result);
            Assert.True(overrideApplied);
        }

        [Fact]
        public void IsEnabled_ConfigTrue_OverridesCodeFalse()
        {
            var definition = new DisabledDefinition();
            var fullName = definition.GetType().FullName!;
            var config = BuildConfig($"DefinitionDecorates:{fullName}:Enabled", "true");

            var result = DefinitionToggle.IsEnabled(definition, config, out var overrideApplied);

            Assert.True(result);
            Assert.True(overrideApplied);
        }

        [Fact]
        public void IsEnabled_ConfigKeyAbsent_UsesCodeDefault()
        {
            var definition = new AlphaDefinition();
            var config = BuildConfig("SomeOtherKey", "true");

            var result = DefinitionToggle.IsEnabled(definition, config, out var overrideApplied);

            Assert.True(result);
            Assert.False(overrideApplied);
        }

        [Fact]
        public void IsEnabled_ConfigKeyUnparseable_UsesCodeDefault()
        {
            var definition = new AlphaDefinition();
            var fullName = definition.GetType().FullName!;
            var config = BuildConfig($"DefinitionDecorates:{fullName}:Enabled", "not-a-bool");

            var result = DefinitionToggle.IsEnabled(definition, config, out var overrideApplied);

            Assert.True(result);
            Assert.False(overrideApplied);
        }

        [Fact]
        public void IsEnabled_ConfigOverrideApplied_FlagIsTrue()
        {
            var definition = new AlphaDefinition();
            var fullName = definition.GetType().FullName!;
            var config = BuildConfig($"DefinitionDecorates:{fullName}:Enabled", "true");

            DefinitionToggle.IsEnabled(definition, config, out var overrideApplied);

            Assert.True(overrideApplied);
        }
    }
}