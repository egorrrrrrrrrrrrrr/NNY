using UnityEngine;


public class BossCreeperRoster : CreeperRoster
{
    public BotShootingConfig Shooting => _shooting;


    [SerializeField] private BotShootingConfig _shooting; 
}
