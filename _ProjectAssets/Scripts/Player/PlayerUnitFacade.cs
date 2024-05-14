using Narratore.Data;
using Narratore.Components;
using UnityEngine;
using Narratore.Solutions.Battle;
using System;
using Narratore;
using System.Collections.Generic;
using Narratore.Pools;
using Narratore.WorkWithMesh;
using Narratore.Extensions;

public interface IPlayerUnitRoot
{
    Transform Root { get; }
    int EntityId { get; }
}

public interface IPlayerUnitRootAndHp : IPlayerUnitRoot
{
    Hp Hp { get; }
}

public interface IPlayerMovableUnit : IPlayerUnitRoot
{
    ReadValue<float> MoveSpeed { get; }
    TwoLegsLoopedRotators FootsAnimator { get; }
    bool IsCanMove { get; }
}

public interface IPlayerPushableUnit : IPlayerUnitRoot
{
    bool IsCanMove { get; set; }
}
public interface IPlayerUnitShooting
{
    event Action<Gun> ShootedGun;
    event Action Shooted;
    event Action RechargeTick;
    event Action Recharged;


    Vector3 Position { get; }
    int PlayerId { get; }
    int PlayerUnitId { get; }
    int LeftBullets { get; }
    int MaxBullets { get; }
    float RechargeProgress { get; }
    float MaxDistance { get; }
    IReadOnlyList<ShootArea> ShootAreas { get; }


    void TryShoot();
    void Recharge();
    IImpact GetDamage();

}


