using UnityEngine;

public class KillBox : EffectZone
{
    public override void Effect(PlayerGeneral plr)
    {
        plr.KillPlayer();
    }
}
