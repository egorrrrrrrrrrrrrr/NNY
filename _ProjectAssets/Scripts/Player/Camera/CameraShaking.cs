using Narratore.Components;
using Narratore.Solutions.Battle;
using System;
using UnityEngine;
using VContainer.Unity;

public class CameraShaking : IDisposable, IInitializable
{
    public CameraShaking(Shaker shootingShaker, Shaker explosionShaker, IPlayerShooting playerShooting, IReadExplosionSource explosionSource)
    {
        _shootingShaker = shootingShaker;
        _explosionShaker = explosionShaker;
        _playerShooting = playerShooting;
        _explosionSource = explosionSource;
    }


    private readonly Shaker _shootingShaker;
    private readonly Shaker _explosionShaker;
    private readonly IPlayerShooting _playerShooting;
    private readonly IReadExplosionSource _explosionSource;


    public void Initialize()
    {
        _playerShooting.StartedShoot += OnStartedShoot;
        _playerShooting.EndedShoot += OnEndedShoot;
        _explosionSource.Explosion += OnExplosion;
    }

    public void Dispose()
    {
        _playerShooting.StartedShoot -= OnStartedShoot;
        _playerShooting.EndedShoot -= OnEndedShoot;
        _explosionSource.Explosion -= OnExplosion;
    }


    private void OnStartedShoot() => _shootingShaker.ToShake();
    private void OnEndedShoot() => _shootingShaker.StopFadeOut();
    private void OnExplosion(ExplosionSource.Data _) => _explosionShaker.ToShake();
}