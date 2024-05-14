using Narratore;
using Narratore.Solutions.Battle;
using Narratore.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MobilePlayerShooting : IBeginnedUpdatable, IDisposable, IBegunGameHandler
{
    public MobilePlayerShooting(IEntitiesAspects<Transform> transfomrs,
                                IPlayerUnitShooting playerUnit,
                                PlayerShooting shooting,
                                PlayerEntitiesIds ids,
                                IPlayerUnitRotator rotator,
                                TouchArea shootArea,
                                Button rechargeButton)
    {
        _transfomrs = transfomrs;
        _playerUnit = playerUnit;
        _shooting = shooting;
        _ids = ids;
        _rotator = rotator;
        _shootArea = shootArea;
        _rechargeButton = rechargeButton;

        _rechargeButton.onClick.AddListener(Recharge);
        _playerUnit.Shooted += OnShooted;
        _playerUnit.Recharged += OnRecharged;

        OnRecharged();


    }


    private readonly IEntitiesAspects<Transform> _transfomrs;
    private readonly IPlayerUnitShooting _playerUnit;
    private readonly PlayerEntitiesIds _ids;
    private readonly IPlayerUnitRotator _rotator;
    private readonly PlayerShooting _shooting;
    private readonly TouchArea _shootArea;
    private readonly Button _rechargeButton;


    public void BegunGame(LevelConfig config)
    {
        _shootArea.gameObject.SetActive(true);
    }

    public void Dispose()
    {
        _rechargeButton.onClick.RemoveListener(Recharge);
        _playerUnit.Shooted -= OnShooted;
        _playerUnit.Recharged -= OnRecharged;
    }

    public void Tick()
    {
        if (_shootArea.IsHold)
            _rotator.Rotate(GetDirection());

        _shooting.SetInput(_shootArea.IsHold);
    }


    private Vector3 GetDirection()
    {
        Vector3 unitPosition = _playerUnit.Position;
        Vector3 nearestPosition = unitPosition + _rotator.Forward;
        float minDistance = float.MaxValue;

        foreach (var pair in _transfomrs.All)
        {
            int entityId = pair.Key;
            if (_ids.TryGetOwner(entityId, out int ownerId) && ownerId == PlayersIds.GetBotId(1) &&
                _transfomrs.TryGet(entityId, out Transform aspect))
            {
                Vector3 entityPosition = aspect.position;
                float sqrMagnitude = (entityPosition - unitPosition).sqrMagnitude;
                if (sqrMagnitude < minDistance)
                {
                    minDistance = sqrMagnitude;
                    nearestPosition = entityPosition;
                }
            }
        }

        return (nearestPosition  - unitPosition).normalized;
    }

    private void Recharge() => _playerUnit.Recharge();

    private void OnRecharged() => _rechargeButton.gameObject.SetActive(false);

    private void OnShooted() =>
        _rechargeButton.gameObject.SetActive(true);
}
