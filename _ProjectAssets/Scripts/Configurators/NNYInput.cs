using Narratore.Input;
using Narratore.Pools;
using Narratore.UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Narratore.DI
{
    public class NNYInput : LevelConfigurator
    {
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private Button _rechargeButton;
        [SerializeField] private TouchArea _mobileShootArea;
        [SerializeField] private TouchArea _gameTouchArea;
        [SerializeField] private CustomCursor _customCursor;
        [SerializeField] private GameObject _tapToStartLabel;
        [SerializeField] private GameObject _clickToStartLabel;

        [Header("DESKTOP")]
        [SerializeField] private LayerMask _desktopShootLayerMask;


        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            if (enabled)
            {
                if (config.DeviceType == DeviceType.Desktop)
                {
                    _customCursor.enabled = true;
                    _moveJoystick.SetAxisMode();
                    _moveJoystick.ViewJoystick = Joystick.ViewOfJoystick.AlwaysHide;
                    _rechargeButton.gameObject.SetActive(false);
                    _mobileShootArea.gameObject.SetActive(false);
                }
                else if (config.DeviceType == DeviceType.Handheld)
                {
                    _customCursor.enabled = false;
                    _moveJoystick.SetTouchMode();
                    _moveJoystick.ViewJoystick = Joystick.ViewOfJoystick.AlwaysShow;
                }
                   
                if (config.DeviceType == DeviceType.Desktop)
                {
                    if (config.IsOuterStarter)
                        _clickToStartLabel.gameObject.SetActive(true);

                    builder.Register<DesktopPlayerShooting>(Lifetime.Singleton).AsImplementedInterfaces();
                    builder.Register<DesktopPlayerUnitRotator>(Lifetime.Singleton)
                        .WithParameter(_desktopShootLayerMask)
                        .AsImplementedInterfaces();
                }
                else
                {
                    if (config.IsOuterStarter)
                        _tapToStartLabel.gameObject.SetActive(true);

                    builder.Register<MobilePlayerShooting>(Lifetime.Singleton).AsImplementedInterfaces()
                        .WithParameter(_rechargeButton)
                        .WithParameter(_mobileShootArea);
                }


                builder.Register<PlayerCharacterJoystickMover>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces().WithParameter(_moveJoystick);
                builder.RegisterInstance(_gameTouchArea);
            }
        }
    }
}

