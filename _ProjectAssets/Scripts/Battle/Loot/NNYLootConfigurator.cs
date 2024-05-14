using Narratore;
using Narratore.DI;
using Narratore.Pools;
using Narratore.Solutions.Battle;
using Narratore.UI;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class NNYLootConfigurator : LootConfigurator<NNYLootDroping, LootDeathSources>
{
    [Header("HEAL")]
    [SerializeField] private HealLootPoolConfig _healPoolConfig;

    [Header("CURRENCY")]
    [SerializeField] private CurrencyLootPoolConfig _currencyPoolConfig;
    [SerializeField] private CurrencyLootPoolConfig _currencyBossPoolConfig;
    [SerializeField] private UiCoinsFlyerPoolConfig _uiCoinsFlyerConfig;
    


    protected override void RegisterEntitiesAspectsImpl(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
    {
        builder.Register<EntitiesAspects<HealLootData>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<HealLootData>>();
        builder.Register<EntitiesAspects<CurrencyLootData>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<CurrencyLootData>>();
    }

    protected override void RegisterLootCollecting(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
    {
        builder.RegisterEntryPoint<HealLootCollecting>(Lifetime.Singleton);
        builder.RegisterEntryPoint<CurrencyLootCollecting>(Lifetime.Singleton);
    }

    protected override void RegisterSources(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
    {
        var healPool = new MBPool<HealLootRoster>(_healPoolConfig, sampleData);
        var currencyLoot = new MBPool<CurrencyLootRoster>(_currencyPoolConfig, sampleData);
        var currencyBossLoot = new MBPool<CurrencyLootRoster>(_currencyBossPoolConfig, sampleData);

        builder.RegisterInstance(new MBPool<UICoinsFlyer>(_uiCoinsFlyerConfig, sampleData)).As<IMBPool<UICoinsFlyer>, IDisposable>();

        builder.Register<LootSpawner<HealLootRoster>>(Lifetime.Scoped).As<ILootSpawner, IDisposable>()
            .WithParameter(0)
            .WithParameter(healPool);

        builder.Register<LootSpawner<CurrencyLootRoster>>(Lifetime.Scoped).As<ILootSpawner, IDisposable>()
            .WithParameter(0)
            .WithParameter(currencyLoot);

        builder.Register<LootSpawner<CurrencyLootRoster>>(Lifetime.Scoped).As<ILootSpawner, IDisposable>()
            .WithParameter(0)
            .WithParameter(currencyBossLoot);
    }

    protected override void RegisterBattleRegistrators(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
    {
        builder.Register<HealLootBattleRegistrator>(Lifetime.Singleton).As<EntityBattleRegistrator<HealLootRoster>>();
        builder.Register<CurrencyLootBattleRegistrator>(Lifetime.Singleton).As<EntityBattleRegistrator<CurrencyLootRoster>>();
    }
}
