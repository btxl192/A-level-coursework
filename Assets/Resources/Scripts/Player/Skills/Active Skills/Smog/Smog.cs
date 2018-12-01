using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.f.ii.1.b
public class Smog : ActiveSkill {

    public GameObject ItemReference;
    [SerializeField]
	private static GameObject SmogCloud;
	private PlayerStats player;

    public Smog(int Lv) : base(Lv)
    {
    }

    public override SkillSets SkillSet{get {return SkillSets.Pyromaniac; }}

    public override string SkillName { get { return "Smog"; } }

    public override int ManaCost
    {
        get
        {
            return 20;
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

    public override float Cooldown
    {
        get
        {
            return 5f;
        }
    }

    //Initialise the smog cloud
    protected override void Setup()
	{
		ItemReference = Resources.Load(FileDir.Sphere) as GameObject;
        if (SmogCloud == null)
        {
            SmogCloud = MonoBehaviour.Instantiate(ItemReference);
        }
        player = PlayerSave.staticplayer.GetComponent<PlayerStats>();
	}

    //amount of poison damage to deal based on the level
	int damage(int lvl)
	{
		return (int)(player.Combat.damage * (0.75+0.05*lvl)); 
	}

	protected override string GetDescLevel(int lvl)
	{
		return "Creates a poisonous gas cloud of " + GetSmogSize(lvl).localScale.x + " units in diameter." + "\nDoes "+(75+5*lvl) + "% ("+damage(lvl)+") of your final damage as poison damage.";
	}

    //Sets the size of the smog cloud and returns the value
	public Transform GetSmogSize(int lvl)
	{
        Setup();
		SmogCloud.transform.localScale = ItemReference.transform.localScale * Mathf.Pow(1.05f,lvl);
		return SmogCloud.transform;
	}

    //Instantiate the smog cloud at the player's location
	public override void UseSkill()
	{
		GameObject WorldSmogCloud = MonoBehaviour.Instantiate(SmogCloud);
		WorldSmogCloud.transform.localScale = GetSmogSize(Level).localScale;
		WorldSmogCloud.GetComponent<Renderer> ().material.color = new Color (0f,0f,0f,0.7f);
		WorldSmogCloud.transform.position = Player.transform.position;
		WorldSmogCloud.AddComponent<SmogGas>();
        WorldSmogCloud.GetComponent<SmogGas>().SetLevel(Level);
		WorldSmogCloud.GetComponent<SmogGas>().SetDamage(damage(Level));
		WorldSmogCloud.SetActive(true);
	}

    protected override Skill MakeClone()
    {
        return new Smog(CurrentLevel);
    }
}
