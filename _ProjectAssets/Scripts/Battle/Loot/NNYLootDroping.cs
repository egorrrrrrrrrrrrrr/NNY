using Narratore.Solutions.Battle;
using System.Collections.Generic;
using UnityEngine;

public class NNYLootDroping : LootDroping
{
    public NNYLootDroping(  LootDeathSources death, 
                            IEntitiesAspects<IDropLootData> dropData, 
                            IReadOnlyList<ILootSpawner> spawners, 
                            int lootOwnerPlayer, 
                            IEntitiesAspects<Transform> transforms) : 
        base(death, dropData, spawners, lootOwnerPlayer, transforms)
    {
    }
}
