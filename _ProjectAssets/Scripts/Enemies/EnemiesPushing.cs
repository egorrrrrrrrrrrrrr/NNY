using Narratore.Solutions.Battle;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Narratore.Extensions;
using Narratore.Pools;
using VContainer.Unity;


public class EnemiesPushing : IDisposable, IInitializable
{
    public EnemiesPushing(  IReadOnlyList<ShootingPushConfig> shootingPushConfigs,
                            IEntitiesAspects<Transform> transforms,
                            IEntitiesAspects<MovableBot> movable,
                            ShellsLifetime shells,
                            ISampleData sampleData,
                            IEntitiesAspects<EntityRoster> entities)
    {
        _transforms = transforms;
        _movable = movable;
        _shells = shells;
        _sampleData = sampleData;
        _entities = entities;

        _shootingPushConfig = new Dictionary<Component, Dictionary<Component, ShootingPushConfig>>();

        foreach (var config in shootingPushConfigs)
        {
            if (!_shootingPushConfig.ContainsKey(config.Shell))
                _shootingPushConfig[config.Shell] = new Dictionary<Component, ShootingPushConfig>();

            foreach (var enemy in config.Entities)
                _shootingPushConfig[config.Shell][enemy] = config;
        }
    }


    private readonly Dictionary<Component, Dictionary<Component, ShootingPushConfig>> _shootingPushConfig;
    private readonly IEntitiesAspects<Transform> _transforms;
    private readonly IEntitiesAspects<EntityRoster> _entities;
    private readonly IEntitiesAspects<MovableBot> _movable;
    private readonly ShellsLifetime _shells;
    private readonly ISampleData _sampleData;
    private readonly Dictionary<int, Tween> _pushings = new();


    public void Initialize()
    {
        _shells.HitToUnit += OnHit;
    }

    public void Dispose()
    {
        _shells.HitToUnit -= OnHit;

        foreach (var pair in _pushings)
            pair.Value.TryKill();

        _pushings.Clear();
    }


    private void OnHit(ReadShell shell, Vector3 hitPoint, MovableBounds _, int victimId)
    {
        if (!_movable.TryGet(victimId, out MovableBot movable) ||
            !_transforms.TryGet(victimId, out Transform transf) ||
            !_entities.TryGet(victimId, out EntityRoster entity) ||
            !TryGetConfig(shell, entity, out ShootingPushConfig config) ||
            movable.IsStun)
        {
            return;
        }

        Vector3 afterPushPosition = transf.position + shell.Forward.WithY(0) * config.PushDistance;

        if (_pushings.ContainsKey(victimId))
            _pushings[victimId].TryKill();

        _pushings[victimId] = transf.DOMove(afterPushPosition, config.PushDuration).OnComplete(() => movable.IsStun = false);
        movable.IsStun = true;
    }

    private bool TryGetConfig(ReadShell shell, EntityRoster entity, out ShootingPushConfig config)
    {
        config = null;
        
        if (!_sampleData.TryGetSample(shell, out Component shellSample) ||
            !_sampleData.TryGetSample(entity, out Component entitySample)) 
            return false;

        if (_shootingPushConfig.ContainsKey(shellSample) && _shootingPushConfig[shellSample].ContainsKey(entitySample))
        {
            config = _shootingPushConfig[shellSample][entitySample];
            return true;
        }

        return false;
    }
}
