using Narratore.Solutions.Battle;

public class LootDeathSources : CombineDeathSource
{
    public LootDeathSources(ShootingDeathSource shootingDeathSource, 
                            ExplosionDeathSource explosionDeathSource, 
                            DeadUnitsIds deadUnitsIds) : base(deadUnitsIds)
    {
        TryAdd(shootingDeathSource);
        TryAdd(explosionDeathSource);
    }
}
