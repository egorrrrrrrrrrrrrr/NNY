using Narratore.Abstractions;
using System;
using UnityEngine.UI;
using VContainer.Unity;

public class ShootingWith2HandsHandler : IDisposable, IInitializable
{
    public ShootingWith2HandsHandler(IsShootingWith2Hands provider, Button button, RewardedAds ads)
    {
        _provider = provider;
        _button = button;
        _ads = ads;
    }


    private readonly IsShootingWith2Hands _provider;
    private readonly Button _button;
    private readonly RewardedAds _ads;
    private bool _isShowing;


    public void Initialize()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void Dispose()
    {
        _button.onClick.RemoveListener(OnClick);
    }


    private async void OnClick()
    {
        if (_provider.Value || _isShowing) return;

        if (_ads.TryShow())
        {
            _isShowing = true;

            await _ads.ShowingTask;

            _isShowing = false;
            _provider.Set(true);
        }
    }
}
