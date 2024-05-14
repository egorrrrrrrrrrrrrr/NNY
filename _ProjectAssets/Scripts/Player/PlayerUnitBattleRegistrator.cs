using Narratore.Solutions.Battle;
using UnityEngine;

public class PlayerUnitBattleRegistrator : EntityBattleRegistrator<PlayerUnitBattleRoster>
{
    public PlayerUnitBattleRegistrator(PlayerEntitiesIds playerUnitsIds,
                                        EntitiesAspects<EntityRoster> entities,
                                        EntitiesAspects<Hp> hps,
                                        EntitiesAspects<ReadHp> readHps,
                                        EntitiesAspects<Transform> transforms,
                                        EntitiesListsBounds bounds,
                                        LootCollectors lootCollectors,
                                        EntitiesAspects<DamageProtection> protection,
                                        EntitiesAspects<IDamageProtection> readProtection) : base(playerUnitsIds, entities, transforms)
    {
        _hps = hps;
        _readHps = readHps;
        _transforms = transforms;
        _bounds = bounds;
        _lootCollectors = lootCollectors;
        _protection = protection;
        _readProtection = readProtection;
    }


    private readonly EntitiesAspects<Hp> _hps;
    private readonly EntitiesAspects<ReadHp> _readHps;
    private readonly EntitiesAspects<Transform> _transforms;
    private readonly EntitiesAspects<DamageProtection> _protection;
    private readonly EntitiesAspects<IDamageProtection> _readProtection;
    private readonly EntitiesListsBounds _bounds;
    private readonly LootCollectors _lootCollectors;

    protected override void RegisterImpl(PlayerUnitBattleRoster unit)
    {
        _hps.Set(unit.Id, unit.Hp);
        _readHps.Set(unit.Id, unit.Hp);
        _transforms.Set(unit.Id, unit.Root);
        _bounds.Set(unit.Id, unit.Bounds);
        _protection.Set(unit.Id, unit.ResurrectShield);
        _readProtection.Set(unit.Id, unit.ResurrectShield);

        _lootCollectors[unit.LootCollider] = unit.Id;
    }

    protected override void UnregisterImpl(PlayerUnitBattleRoster unit)
    {
        _hps.Remove(unit.Id);
        _readHps.Remove(unit.Id);
        _transforms.Remove(unit.Id);
        _bounds.Remove(unit.Id);
        _protection.Remove(unit.Id);
        _readProtection.Remove(unit.Id);

        _lootCollectors.Remove(unit.LootCollider);
    }
}
