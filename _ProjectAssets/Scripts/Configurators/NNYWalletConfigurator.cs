using Narratore;
using Narratore.DI;
using Narratore.MetaGame;
using Narratore.Pools;
using UnityEngine;
using VContainer;


public class NNYWalletConfigurator : LevelConfigurator
{
    [SerializeField] private WalletProvider _wallet;
    [SerializeField] private CurrencyDescriptor _currency;

    public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
    {
        builder.RegisterInstance(_wallet);
        builder.RegisterInstance(_currency);
    }
}
