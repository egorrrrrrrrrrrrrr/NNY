using Narratore.DI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class With2HandsShootingConfigurator : MainSceneConfigurator
{
    [SerializeField] private IsShootingWith2Hands _provider;
    [SerializeField] private Button _button;

    public override void Configure(IContainerBuilder builder, bool isDebugApi, bool isDebugData)
    {
        builder.RegisterEntryPoint<ShootingWith2HandsHandler>(Lifetime.Singleton)
            .WithParameter(_button)
            .WithParameter(_provider);
    }
}
