using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.iii.1.b
public class SneakAttack : Skill
{
    public SneakAttack(int Lv) : base(Lv)
    {
    }

    public override SkillSets SkillSet
    {
        get
        {
            return SkillSets.Assassin;
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
            return "Sneak Attack";
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
        return string.Format("Deal {0}% more damage to enemies unaware of you", GetDamageIncrease(lvl) * 100);
    }

    //The damage increase when attacking an unaware enemy
    public float GetDamageIncrease(int lvl)
    {
        return 0.5f * lvl;
    }

    public float DamageIncrease
    {
        get
        {
            return GetDamageIncrease(Level);
        }
    }

    protected override Skill MakeClone()
    {
        return new SneakAttack(CurrentLevel);
    }
}
