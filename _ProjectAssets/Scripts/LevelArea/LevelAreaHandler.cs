using Narratore.Extensions;
using Narratore.Helpers;
using Narratore.Solutions.Battle;
using Narratore.UI;
using Narratore.WorkWithMesh;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Narratore.DI
{


    [System.Serializable]
    public class LevelAreaConfig
    {
        public LevelAreaConfig(EntityCount outEnemies, float outEnemiesSpawnDelay, int outDamagePerSecond, LevelOutDamageSourceKey damageSourceKey)
        {
            _outEnemies = outEnemies;
            _outEnemiesSpawnDelay = outEnemiesSpawnDelay;
            _outDamagePerSecond = outDamagePerSecond;
            _damageSourceKey = damageSourceKey;
        }

        private LevelAreaConfig() { }


        public EntityCount OutEnemies => _outEnemies;
        public float OutEnemiesSpawnDelay => _outEnemiesSpawnDelay;
        public int OutDamagePerSecond => _outDamagePerSecond;
        public LevelOutDamageSourceKey DamageSourceKey => _damageSourceKey;


        [SerializeField] private EntityCount _outEnemies;
        [SerializeField] private float _outEnemiesSpawnDelay;
        [SerializeField] private int _outDamagePerSecond;
        [SerializeField] private LevelOutDamageSourceKey _damageSourceKey;
        
    }

    public class LevelAreaHandler : IBeginnedUpdatable
    {
        public LevelAreaHandler(MeshFrame area,
                                LevelAreaConfig config,
                                IPlayerUnitRootAndHp playerUnit,
                                LoopedTextFadeAnimation warning,
                                IReadOnlyList<ISpawner> spawners,
                                RandomOutCameraHeldPoints spawnPoints,
                                IEntitiesAspects<DamageProtection> protection)
        {
            _config = config;
            _area = area;
            _playerUnit = playerUnit;
            _warning = warning;
            _spawners = spawners;
            _spawnPoints = spawnPoints;

            Vector3 center = _area.transform.position;

            _levelAreaBounds = new Bounds(center, _area.Size.To3D(Enums.TwoAxis.XZ, 10f));
            _secondCounter = new LoopedFloatCounter(0, 1, 0);
            _protection = protection;
        }


        private readonly MeshFrame _area;
       
        private readonly IPlayerUnitRootAndHp _playerUnit;
        private readonly Bounds _levelAreaBounds;
        private readonly LoopedTextFadeAnimation _warning;
        private readonly LevelAreaConfig _config;
        private readonly IReadOnlyList<ISpawner> _spawners;
        private readonly IHeldPoints _spawnPoints;
        private readonly IEntitiesAspects<DamageProtection> _protection;

        private CancellationTokenSource _spawning;
        private bool _isSpawned;
        private LoopedFloatCounter _secondCounter;
        

        public void Tick()
        {
            if (_levelAreaBounds.Contains(_playerUnit.Root.position))
            {
                if (_warning.enabled)
                    _warning.Disable();

                if (_spawning != null)
                {
                    _spawning.Cancel();
                    _spawning = null;
                }
            }
            else
            {
                if (!_warning.enabled)
                    _warning.Enable();

                if (_spawning == null && !_isSpawned)
                    TrySpawn();

                float secondsCache = _secondCounter.Current;
                float seconds = _secondCounter.ApplyDelta(Time.deltaTime);

                if (seconds < secondsCache)
                {
                    int damage = _config.OutDamagePerSecond;

                    if (_protection.TryGet(_playerUnit.EntityId, out DamageProtection protection))
                        damage = protection.Reduce(_config.DamageSourceKey, damage);

                    _playerUnit.Hp.ApplyDelta(-damage);
                }
            }
        }


        private async void TrySpawn()
        {
            _spawning = new CancellationTokenSource();

            bool isCanceled = await UniTaskHelper.Delay(_config.OutEnemiesSpawnDelay, _spawning.Token);
            if (isCanceled) return;

            if (TryGetSpawner(out ISpawner spawner))
            {
                for (int i = 0; i < _config.OutEnemies.Item2; i++)
                    spawner.Spawn(PlayersIds.GetBotId(2), _spawnPoints.Get());
            }
            else
                Debug.LogWarning($"In level area not found spawner for unit {_config.OutEnemies.Item1.name}");

            _isSpawned = true;
        }

        private bool TryGetSpawner(out ISpawner spawner)
        {
            spawner = null;
            foreach (var check in _spawners)
                if (check.Sample == _config.OutEnemies.Item1)
                {
                    spawner = check;
                    return true;
                }

            return false;
        }
    }
}

