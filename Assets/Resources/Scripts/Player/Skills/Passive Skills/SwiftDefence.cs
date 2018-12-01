using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.iii.1.c
public class SwiftDefence : Skill
{ 
    public SwiftDefence(int Lv) : base(Lv)
    {
        PlayerMovement.PlayerMoving += PlayerMoving;
        PlayerMovement.PlayerNotMoving += PlayerNotMoving;
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
            return "Swift Defence";
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
        return string.Format("Take {0}% less damage while moving.", GetDamageReduction(lvl) * 100);
    }

    //The amount of damage to take away
    public float GetDamageReduction(int lvl)
    {
        return 0.03f * lvl;
    }

    public float DamageReduction
    {
        get
        {
            return GetDamageReduction(Level);
        }
    }

    protected override Skill MakeClone()
    {
        return new SwiftDefence(CurrentLevel);
    }

    //Runs when the player is moving. Add the SwiftDefence bonus
    private void PlayerMoving()
    {
        if (Level > 0)
        {
            PlayerStats PStats = PlayerSave.staticplayer.GetComponent<PlayerStats>();
            if (!PStats.ArmourVals.ContainsMultDefenceBonus("Swift Defence"))
            {
                PStats.ArmourVals.AddMultDefence("Swift Defence", DamageReduction);
            }
        }   
    }

    //Runs when the player is not moving. Remove the SwiftDefence bonus
    private void PlayerNotMoving()
    {
        if (Level > 0)
        {
            PlayerStats PStats = PlayerSave.staticplayer.GetComponent<PlayerStats>();
            if (PStats.ArmourVals.ContainsMultDefenceBonus("Swift Defence"))
            {
                PStats.ArmourVals.RemoveMultDefence("Swift Defence");
            }
        }

    }
}
