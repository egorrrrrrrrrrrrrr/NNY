using Narratore.Pools;
using UnityEngine;


[System.Serializable]
public class CreeperPoolConfig : MBPoolConfig<CreeperRoster>
{
    public CreeperPoolConfig(CreeperRoster sample, Transform poolParent, Transform whenItemActiveParent, int startItemsCount = 0, bool isActivateDeactivateItem = true) : base(sample, poolParent, whenItemActiveParent, startItemsCount, isActivateDeactivateItem)
    {
    }

    public CreeperPoolConfig(CreeperRoster sample, Transform poolParent, Transform whenItemActiveParent, BaseCreeperRoster[] startItems = null, bool isActivateDeactivateItem = true) : base(sample, poolParent, whenItemActiveParent, startItems, isActivateDeactivateItem)
    {
    }
}
