using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.iv.1.c
public class GetHyped : Skill {

    private int CurrentStacks;

    public GetHyped(int Lv) : base(Lv)
    {
        GenericEnemy.DamagedByPlayer += ApplyStatus;
        GetHypedStatus.StatusEnded += ResetStacks;
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
            return "GET HYPED";
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
        return string.Format("Attacks speed increases by {0}% every time you damage an enemy for 3 seconds. Stacks up to {1} times.", GetIncreaseAmount(lvl)*100, GetMaxStacks(lvl));
    }

    protected override Skill MakeClone()
    {
        return new GetHyped(CurrentLevel);
    }

    //The attack speed increase amount
    public float GetIncreaseAmount(int lvl)
    {
        return 0.01f * lvl;
    }

    public float IncreaseAmount
    {
        get
        {
            return GetIncreaseAmount(Level);
        }
    }

    //the maximum number of times the effect stacks
    public int GetMaxStacks(int lvl)
    {
        return 2 * lvl;
    }

    public int MaxStacks
    {
        get
        {
            return GetMaxStacks(Level);
        }
    }

    public override List<Skill> ReqSkills
    {
        get
        {
            return new List<Skill>()
            {
                new TriggerHappy(5)
            };
        }
    }

    //Apply the GetHyped status affect
    private void ApplyStatus(int i)
    {
        if (CurrentStacks < MaxStacks)
        {
            CurrentStacks += 1;
            PlayerSave.staticplayer.GetComponent<Statuses>().ApplyStatus(Statuses.MakeStatus(Statuses.statuses.GetHyped, CurrentStacks));
        }
    }

    private void ResetStacks()
    {
        CurrentStacks = 0;
    }
}
