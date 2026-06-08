// ***********************************************************************
//  Assembly          : RzR.Shared.Application.AppInitDefinitionDecorate.Tests
//  Author            : RzR
//  Created           : 07-06-2026 09:06
// 
//  Last Modified By : RzR
//  Last Modified On : 07-06-2026 13:12
//  ***********************************************************************
//  <copyright file="FakeDefinitions.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using Microsoft.Extensions.DependencyInjection;
using RzR.Infrastructure.BootstrapDefinition.Abstraction;
using RzR.Infrastructure.BootstrapDefinition.Abstraction.Contexts;
using RzR.Infrastructure.BootstrapDefinition.AppAndServiceImplements;

#endregion

namespace AppInitDefinitionDecorate.Tests.Fixtures
{
    /// <summary>Concrete definition; always enabled; order 0.</summary>
    public sealed class AlphaDefinition : InitDefinitionDecorate
    {
        public static int ServiceCallCount;
        public static int AppCallCount;

        public AlphaDefinition()
        {
            InitializeOrder = 0;
            IsDecorationEnable = true;
        }

        public override void ServiceConfiguration(ServiceConfigurationContext context)
        {
            ServiceCallCount++;
        }
    }

    /// <summary>Concrete definition; always enabled; order 1 (runs after Alpha by order).</summary>
    public sealed class BetaDefinition : InitDefinitionDecorate
    {
        public static int ServiceCallCount;

        public BetaDefinition()
        {
            InitializeOrder = 1;
            IsDecorationEnable = true;
        }

        public override void ServiceConfiguration(ServiceConfigurationContext context)
        {
            ServiceCallCount++;
        }
    }

    /// <summary>Concrete definition; disabled via IsDecorationEnable=false.</summary>
    public sealed class DisabledDefinition : InitDefinitionDecorate
    {
        public DisabledDefinition()
        {
            InitializeOrder = 0;
            IsDecorationEnable = false;
        }

        public override void ServiceConfiguration(ServiceConfigurationContext context)
        {
            throw new Exception("DisabledDefinition should never be invoked.");
        }
    }

    /// <summary>Abstract — must not be discovered.</summary>
    public abstract class AbstractDefinition : InitDefinitionDecorate
    {
    }

    /// <summary>
    ///     Definition that depends on <see cref="AlphaDefinition" />.
    ///     Even though order=0 matches Alpha, DAG forces it after Alpha.
    /// </summary>
    [DependsOn(typeof(AlphaDefinition))]
    public sealed class DependentOnAlphaDefinition : InitDefinitionDecorate
    {
        public DependentOnAlphaDefinition()
        {
            InitializeOrder = 0;
        }
    }


    /// <summary>
    ///     Definition that overrides only the legacy IServiceCollection overload (back-compat bridge test).
    /// </summary>
    public sealed class LegacyOnlyDefinition : InitDefinitionDecorate
    {
        public static int LegacyCallCount;

#pragma warning disable CS0672, CS0618
        public override void ServiceConfiguration(IServiceCollection services)
        {
            LegacyCallCount++;
        }
#pragma warning restore CS0672, CS0618
    }

    /// <summary>Definition with async service configuration.</summary>
    public sealed class AsyncServiceDefinition : InitDefinitionDecorate
    {
        public static int AsyncCallCount;

        public override async ValueTask ServiceConfigurationAsync(ServiceConfigurationContext context,
            CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            AsyncCallCount++;
        }
    }

    /// <summary>Definition with async application configuration.</summary>
    public sealed class AsyncAppDefinition : InitDefinitionDecorate
    {
        public static int AsyncAppCallCount;

        public override async ValueTask ApplicationConfigurationAsync(ApplicationConfigurationContext context,
            CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            AsyncAppCallCount++;
        }
    }

    /// <summary>
    ///     Definition with order=-1, FullName sorts after GammaOrder2.
    ///     Used to verify stable FullName tiebreak when orders match.
    /// </summary>
    public sealed class ZetaEqualOrderDefinition : InitDefinitionDecorate
    {
        public static readonly List<string> InvocationOrder = new();

        public ZetaEqualOrderDefinition()
        {
            InitializeOrder = 5;
        }

        public override void ServiceConfiguration(ServiceConfigurationContext context)
        {
            InvocationOrder.Add(GetType().Name);
        }
    }

    /// <summary>
    ///     Definition with the same order as <see cref="ZetaEqualOrderDefinition" />; FullName sorts first.
    /// </summary>
    public sealed class AardvarkEqualOrderDefinition : InitDefinitionDecorate
    {
        public AardvarkEqualOrderDefinition()
        {
            InitializeOrder = 5;
        }

        public override void ServiceConfiguration(ServiceConfigurationContext context)
        {
            ZetaEqualOrderDefinition.InvocationOrder.Add(GetType().Name);
        }
    }
}