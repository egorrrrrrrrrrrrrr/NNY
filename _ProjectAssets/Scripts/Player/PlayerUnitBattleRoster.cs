using Narratore.Components;
using Narratore.Solutions.Battle;
using Narratore.WorkWithMesh;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerUnitBattleRoster : EntityRoster
{
    public override event UnityAction OutBattle
    {
        add { _death.Ended += value; }
        remove { _death.Ended -= value; }
    }


    public Transform GunRecoilTarget => _gunRecoilTarget;
    public Transform MainGunAttach => _mainGunAttach;
    public Transform SecondGunAttach => _secondGunAttach;
    public IReadOnlyList<MovableBounds> Bounds => _bounds;
    public StatValue<float> MoveSpeed => _moveSpeed;
    public TwoLegsLoopedRotators FootsAnimator => _footsAnimator;
    public SecondHandState SecondHandState => _secondHandState;
    public Hp Hp => _hp;
    public StubUnitDeath Death => _death;
    public Collider LootCollider => _lootCollider;
    public DamageProtection ResurrectShield => _resurrectShield;


    [SerializeField] private Transform _gunRecoilTarget;
    [SerializeField] private Transform _mainGunAttach;
    [SerializeField] private Transform _secondGunAttach;
    [SerializeField] private MovableBounds[] _bounds;
    [SerializeField] private StatValue<float> _moveSpeed;
    [SerializeField] private TwoLegsLoopedRotators _footsAnimator;
    [SerializeField] private SecondHandState _secondHandState;
    [SerializeField] private Hp _hp;
    [SerializeField] private StubUnitDeath _death;
    [SerializeField] private Collider _lootCollider;
    [SerializeField] private DamageProtection _resurrectShield;
}
