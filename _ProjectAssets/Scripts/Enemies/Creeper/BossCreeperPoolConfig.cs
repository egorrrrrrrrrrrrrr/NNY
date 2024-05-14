using Narratore.Pools;
using UnityEngine;

[System.Serializable]
public class BossCreeperPoolConfig : MBPoolConfig<BossCreeperRoster>
{
    public BossCreeperPoolConfig(BossCreeperRoster sample, Transform poolParent, Transform whenItemActiveParent, int startItemsCount = 0, bool isActivateDeactivateItem = true) : base(sample, poolParent, whenItemActiveParent, startItemsCount, isActivateDeactivateItem)
    {
    }

    public BossCreeperPoolConfig(BossCreeperRoster sample, Transform poolParent, Transform whenItemActiveParent, BossCreeperRoster[] startItems = null, bool isActivateDeactivateItem = true) : base(sample, poolParent, whenItemActiveParent, startItems, isActivateDeactivateItem)
    {
    }
}
