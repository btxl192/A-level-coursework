using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : ActiveSkill {

	public override SkillSets SkillSet {get {return SkillSets.Infrigidare; }}

    public override string SkillName { get { return "Blizzard"; } }

    public override int ManaCost
    {
        get
        {
            return 10;
        }
    }

    public override bool HoldKey
    {
        get
        {
            return false;
        }
    }

    public override SkillTypes SkillType
    {
        get
        {
            return SkillTypes.Active;
        }
    }

    public override int MaxLevel
    {
        get
        {
            return 10;
        }
    }

    public Blizzard(int Lv) : base(Lv)
    {
    }

    float dist(int lvl)
	{
		return lvl+4;
	}

	protected override string GetDescLevel(int lvl)
	{
		return "Slows down enemies that are " + dist(lvl).ToString() + " units away from you";
	}

	public override void UseSkill()
	{
		foreach(GameObject enemy in Enemies.GetEnemies())
		{
			if(Vector3.Distance(enemy.transform.position, Player.transform.position) <= dist(Level))
			{
				enemy.GetComponent<Statuses>().ApplyStatus(Statuses.MakeStatus(Statuses.statuses.Slow,Level), null);
			}
		}
	}

    protected override Skill MakeClone()
    {
        return new Blizzard(CurrentLevel);
    }
}
