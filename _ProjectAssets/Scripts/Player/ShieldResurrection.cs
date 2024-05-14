using Narratore.Helpers;
using Narratore.Solutions.Battle;
using System;
using System.Threading;
using UnityEngine;

public class ShieldResurrection : IDisposable
{
    public ShieldResurrection(IEntitiesAspects<DamageProtection> protection, float shieldDuration, IPlayerUnitRoot unit)
    {
        _protection = protection;
        _shieldDuration = shieldDuration;
        _unit = unit;
    }


    private readonly IEntitiesAspects<DamageProtection> _protection;
    private readonly float _shieldDuration;
    private readonly IPlayerUnitRoot _unit;
    private CancellationTokenSource _cts;


    public async void Create()
    {
        if (_protection.TryGet(_unit.EntityId, out DamageProtection protection))
        {
            _cts = new CancellationTokenSource();
            protection.Enable();

            bool isCanceled = await UniTaskHelper.Delay(_shieldDuration, _cts.Token);
            if (isCanceled) return;

            protection.Disable();
        }
        else
            Debug.LogError("Fail create resurrect shield for player unit. Not found unit protection");
    }


    public void Dispose()
    {
        _cts?.Cancel();
    }
}
