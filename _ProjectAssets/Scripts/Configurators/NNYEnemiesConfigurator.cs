using Cysharp.Threading.Tasks;
using Narratore.AI;
using Narratore.Pools;
using Narratore.Solutions.Battle;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Narratore.DI
{
    public class NNYEnemiesConfigurator : LevelConfigurator
    {
        [Header("RECORD MODE")]
        [SerializeField] private LevelModeDescriptor _recordLevelModeKey;
        [SerializeField] private SpawnCurveWavesConfig _recordSpawnWaves;

        [Header("VIEW")]
        [SerializeField] private TMP_Text _enemiesCount;


        [Header("ENEMIES")]
        [SerializeField] private CreeperPoolConfig[] _configs;
        [SerializeField] private BossCreeperPoolConfig _bossConfig;
        [SerializeField] private float _creeperExplosionDistance;


        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            builder.RegisterEntryPoint<SpawnedUnitsCounter>(Lifetime.Singleton).WithParameter(_enemiesCount);

            LevelSpawnWavesConfig[] levels = GetComponentsInChildren<LevelSpawnWavesConfig>();
            LoopedCounter counter = new LoopedCounter(0, levels.Length - 1, config.RawLevel);
            IReadOnlyList<SpawnWavesConfig> waves = levels[counter.Current].Waves;

            builder.RegisterInstance(new NNYSpawnData(_recordSpawnWaves, waves, _recordLevelModeKey, config)).As<ISpawnData>();

            RegisterEnemiesMove(builder);
            RegisterEntitiesAspects(builder);

            RegisterCreepers(builder, sampleData);
            RegisterBossCreepers(builder, sampleData);

            builder.Register<CreeperSelfExplosionDeathSource>(Lifetime.Singleton)
                .AsSelf().As<IBeginnedUpdatable>()
                .WithParameter(_creeperExplosionDistance);

            builder.Register<BotsShooting>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(PlayersIds.GetBotId(1));
        }


       

        private void RegisterEnemiesMove(IContainerBuilder builder)
        { 
            builder.Register<SeekSteering>(Lifetime.Singleton).WithParameter(0.0001f);
            builder.Register<EnemiesMover>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterEntitiesAspects(IContainerBuilder builder)
        {
            builder.Register<EntitiesAspects<MovableBot>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<MovableBot>>();
            builder.Register<EntitiesAspects<CreeperDeathExplosion>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<CreeperDeathExplosion>>();
            builder.Register<EntitiesAspects<BotShootingConfig>>(Lifetime.Singleton).AsSelf().As<IEntitiesAspects<BotShootingConfig>>();
        }



        private void RegisterCreepers(IContainerBuilder builder, SampleData sampleData)
        {
            builder.Register<CreeperBattleRegistrator>(Lifetime.Singleton).As<EntityBattleRegistrator<CreeperRoster>>();

            for (int i = 0; i < _configs.Length; i++)
            {
                var pool = new MBPool<CreeperRoster>(_configs[i], sampleData);

                builder.Register<PoolSpawner<CreeperRoster>>(Lifetime.Scoped).As<ISpawner<CreeperRoster>, ISpawner, IDisposable>()
                    .WithParameter(pool)
                    .WithParameter(0);
            }
        }

        private void RegisterBossCreepers(IContainerBuilder builder, SampleData sampleData)
        {
            var pool = new MBPool<BossCreeperRoster>(_bossConfig, sampleData);

            builder.Register<BossCreeperBattleRegistrator>(Lifetime.Singleton).As<EntityBattleRegistrator<BossCreeperRoster>>();
            builder.Register<PoolSpawner<BossCreeperRoster>>(Lifetime.Scoped).As<ISpawner<BossCreeperRoster>, ISpawner, IDisposable>()
                .WithParameter(0)
                .WithParameter(pool);
        }
    }
}

