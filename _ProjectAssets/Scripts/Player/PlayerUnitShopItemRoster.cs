using Narratore.MetaGame;
using UnityEngine;


public class PlayerUnitShopItemRoster : UpgradableShopItem
{
    public PlayerUnitBattleRoster BattleRoster => _battleRoster;


    [SerializeField] private PlayerUnitBattleRoster _battleRoster;
}