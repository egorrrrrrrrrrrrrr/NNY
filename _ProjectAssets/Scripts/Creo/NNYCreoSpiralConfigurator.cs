using Narratore;
using Narratore.DI;
using VContainer;
using Narratore.Pools;
using UnityEngine;
using Narratore.Solutions.Battle;

public class NNYCreoSpiralConfigurator : LevelConfigurator
{
    [SerializeField] private RandomOutCameraHeldPointsConfig _spawnPointsConfig;
    [SerializeField] private CreoSpiralPlayerUnitConfig _playerUnitConfig;


    public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
    {
        builder.Register<CircleHeldPoints>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(_spawnPointsConfig);
        builder.Register<UnitsWavesSpawner>(Lifetime.Singleton).As<IUnitsWavesSpawner>().WithParameter(PlayersIds.GetBotId(1));
        builder.Register<SpiralPlayerUnitBot>(Lifetime.Singleton).WithParameter(_playerUnitConfig).AsImplementedInterfaces();
    }
}
