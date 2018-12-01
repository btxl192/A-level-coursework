using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.i.1.c
public class ManaVampire : Skill
{
    public ManaVampire(int Lv) : base(Lv)
    {
        GenericEnemy.DamagedByPlayer += Steal;
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
            return "Mana Vampire";
        }
    }

    public override int ReqLevel
    {
        get
        {
            return 5;
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

    //Adds mana for the player
    void Steal(int damage)
    {
        if (Level > 0)
        {
            PlayerSave.staticplayer.GetComponent<PlayerStats>().MP.ChangeMana((int)(AmountStolen(Level) * (float)damage));
        }        
    }

    //Amount of mana to steal each level
    private float AmountStolen (int lvl)
    {
        return lvl * 0.05f;    
    }

    protected override string GetDescLevel(int lvl)
    {
        return "Returns " + AmountStolen(lvl) * 100f + " % of the damage you deal to you as mana.";
    }

    protected override Skill MakeClone()
    {
        return new ManaVampire(CurrentLevel);
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
