using UnityEngine;

public class Checkpoint : EffectZone
{
    public override void Effect(PlayerGeneral plr)
    {
        plr.HitCheckpoint(gameObject);
    }
}
