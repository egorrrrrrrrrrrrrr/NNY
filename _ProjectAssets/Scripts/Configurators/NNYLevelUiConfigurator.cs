using Narratore.Pools;
using Narratore.UI;
using UnityEngine;
using VContainer;

namespace Narratore.DI
{
    public class NNYLevelUiConfigurator : LevelConfigurator
    {
        [SerializeField] private ShopWindow _shopWindow;
        [SerializeField] private InfoCanvas _infoCanvas;


        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            builder.RegisterInstance(_shopWindow);

            builder.Register<InfoCanvasHandler>(Lifetime.Singleton).As<IBegunGameHandler>()
                .WithParameter(_infoCanvas);
        }
    }
}

