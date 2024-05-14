using DG.Tweening;
using Narratore.Extensions;
using Narratore.Pools;
using Narratore.Solutions.Battle;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;



/// <summary>
/// Не доделано, так как нужно еще как то делать плавное движение камеры, которая следует за игроком
/// </summary>
public class PlayerUnitShootRecoil : IDisposable, IInitializable
{
    public PlayerUnitShootRecoil(IReadOnlyList<ShootingPushConfig> shootingPushConfig,
                                    IEntitiesAspects<EntityRoster> entities,
                                    IPlayerPushableUnit pushable,
                                    ShellsLifetime shells,
                                    ISampleData sampleData,
                                    ICameraMover camera)
    {
        _shootingPushConfig = shootingPushConfig;
        _entities = entities;
        _pushable = pushable;
        _shells = shells;
        _sampleData = sampleData;
        _camera = camera;
    }


    private readonly IReadOnlyList<ShootingPushConfig> _shootingPushConfig;
    private readonly IEntitiesAspects<EntityRoster> _entities;
    private readonly IPlayerPushableUnit _pushable;
    private readonly ShellsLifetime _shells;
    private readonly ISampleData _sampleData;
    private readonly ICameraMover _camera;
    private Tween _pushing;
    

    public void Initialize()
    {
        _pushable.IsCanMove = true;
        _shells.Shooted += OnShooted;
    }

    public void Dispose()
    {
        _shells.Shooted -= OnShooted;
        _pushing.TryKill();
    }


    private void OnShooted(ShootData data)
    {
        if (!_entities.TryGet(_pushable.EntityId, out EntityRoster entity) ||
            !TryGetConfig(data.Sample, entity, out ShootingPushConfig config))
        {
            return;
        }

        Vector3 afterPushPosition = _pushable.Root.position - data.GunDirection.WithY(0) * config.PushDistance;

        _pushing.TryKill();
        _pushing = _pushable.Root.DOMove(afterPushPosition, config.PushDuration)
                                    .OnUpdate(_camera.UpdateCameraPos)
                                    .OnComplete(() => _pushable.IsCanMove = true);
        _pushable.IsCanMove = false;
    }

    private bool TryGetConfig(ReadShell shellSample, EntityRoster entity, out ShootingPushConfig config)
    {
        config = null;

        if (!_sampleData.TryGetSample(entity, out Component entitySample))
            return false;

        for (int i = 0; i < _shootingPushConfig.Count; i++)
            if (_shootingPushConfig[i].Shell == shellSample &&
                _shootingPushConfig[i].Entities == entitySample)
            {
                config = _shootingPushConfig[i];
                return true;
            }

        return false;
    }
}
