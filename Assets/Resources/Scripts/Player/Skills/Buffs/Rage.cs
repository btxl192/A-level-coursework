using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.iv.2.a
public class Rage : ActiveSkill
{
    public Rage(int Lv) : base(Lv)
    {
    }

    public override SkillSets SkillSet
    {
        get
        {
            return SkillSets.Combat;
        }
    }

    public override string SkillName
    {
        get
        {
            return "Rage";
        }
    }

    public override bool HoldKey
    {
        get
        {
            return false;
        }
    }

    public override int ManaCost
    {
        get
        {
            return 10;
        }
    }

    public override SkillTypes SkillType
    {
        get
        {
            return SkillTypes.Buff;
        }
    }

    public override int MaxLevel
    {
        get
        {
            return 10;
        }
    }

    //Apply the Rage status on to the player
    public override void UseSkill()
    {
        Statuses.StatusType temp = Statuses.MakeStatus(Statuses.statuses.Rage, Level);
        Player.GetComponent<Statuses>().ApplyStatus(temp, null);
    }

    protected override string GetDescLevel(int lvl)
    {
        return "Increases attack by " + 5 * lvl + "%" + " for 30 seconds";
    }

    protected override Skill MakeClone()
    {
        return new Rage(Level);
    }

    public override List<Skill> ReqSkills
    {
        get
        {
            return new List<Skill>()
            {
                new Deadly(5)
            };
        }
    }

}
