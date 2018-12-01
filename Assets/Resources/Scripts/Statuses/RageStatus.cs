using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageStatus : StatusBase
{
    public override string Desc
    {
        get
        {
            return string.Format("Damage increased by {0}%", Level * 0.05f * 100);
        }
    }

    protected override float lifetime
    {
        get
        {
            return 30f;
        }
    }

    protected override void InitialiseStatusIcon()
    {
        StatusIconReference = Resources.Load(FileDir.RageStatusIcon) as GameObject;
    }

    //When the status is applied, add a multiplicative damage bonus to the player
    protected override void OnStatusApplied()
    {
        PlayerSave.staticplayer.GetComponent<PlayerStats>().Combat.AddMultDamage("Rage", Level * 0.05f + 1);
    }

    //When the status ends, remove the multiplicative damage bonus to the player
    protected override void OnStatusEnd()
    {
        PlayerSave.staticplayer.GetComponent<PlayerStats>().Combat.RemoveMultDmgBuff("Rage");
    }
}
