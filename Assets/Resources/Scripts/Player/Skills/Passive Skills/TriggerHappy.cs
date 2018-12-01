using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.iv.1.b
public class TriggerHappy : Skill
{
    public TriggerHappy(int Lv) : base(Lv)
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
            return "Trigger Happy";
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
        return string.Format("Increase attack speed by {0}%", GetDecrease(lvl) * 100);
    }

    //The attack speed amount to decrease
    public float GetDecrease(int lvl)
    {
        return 0.04f * lvl;
    }

    public float DecreaseAmount
    {
        get
        {
            return GetDecrease(Level);
        }
    }

    protected override Skill MakeClone()
    {
        return new TriggerHappy(CurrentLevel);
    }
}
