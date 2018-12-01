using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tome : UsableItem {

    [SerializeField]
    private int SPtoGive;

    [SerializeField]
    private Skill.SkillSets _SkillSet;
    public Skill.SkillSets SkillSet
    {
        get
        {
            return _SkillSet;
        }
    }

    public override string FormattedDesc
    {
        get
        {
            if (SkillSet != Skill.SkillSets.General)
            {
                return base.FormattedDesc + "\n\nGrants access to " + SkillSet.ToString() + " skills.\nGives you " + SPtoGive + " skill points";
            }
            else
            {
                return base.FormattedDesc + "\nGives you " + SPtoGive + " skill points";
            }
        }
    }

    public override string GetSaveData()
    {
        return base.GetSaveData() + "," + SkillSet.ToString();
    }

    public void SetSkillSet(Skill.SkillSets s)
    {
        _SkillSet = s;
    }

    //Give the player skill points and add a skillset 
    public override void UseItem()
    {
        PlayerSave.staticplayer.GetComponent<PlayerStats>().ChangeSkillPoints(SPtoGive);
        Amount -= 1;
        if (SkillSet != Skill.SkillSets.General)
        {
            if (!SkillManager.PlayerSkillSets.Contains(SkillSet))
            {
                SkillManager.AddPlayerSkillSet(SkillSet);                
            }
            else
            {
                TempPopup.Show("You already have access to this skill set!", Color.red);
            }
        }
    }
}
