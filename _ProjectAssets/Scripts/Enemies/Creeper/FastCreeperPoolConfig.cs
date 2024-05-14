using Narratore.Pools;
using UnityEngine;

[System.Serializable]
public class FastCreeperPoolConfig : MBPoolConfig<FastCreeperRoster>
{
    public FastCreeperPoolConfig(FastCreeperRoster sample, Transform poolParent, Transform whenItemActiveParent, int startItemsCount = 0, bool isActivateDeactivateItem = true) : base(sample, poolParent, whenItemActiveParent, startItemsCount, isActivateDeactivateItem)
    {
    }

    public FastCreeperPoolConfig(FastCreeperRoster sample, Transform poolParent, Transform whenItemActiveParent, FastCreeperRoster[] startItems = null, bool isActivateDeactivateItem = true) : base(sample, poolParent, whenItemActiveParent, startItems, isActivateDeactivateItem)
    {
    }
}
