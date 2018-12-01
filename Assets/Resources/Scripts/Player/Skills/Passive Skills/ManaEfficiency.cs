using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.i.1.d
public class ManaEfficiency : Skill {

    public ManaEfficiency(int Lv) : base(Lv)
    {
    }

    public override SkillSets SkillSet
    {
        get
        {
            return SkillSets.General;
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
            return "Mana Efficiency";
        }
    }

    public override int MaxLevel
    {
        get
        {
            return 5;
        }
    }

    protected override string GetDescLevel(int lvl)
    {
        return string.Format("Skills use {0}% less mana", GetManaDiscount(lvl) * 100);
    }

    protected override Skill MakeClone()
    {
        return new ManaEfficiency(0);
    }

    //Percentage of mana skills should have their cost reduced by
    private float GetManaDiscount(int lvl)
    {
        return 0.1f * lvl;
    }

    public float ManaDiscount
    {
        get
        {
            return GetManaDiscount(Level);
        }
    }

    public override List<Skill> ReqSkills
    {
        get
        {
            return new List<Skill>
            {
                new ManaReservoir(1)
            };
        }
    }
}
