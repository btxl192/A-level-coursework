using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.i.1.a
public class Durable : Skill{

    //How much health should be increased per level
    public int HPincreaseamount
    {
        get
        {
            return 50;
        }
    }

    public Durable(int Lv) : base(Lv)
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
            return "Durable";
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
        return "Max HP is increased by " + lvl * HPincreaseamount;
    }

    protected override Skill MakeClone()
    {
        return new Durable(Level);
    }
}
