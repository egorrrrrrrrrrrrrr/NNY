using Narratore.Pools;
using Narratore.Solutions.Battle;
using UnityEngine;
using VContainer;

namespace Narratore.DI
{
    public class NNYSpawnConfigurator :LevelConfigurator
    {
        [Header("SPAWN POINTS")]
        [SerializeField] private RandomOutCameraHeldPointsConfig _spawnPointsConfig; 
        
        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            if (enabled)
            {
                builder.Register<PlayerDirectionMoveOutViewHeldPoints>(Lifetime.Singleton).As<IHeldPoints>().WithParameter(_spawnPointsConfig);
                builder.Register<UnitsWavesSpawner>(Lifetime.Singleton).As<IUnitsWavesSpawner>().WithParameter(PlayersIds.GetBotId(1));
            }
        }
    }
}

