using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.i.1.e
public class MedicalExpert : Skill {

    public MedicalExpert(int Lv) : base(Lv)
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
            return "Medical Expert";
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
        return string.Format("Healing items become {0}% more effective", GetHealMult(lvl) * 100);
    }

    protected override Skill MakeClone()
    {
        return new MedicalExpert(CurrentLevel);
    }

    //How much healing items should have their effects boosted by
    private float GetHealMult(int lvl)
    {
        return 0.1f * lvl;
    }

    public float HealMult
    {
        get
        {
            return GetHealMult(Level);
        }
    }
}
