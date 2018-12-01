using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.ii.1.a
public class Explosion : ActiveSkill {

	private GameObject ItemReference;
	private static GameObject fireball;
    
	public override SkillSets SkillSet {get {return SkillSets.Pyromaniac; }}

    public override float Cooldown
    {
        get
        {
            return 2f;
        }
    }

    public override string SkillName { get { return "Explosion"; } }

    public override int ManaCost
    {
        get
        {
            return 5;
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

    public Explosion(int Lv) : base(Lv)
    {
    }

    //Initialise the fireball object
    protected override void Setup()
	{
		ItemReference = Resources.Load(FileDir.FireBall) as GameObject;
        if (fireball == null)
        {
            fireball = MonoBehaviour.Instantiate(ItemReference);
        }
    }

	protected override string GetDescLevel(int lvl)
	{
		return "Does "+(50+5*(lvl-1))+"% of your final damage (" + (int)(PStats.Combat.damage * (0.5 + 0.05*(lvl-1))) + ") on impact and "+(150+5*(lvl-1))+"% of your final damage (" + (int)(PStats.Combat.damage*(1.5+0.05*(lvl-1))) + ") as splash damage.";
	}

    //Fire the fireball
	public override void UseSkill()
	{
        Setup();
        fireball.GetComponent<GenericProjectile>().Shoot(ItemReference,Player,null, PlayerSave.staticplayer.GetComponent<PlayerStats>().Combat.damage, true);
	}

    protected override Skill MakeClone()
    {
        return new Explosion(CurrentLevel);
    }
}
