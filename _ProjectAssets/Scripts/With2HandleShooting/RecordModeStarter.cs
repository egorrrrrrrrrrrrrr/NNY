using Narratore;
using Narratore.GameplayLoop;
using System;
using UnityEngine.UI;
using VContainer.Unity;

public class RecordModeStarter : IDisposable, IInitializable
{
    public RecordModeStarter(Button button, GameLoop loop, LevelModeDescriptor recordLevelModeKey)
    {
        _button = button;
        _loop = loop;
        _recordLevelModeKey = recordLevelModeKey;
    }


    private readonly Button _button;
    private readonly GameLoop _loop;
    private readonly LevelModeDescriptor _recordLevelModeKey;
    

    public void Initialize()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void Dispose()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        _loop.BeginLevel(_recordLevelModeKey);
    }
}
