using Narratore.Data;
using Narratore.Solutions.Battle;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BotShootingConfig
{
    public Gun Gun => _gun;
    public IReadOnlyList<ReadValue<int>> Damage => _damage;
    public float MaxDistance => _maxDistance.Get();


    [SerializeField] private Gun _gun;
    [SerializeField] private ReadValue<int>[] _damage;
    [SerializeField] private ReadValue<float> _maxDistance;
}
