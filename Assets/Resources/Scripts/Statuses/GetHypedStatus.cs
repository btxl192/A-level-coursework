using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHypedStatus : StatusBase
{

    public delegate void BlankEvent();
    public static event BlankEvent StatusEnded;

    protected override float lifetime
    {
        get
        {
            return 3;
        }
    }

    public override string Desc
    {
        get
        {
            return string.Format("Attack speed delay decreased by {0}%", ((GetHyped)SkillManager.Skills[13]).IncreaseAmount * Level * 100);
        }
    }

    protected override void InitialiseStatusIcon()
    {
        StatusIconReference = Resources.Load(FileDir.GetHypedStatusIcon) as GameObject;
    }

    protected override void OnStatusEnd()
    {
        if (StatusEnded != null)
        {
            StatusEnded();
        }
        PlayerSave.staticplayer.GetComponent<PlayerStats>().Combat.RemoveMultAttSpdBuff("Get Hyped");
    }

    public void ResetTimer()
    {
        timelived = 0;
    }

    //Remove the current GetHyped and add on the higher level GetHyped
    protected override void OnLevelChanged()
    {
        PlayerStats PStats = PlayerSave.staticplayer.GetComponent<PlayerStats>();
        PStats.Combat.RemoveMultAttSpdBuff("Get Hyped");
        PStats.Combat.AddMultAttSpeed("Get Hyped", 1 - ((GetHyped)SkillManager.Skills[13]).IncreaseAmount * Level);
    }
}
