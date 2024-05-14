using Narratore.Solutions.Battle;

namespace Narratore.DI
{
    public class NNYDamageSource : MultyDamageSource
    {
        public NNYDamageSource(ShootingDamageSource shooting, ExplosionDamageSource explosion) : base()
        {
            TryAdd(shooting);
            TryAdd(explosion);
        }
    }
}

