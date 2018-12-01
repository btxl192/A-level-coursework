using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.iv.1.a
public class Deadly : Skill
{
    public Deadly(int Lv) : base(Lv)
    {
    }

    public override SkillSets SkillSet
    {
        get
        {
            return SkillSets.Combat;
        }
    }

    public override SkillTypes SkillType
    {
        get
        {
            return SkillTypes.Passive;
        }
    }

    public override string SkillName
    {
        get
        {
            return "Deadly";
        }
    }

    public override int MaxLevel
    {
        get
        {
            return 10;
        }
    }

    protected override string GetDescLevel(int lvl)
    {
        return string.Format("Final damage is increased by {0}%", getdamageincreaseamount(lvl)*100);
    }

    //Returns how much damage should be increased by
    public float getdamageincreaseamount(int lvl)
    {
        return 0.1f * lvl;
    }

    public float DamageIncreaseAmount
    {
        get
        {
            return getdamageincreaseamount(Level);
        }
    }

    protected override Skill MakeClone()
    {
        return new Deadly(CurrentLevel);
    }
}
