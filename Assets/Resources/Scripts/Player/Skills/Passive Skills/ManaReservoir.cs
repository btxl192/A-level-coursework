using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.i.1.b
public class ManaReservoir : Skill {

    //Amount of mana to increase per level
    public int MPincreaseamount
    {
        get
        {
            return 50;
        }
    }

    public ManaReservoir(int Lv) : base(Lv)
    {
    }

    public override SkillSets SkillSet
    {
        get
        {
            return SkillSets.General;
        }
    }

    public override string SkillName
    {
        get
        {
            return "Mana Reservoir";
        }
    }

    public override SkillTypes SkillType
    {
        get
        {
            return SkillTypes.Passive;
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
        return "Max MP is increased by " + lvl * MPincreaseamount;
    }

    protected override Skill MakeClone()
    {
        return new ManaReservoir(Level);
    }
}
