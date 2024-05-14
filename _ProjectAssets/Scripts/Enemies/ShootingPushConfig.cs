using Narratore.Solutions.Battle;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootingPushConfig
{
    public ShootingPushConfig(ReadShell shell, EntityRoster[] entities, float stunDuration, float pushDistance)
    {
        _shell = shell;
        _entities = entities;
        _pushDuration = stunDuration;
        _pushDistance = pushDistance;
    }

    private ShootingPushConfig() { }


    public Component Shell => _shell;
    public IReadOnlyList<Component> Entities => _entities;
    public float PushDuration => _pushDuration;
    public float PushDistance => _pushDistance;


    [SerializeField] [HideInInspector] private string _key;
    [SerializeField] private ReadShell _shell;
    [SerializeField] private EntityRoster[] _entities;
    [SerializeField] private float _pushDuration;
    [SerializeField] private float _pushDistance;

    public void OnValidate()
    {
        _key = $"{_shell.name}";
    }
}
