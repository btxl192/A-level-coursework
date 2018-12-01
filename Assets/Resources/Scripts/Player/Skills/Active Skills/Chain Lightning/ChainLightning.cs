using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.ii.1.c
public class ChainLightning : ActiveSkill
{
    public static int MaxEnemiesHit { get { return 4; } }
    private ChainLightningProjectile ChainLightningProj { get { return (Resources.Load(FileDir.ChainLightning) as GameObject).GetComponent<ChainLightningProjectile>(); } }

    public ChainLightning(int Lv) : base(Lv)
    {
    }

    public override SkillSets SkillSet
    {
        get
        {
            return SkillSets.Pyromaniac;
        }
    }

    public override string SkillName
    {
        get
        {
            return "Chain Lightning";
        }
    }

    public override int ManaCost
    {
        get
        {
            return 2;
        }
    }

    public override bool HoldKey
    {
        get
        {
            return true;
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

    protected override string GetDescLevel(int lvl)
    {
        return "Lightning arcs between up to " + MaxEnemiesHit + " enemies, dealing " + (10 * lvl) + "% of your damage (" + (int)(( 0.1 * lvl) * PStats.Combat.damage) + " damage). Skill can be held down.";
    }

    //Launch the chain lightning projectile to the enemy the player is locked on to, or the enemy closest to the mouse cursor
    public override void UseSkill()
    {
		if (Player.GetComponent<PlayerBehaviour> ().lockedon != null) 
		{
			ChainLightningProj.Shoot (Player.gameObject, Player.GetComponent<PlayerBehaviour> ().lockedon, (int)((0.1 * Level) * PStats.Combat.damage), true);
		} 
		else if (LockingOn.GetIndexOfClosestEnemyToMousePoint () != -1) 
		{
			ChainLightningProj.Shoot (Player.gameObject, Enemies.GetEnemies () [LockingOn.GetIndexOfClosestEnemyToMousePoint ()], (int)((0.1 * Level) * PStats.Combat.damage), true);
		} 
		else 
		{
			TempPopup.Show("No nearby enemies for chain lightning!", Color.red);
		}
    }

    protected override Skill MakeClone()
    {
        return new ChainLightning(CurrentLevel);
    }
}
