using Narratore.Solutions.Battle;
using System;
using UnityEngine;


public interface IPlayerShooting
{
    event Action GettedCommandShoot;
    event Action StartedShoot;
    event Action EndedShoot;
}

public class PlayerShooting : IDisposable, IPlayerShooting
{
    public event Action GettedCommandShoot;
    public event Action StartedShoot;
    public event Action EndedShoot;


    public PlayerShooting(IPlayerUnitShooting shooting, ShellsLifetime shellsLifetime)
    {
        _unitShooting = shooting;
        _shellsLifetime = shellsLifetime;

        _unitShooting.ShootedGun += OnShooted;
    }



    private readonly IPlayerUnitShooting _unitShooting;
    private readonly ShellsLifetime _shellsLifetime;
    private bool _isShooting;

    public void SetInput(bool isShoot)
    {
        if (isShoot)
        {
            _unitShooting.TryShoot();
            GettedCommandShoot?.Invoke();
        }
        else if (_isShooting)
        {
            _isShooting = false;
            EndedShoot?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
            _unitShooting.Recharge();
    }

    public void Dispose()
    {
        _unitShooting.ShootedGun -= OnShooted;
    }

    private void OnShooted(Gun gun)
    {
        _shellsLifetime.Shoot(  gun,
                                _unitShooting.PlayerId,
                                _unitShooting.PlayerUnitId,
                                _unitShooting.GetDamage(),
                                _unitShooting.MaxDistance);

        if (!_isShooting)
        {
            _isShooting = true;
            StartedShoot?.Invoke();
        }
    }
}
