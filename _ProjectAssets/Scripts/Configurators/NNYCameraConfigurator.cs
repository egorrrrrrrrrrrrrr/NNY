using Narratore.Components;
using Narratore.CameraTools;
using Narratore.UnityUpdate;
using VContainer;
using UnityEngine;
using VContainer.Unity;
using Narratore.Pools;

namespace Narratore.DI
{
    public class NNYCameraConfigurator : LevelConfigurator
    {
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private Shaker _cameraExplosionShaker; 
        [SerializeField] private Shaker _cameraShootingShaker; 


        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            builder.Register<AlwaysMainCamera>(Lifetime.Singleton).As<ICurrentCameraGetter>().WithParameter(_cameraRoot);
            builder.RegisterEntryPoint<CameraShaking>(Lifetime.Singleton)
                .WithParameter("explosionShaker", _cameraExplosionShaker)
                .WithParameter("shootingShaker", _cameraShootingShaker);
        }
    }
}

