using Narratore;
using Narratore.DI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class RecordModeConfigurator : MainSceneConfigurator
{
    [SerializeField] private Button _button;
    [SerializeField] private LevelModeDescriptor _recordLevelMode;


    public override void Configure(IContainerBuilder builder, bool isDebugApi, bool isDebugData)
    {
        builder.RegisterEntryPoint<RecordModeStarter>(Lifetime.Singleton)
            .WithParameter(_button)
            .WithParameter(_recordLevelMode);
    }
}
