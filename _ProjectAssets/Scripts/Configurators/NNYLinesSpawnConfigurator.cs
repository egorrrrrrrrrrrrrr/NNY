using Narratore.Pools;
using Narratore.Solutions.Battle;
using UnityEngine;
using VContainer;

namespace Narratore.DI
{


    public class NNYLinesSpawnConfigurator : LevelConfigurator
    {
        [Header("SPAWN POINTS")]
        [SerializeField] private RandomOutCameraHeldPointsConfig _spawnPointsConfig;
        [SerializeField] private DirectionsConfig _directionsCOnfig;


        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            builder.Register<LinesHeldPoints>(Lifetime.Singleton).As<IHeldPoints>().WithParameter(_spawnPointsConfig).WithParameter(_directionsCOnfig);
            builder.Register<LineUnitsWavesSpawner>(Lifetime.Singleton).As<IUnitsWavesSpawner>().WithParameter(PlayersIds.GetBotId(1));
        }
    }
}

