using Cysharp.Threading.Tasks;
using Narratore.Helpers;
using Narratore.Interfaces;
using Narratore.Solutions.Battle;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using Narratore.Extensions;
using UnityEngine.Assertions.Must;

namespace Narratore.DI
{
    public class LineUnitsWavesSpawner : IDisposable, IUnitsWavesSpawner
    {
        public event Action<IWithId> Spawned;
        public event Action<IWithId> Dead;


        public LineUnitsWavesSpawner(IReadOnlyList<ISpawner> spawners, IReadOnlyList<SpawnWavesConfig> waves, IHeldPoints spawnPoints, int playerId, IPlayerUnitRotator unitRotator, IPlayerUnitRoot unit)
        {
            _waves = waves;
            _spawnPoints = spawnPoints;
            _playerId = playerId;

            _spawners = new();
            for (int i = 0; i < spawners.Count; i++)
            {
                ISpawner spawner = spawners[i];

                _spawners[spawner.Sample] = spawner;
                spawner.Dead += OnDead;
            }

            _cts = new CancellationTokenSource();
            _cs = new UniTaskCompletionSource();

            SpawnCount = 0;
            foreach (var wave in _waves)
                foreach (var pair in wave.Entities)
                {
                    if (_spawners.ContainsKey(pair.Item1))
                        SpawnCount += pair.Item2;
                    else
                        throw new Exception($"Not has spawner for entity {pair.Item1.name}. Type {pair.Item1.GetType().Name}");
                }

            _living = new HashSet<int>();
            Killed = 0;
            _unitRotator = unitRotator;
            _unit = unit;
        }



        /// <summary>
        /// Считаем эти данные сами, а не берем из спавнеров, 
        /// так как спавнеры могут использоваться в других местах тоже
        /// </summary>
        public int LeftSpawnCount => SpawnCount - SpawnedCount;
        public int SpawnCount { get; }
        public int SpawnedCount { get; private set; }
        public int LivingCount => _living.Count;
        public int Killed { get; private set; }



        private readonly Dictionary<EntityRoster, ISpawner> _spawners;
        private readonly IReadOnlyList<SpawnWavesConfig> _waves;
        private readonly IHeldPoints _spawnPoints;
        private readonly int _playerId;
        private readonly CancellationTokenSource _cts;
        private readonly HashSet<int> _living;
        private readonly IPlayerUnitRotator _unitRotator;
        private readonly IPlayerUnitRoot _unit;
        private UniTaskCompletionSource _cs;


        public UniTask Task => _cs.Task;


        public void Dispose()
        {
            _cts.Cancel();

            if (_cs != null)
                _cs.TrySetCanceled();

            foreach (var pair in _spawners)
                pair.Value.Dead -= OnDead;
        }

        public async void Spawn()
        {
            _cs = new UniTaskCompletionSource();

            bool isCanceled = false;
            SpawnWavesConfig wave = _waves[0];


            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            SpawnWave(wave.Entities[0], 0.4f);
            await UniTask.Delay(1600);

            _cs.TrySetResult();
        }

        private async void SpawnWave(EntityCount entityCount, float delay)
        {
            bool isCanceled = false;
            for (int i = 0; i < entityCount.Item2; i++)
            {
                ToSpawn(entityCount.Item1);

                if (i == entityCount.Item2 - 1)
                    break;

                isCanceled = await UniTaskHelper.Delay(delay, _cts.Token);
                if (isCanceled) return;
            }
        }

        private Vector3 ToSpawn(EntityRoster entity)
        {
            IHeldPoint point = _spawnPoints.Get();
            IWithId unit = _spawners[entity].Spawn(_playerId, point);

            _living.Add(unit.Id);
            SpawnedCount++;
            Spawned?.Invoke(unit);

            return point.Position;
        }

        private void OnDead(IWithId unit)
        {
            if (_living.Contains(unit.Id))
            {
                _living.Remove(unit.Id);
                Killed++;

                Dead?.Invoke(unit);
            }
        }
    }
}

