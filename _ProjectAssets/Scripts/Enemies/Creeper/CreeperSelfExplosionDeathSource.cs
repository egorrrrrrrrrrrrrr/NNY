using Narratore.Solutions.Battle;
using System.Linq;
using UnityEngine;

namespace Narratore.DI
{
    public class CreeperSelfExplosionDeathSource : DeathSource, IBeginnedUpdatable
    {
        public CreeperSelfExplosionDeathSource(DeadUnitsIds deadUnitsIds,
                                                IPlayerUnitRoot playerUnit,
                                                IEntitiesAspects<CreeperDeathExplosion> creepers,
                                                IEntitiesAspects<Transform> creepersTransform,
                                                float explosionDistance,
                                                IEntitiesAspects<DamageProtection> protection) : base(deadUnitsIds)
        {
            _playerUnit = playerUnit;
            _creepers = creepers;
            _creepersTransform = creepersTransform;
            _sqrExplosionDistance = explosionDistance * explosionDistance;
            _protection = protection;
        }


        private readonly IPlayerUnitRoot _playerUnit;
        private readonly IEntitiesAspects<CreeperDeathExplosion> _creepers;
        private readonly IEntitiesAspects<Transform> _creepersTransform;
        private readonly IEntitiesAspects<DamageProtection> _protection;
        private readonly float _sqrExplosionDistance;


        public override void Dispose() { }

        public void Tick()
        {
            if (_protection.TryGet(_playerUnit.EntityId, out DamageProtection protection) && protection.enabled)
                return;

            Vector3 playerPosition = _playerUnit.Root.position;
            int[] ids = _creepers.All.Keys.ToArray();

            for (int i = 0; i < ids.Length; i++)
            {
                int creeperId = ids[i];
                if (IsNearPoint(creeperId, playerPosition) && _creepers.TryGet(creeperId, out CreeperDeathExplosion creeper))
                {
                    ToDeath(creeperId);
                    creeper.Death();
                }
            }
        }


        private bool IsNearPoint(int creeperId, Vector3 point)
        {
            return _creepersTransform.TryGet(creeperId, out Transform transf) &&
                    (transf.position - point).sqrMagnitude < _sqrExplosionDistance;
        }

    }
}

