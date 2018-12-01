using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.iii.1.a
public class Toxic : Skill {

    //Chance to add the poison effect to the player's weapon
    public float PoisonChance
    {
        get
        {
            return 0.5f;
        }
    }

	public override SkillSets SkillSet {get {return SkillSets.Assassin; }}

    public override string SkillName { get { return "Toxic"; } }

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

    public Toxic(int Lv) : base(Lv)
	{
	}

	protected override string GetDescLevel(int lvl)
	{
		if(lvl==1)
		{
			return string.Format("Improves poison damage by {0} level. Your attacks have a {1}% chance of poisoning the enemy.", lvl, PoisonChance * 100);
		}
		return string.Format("Improves poison damage by {0} levels. Your attacks have a {1}% chance of poisoning the enemy.", lvl, PoisonChance * 100);
    }

    protected override Skill MakeClone()
    {
        return new Toxic(CurrentLevel);
    }
}
