using Narratore.Data;
using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using Narratore.Solutions.Timer;
using UnityEngine;


public class PlayerGunRoster : UpgradableShopItem
{
    public Transform Root => _root;
    public Gun Gun => _gun;
    public LocalPositionRecoil Recoil => _recoil;
    public int Damage => _damage.Get();
    public FloatStat MoveSpeed => _moveSpeed;
    public IReadOnlyTimer RechargeTimer => _rechargeTimer;
    public float MaxDistance => _maxDistance.Get();
    public int MinExplosionDamage => _minExplosionDamage.Get();
    public float ExplosionRadius => _exposionRadius.Get();
    public bool IsWithExplosion => _minExplosionDamage != null && _exposionRadius != null;
    public ShootArea ShootArea => _shootArea;


    [Header("PLAYER GUN*")]
    [SerializeField] private Transform _root;
    [SerializeField] private Gun _gun;
    [SerializeField] private LocalPositionRecoil _recoil;
    [SerializeField] private IntStat _damage;
    [SerializeField] private FloatStat _moveSpeed;
    [SerializeField] private GunRechargeTimer _rechargeTimer;
    [SerializeField] private ReadValue<float> _maxDistance;
    [SerializeField] private ShootArea _shootArea;

    [Header("EXPLOSION")]
    [SerializeField] private ReadValue<int> _minExplosionDamage;
    [SerializeField] private ReadValue<float> _exposionRadius;
}
