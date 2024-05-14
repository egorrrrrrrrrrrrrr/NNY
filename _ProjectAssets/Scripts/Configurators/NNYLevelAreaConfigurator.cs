using Narratore.Pools;
using Narratore.UI;
using Narratore.WorkWithMesh;
using UnityEngine;
using VContainer;

namespace Narratore.DI
{


    public class NNYLevelAreaConfigurator : LevelConfigurator
    {
        [SerializeField] private MeshFrame _area;
        [SerializeField] private LoopedTextFadeAnimation _warning;
        [SerializeField] private LevelAreaConfig _config;
        [SerializeField] private RandomOutCameraHeldPointsConfig _heldPointsConfig;

        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            builder.RegisterInstance(_area);

            builder.Register<RandomOutCameraHeldPoints>(Lifetime.Singleton).WithParameter(_heldPointsConfig).AsSelf();
            builder.Register<LevelAreaHandler>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces()
                .WithParameter(_config)
                .WithParameter(_warning);
        }
    }
}

