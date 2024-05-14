using Narratore.DI;
using Narratore.Solutions.Battle;


public class NNYCombineUnitsDeath : CombineDeathSource
{
    public NNYCombineUnitsDeath(ShootingDeathSource shooting, 
                                ExplosionDeathSource explosion, 
                                CreeperSelfExplosionDeathSource creepersSelfExplosion, 
                                DeadUnitsIds deadUnitsIds) : base(deadUnitsIds)
    {
        TryAdd(shooting);
        TryAdd(explosion);
        TryAdd(creepersSelfExplosion);
    }
}
