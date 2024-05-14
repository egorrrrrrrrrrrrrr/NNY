using Narratore;
using Narratore.CameraTools;
using Narratore.Extensions;
using Narratore.Helpers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

public class PlayerBulletsUiObserver : IInitializable, IDisposable, IBeginnedUpdatable
{
    public PlayerBulletsUiObserver( IPlayerUnitShooting unit, 
                                    TMP_Text leftBulletsLabel, 
                                    Slider rechargeSlider, 
                                    RectTransform bulletsInfoPanel, 
                                    ICurrentCameraGetter camera, 
                                    Canvas canvas)
    {
        _unit = unit;
        _leftBulletsLabel = leftBulletsLabel;
        _rechargeSlider = rechargeSlider;
        _bulletsInfoPanel = bulletsInfoPanel;
        _camera = camera;
        _canvas = canvas;
    }


    private readonly IPlayerUnitShooting _unit;
    private readonly TMP_Text _leftBulletsLabel;
    private readonly Slider _rechargeSlider;
    private readonly RectTransform _bulletsInfoPanel;
    private readonly ICurrentCameraGetter _camera;
    private readonly Canvas _canvas;



    public void Tick()
    {
        _leftBulletsLabel.text = _unit.LeftBullets.ToString();
        _bulletsInfoPanel.anchoredPosition = (UiHelper.Convert(_unit.Position, _camera.Get, _camera.Transform, _canvas.scaleFactor) + new Vector3(0, 150, 0)).To2D();
    }

    public void Dispose()
    {
        _unit.RechargeTick -= OnTickTimerRecharge;
        _unit.Recharged -= Recharged;
    }

    public void Initialize()
    {
        _unit.RechargeTick += OnTickTimerRecharge;
        _unit.Recharged += Recharged;

        _leftBulletsLabel.text = _unit.MaxBullets.ToString();
    }


    private void OnTickTimerRecharge()
    {
        if (!_rechargeSlider.gameObject.activeSelf)
            _rechargeSlider.gameObject.SetActive(true);

        _rechargeSlider.value = _unit.RechargeProgress;
    }

    private void Recharged()
    {
        _leftBulletsLabel.text = _unit.MaxBullets.ToString();
        _rechargeSlider.gameObject.SetActive(false);
    }
}
