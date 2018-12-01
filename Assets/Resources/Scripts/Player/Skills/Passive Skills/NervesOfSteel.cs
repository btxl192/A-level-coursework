using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.iv.1.d
public class NervesOfSteel : Skill
{
    public NervesOfSteel(int Lv) : base(Lv)
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
            return "Nerves of Steel";
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
        return string.Format("Defence is increased by {0}", GetIncreaseAmount(lvl));
    }

    //Amount of armour to increase by each level
    public int GetIncreaseAmount(int lvl)
    {
        return 100 * lvl;
    }

    public int IncreaseAmount
    {
        get
        {
            return GetIncreaseAmount(Level);
        }
    }

    protected override Skill MakeClone()
    {
        return new NervesOfSteel(CurrentLevel);
    }
}
