using Narratore;
using Narratore.Data;
using Narratore.Solutions.Battle;
using System;
using System.Collections.Generic;

public class BotsShooting : EntitiesAspectsObserver<BotShootingConfig>, IBeginnedUpdatable
{
    public BotsShooting(IEntitiesAspects<BotShootingConfig> target, ShellsLifetime shells, int botPlayerId) : base(target)
    {
        _configs = target;
        _shells = shells;
        _botPlayerId = botPlayerId;

        _callbacks = new();
    }


    private readonly IEntitiesAspects<BotShootingConfig> _configs;
    private readonly ShellsLifetime _shells;
    private readonly int _botPlayerId;
    private readonly Dictionary<Gun, Action> _callbacks;


    public void Tick()
    {
        foreach (var pair in _configs.All)
        {
            Gun gun = pair.Value.Gun;
            if (gun.IsCanTodoAction)
                gun.ShootMagazine();
        }
    }


    protected override void OnAdded(int entityId, BotShootingConfig aspect)
    {
        void OnShooted() => this.OnShooted(aspect, entityId);
        int attackIndex = UnityEngine.Random.Range(0, aspect.Gun.MagazinesCount);

        aspect.Gun.TryChangeMagazine(attackIndex);
        aspect.Gun.Recharge();

        aspect.Gun.Shooted += OnShooted;
        _callbacks[aspect.Gun] = OnShooted;

    }

    protected override void OnRemoving(int entityId)
    {
        _configs.TryGet(entityId, out BotShootingConfig config);

        config.Gun.Shooted -= _callbacks[config.Gun];
        _callbacks.Remove(config.Gun);
    }

    protected override void OnDispose()
    {
        foreach (var pair in _callbacks)
            pair.Key.Shooted -= pair.Value;

        _callbacks.Clear();
    }

    private void OnShooted(BotShootingConfig config, int entityId)
    {
        ReadValue<int> damage = config.Damage[config.Gun.MagazineIndex];
        IImpact impact = new ShellDamage(_botPlayerId, damage.Get(), ImpactTargets.Enemies);

        _shells.Shoot(config.Gun, _botPlayerId, entityId, impact, config.MaxDistance);
    }
}
