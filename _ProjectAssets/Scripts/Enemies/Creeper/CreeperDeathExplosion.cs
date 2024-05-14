using Narratore.Solutions.Battle;
using UnityEngine;

namespace Narratore.DI
{
    public class CreeperDeathExplosion : ExplosionUnitDeath
    {
        public void Death()
        {
            IExplosionKillable shootingKillable = this;
            shootingKillable.Death(Vector3.zero, 0);
        }
    }
}

