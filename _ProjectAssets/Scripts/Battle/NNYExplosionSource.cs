using Narratore.Solutions.Battle;

public class NNYExplosionSource : MultyExplosionSource
{
    public NNYExplosionSource(ShootingHitExplosionSource shooting)
    {
        TryAdd(shooting);
    }
}