/// <summary>
/// Скрывает сложность того, что юнит и пушка являются респавнящимися, а также то, что пушки может быть 2
/// </summary>
public class PlayerUnitFacade : IPlayerUnitRoot, 
                                IPlayerMovableUnit, 
                                IPlayerUnitShooting, 
                                IPlayerPushableUnit, 
                                IPlayerUnitRootAndHp, 
                                IWithHp, 
                                IDisposable
{
    public event Action<Gun> ShootedGun;
    public event Action Shooted;
    public event Action RechargeTick;
    public event Action Recharged;
    public event Action DecreasedHp;
    public event Action ChangedHp;

    public PlayerUnitFacade(PlayerUnitSpawner unit,
                            PlayerGunSpawner mainGunSpawner,
                            PlayerGunSpawner secondGunSpawner,
                            PlayerUnitBattleRegistrator registrator,
                            IsShootingWith2Hands isShootingWith2Hand,
                            SampleData sampleData,
                            DeviceType deviceType)
    {
        _unitSpawner = unit;
        _mainGunSpawner = mainGunSpawner;
        _registrator = registrator;
        _playerId = PlayersIds.LocalPlayerId;
        _secondGunSpawner = secondGunSpawner;
        _isShootingWith2Hand = isShootingWith2Hand;
        _sampleData = sampleData;
        _deviceType = deviceType;

        _unitSpawner.Spawned += OnRespawnedUnit;
        _unitSpawner.Spawning += OnRespawningUnit;
        _mainGunSpawner.Spawned += OnRespawnedGun;
        _isShootingWith2Hand.Changed += OnChangedCountHands;

        _guns = new(2);
        _shootAreas = new(2);

        OnRespawnedGunWithoutCommonRespawn();
        OnRespawnedUnit();
    }


    private readonly PlayerUnitSpawner _unitSpawner;
    private readonly PlayerGunSpawner _mainGunSpawner;
    private readonly PlayerGunSpawner _secondGunSpawner;
    private readonly PlayerUnitBattleRegistrator _registrator;
    private readonly ReadBoolProvider _isShootingWith2Hand;
    private readonly SampleData _sampleData;
    private readonly DeviceType _deviceType;
    private readonly int _playerId;

    private PlayerUnitBattleRoster _unit;
    private List<PlayerGunRoster> _guns;
    private List<ShootArea> _shootAreas;


    public Transform Root => _unit.Root;
    public TwoLegsLoopedRotators FootsAnimator => _unit.FootsAnimator;
    public IReadOnlyList<ShootArea> ShootAreas => _shootAreas;
    public ReadValue<float> MoveSpeed => _unit.MoveSpeed;
    public Vector3 Position => Root.position;
    public int PlayerId => _playerId;
    public int PlayerUnitId => _unit.Id;
    public int EntityId => PlayerUnitId;
    public Hp Hp => _unit.Hp;
    public bool IsCanMove { get; set; }
    public int LeftBullets => _guns[0].Gun.Magazine.Size.Current;
    public int MaxBullets => _guns[0].Gun.Magazine.Size.Max;
    public float RechargeProgress => _guns[0].RechargeTimer.Progress;
    public float MaxDistance => _guns[0].MaxDistance;
    public PlayerGunRoster MainGun => _mainGunSpawner.Current;


    public void TryShoot()
    {
        for (int i = 0; i < _guns.Count; i++)
            if (_guns[i].Gun.IsCanTodoAction)
                _guns[i].Gun.Shoot();
    }

    public void Recharge()
    {
        for (int i = 0; i < _guns.Count; i++)
            _guns[i].Gun.Recharge();
    }

    public void Dispose()
    {
        _unitSpawner.Spawned -= OnRespawnedUnit;
        _unitSpawner.Spawning -= OnRespawningUnit;
        _mainGunSpawner.Spawned -= OnRespawnedGun;
        _isShootingWith2Hand.Changed -= OnChangedCountHands;

        if (_unit != null)
        {
            _unit.Hp.Decreased -= OnDecresedHp;
            _unit.Hp.Changed -= OnChangedHp;
        }

        for (int i = 0; i < _guns.Count; i++)
            _guns[i].Gun.ShootedGun -= OnShooted;
    }

    public IImpact GetDamage()
    {
        if (MainGun.IsWithExplosion)
            return new ExplosionImpact(MainGun.MinExplosionDamage, MainGun.Damage, MainGun.ExplosionRadius, ImpactTargets.Enemies);
        else
            return new ShellDamage(PlayersIds.LocalPlayerId, MainGun.Damage, ImpactTargets.Enemies);
    }


    private void OnChangedCountHands() => _mainGunSpawner.TrySpawn();

    private void OnRespawnedGun()
    {
        OnRespawnedGunWithoutCommonRespawn();
        OnRespawned();
    }

    private void OnRespawnedGunWithoutCommonRespawn()
    {
        foreach (var gun in _guns)
            gun.Gun.ShootedGun -= OnShooted;

        if (_guns.Count > 0)
        {
            _guns[0].RechargeTimer.Ticked -= OnRechargeTick;
            _guns[0].RechargeTimer.Elapsed -= OnRecharged;
        }

        _guns.Clear();
        _guns.Add(_mainGunSpawner.Current);

        _shootAreas.Clear();
        _shootAreas.Add(_mainGunSpawner.Current.ShootArea);

        _guns[0].RechargeTimer.Ticked += OnRechargeTick;
        _guns[0].RechargeTimer.Elapsed += OnRecharged;

        if (_isShootingWith2Hand.Value && _secondGunSpawner.TrySpawn())
        {
            _guns.Add(_secondGunSpawner.Current);
            _shootAreas.Add(_secondGunSpawner.Current.ShootArea);
        }

        for (int i = 0; i < _guns.Count; i++)
            _guns[i].Gun.ShootedGun += OnShooted;
    }

    private void OnRespawnedUnit()
    {
        _unit = _unitSpawner.Current.BattleRoster;
        _unit.Hp.Decreased += OnDecresedHp;
        _unit.Hp.Changed += OnChangedHp;
        _registrator.Register(_unit, _playerId);
        _sampleData.Set(_unit, _unitSpawner.CurrentPrefab.BattleRoster);

        if (_deviceType == DeviceType.Handheld)
            _unit.Hp.SetLevel(1);

        OnRespawned();
    }

    private void OnRespawningUnit()
    {
        if (_unit != null)
        {
            _unit.Hp.Decreased -= OnDecresedHp;
            _unit.Hp.Changed -= OnChangedHp;
            _registrator.Unregister(_unit);
            _sampleData.Remove(_unit);

            foreach (var gun in _guns)
                gun.Root.SetParent(null);
        }
    }

    private void OnRespawned()
    {
        PlayerGunRoster mainGun = _guns[0];
        SecondHandState.StateKey state = _guns.Count > 1 ? SecondHandState.StateKey.WithGun : SecondHandState.StateKey.Free;


        mainGun.Root.SetParent(_unit.MainGunAttach);
        mainGun.Root.localPosition = Vector3.zero;
        mainGun.Root.localRotation = Quaternion.identity;

        if (state == SecondHandState.StateKey.WithGun)
        {
            PlayerGunRoster secondGun = _guns[1];

            secondGun.Root.SetParent(_unit.SecondGunAttach);
            secondGun.Root.localPosition = Vector3.zero;
            secondGun.Root.localRotation = Quaternion.identity;
        }

        _unit.SecondHandState.Switch(state);
        _unit.MoveSpeed.SetStat(mainGun.MoveSpeed);

        mainGun.Recoil.SetTarget(_unit.GunRecoilTarget);

        if (_deviceType == DeviceType.Handheld)
            for (int i = 0; i < _shootAreas.Count; i++)
                _shootAreas[i].SetPosition(_guns[i].Gun.ShootPoint.position.WithY(0.05f));
    }

    private void OnShooted(Gun gun)
    {
        ShootedGun?.Invoke(gun);
        Shooted?.Invoke();
    }
    private void OnDecresedHp() => DecreasedHp?.Invoke();
    private void OnChangedHp() => ChangedHp?.Invoke();

    private void OnRechargeTick() => RechargeTick?.Invoke();
    private void OnRecharged() => Recharged?.Invoke();
}
