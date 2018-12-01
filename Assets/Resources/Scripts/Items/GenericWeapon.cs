using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericWeapon : GenericItem
{

    public bool isPlayerWeapon { get; set; }

    [SerializeField]
    protected int _damage;
    public int damage
    {
        get {return _damage; }
    }

    [SerializeField]
    protected static List<string> _whitelisttags = new List<string>() //list of tags that the weapon will not affect
    {
        {"Projectile"}
    };
    public static List<string> whitelisttags
    {
        get {return _whitelisttags; }
    }

    protected List<string> _ExtraWhiteListtags = new List<string>();
    public List<string> ExtraWhiteListtags
    {
        get {return _ExtraWhiteListtags; }
    }

    //Objective 1.3.2.3.1
    [SerializeField, HideInInspector]
	protected List<Statuses.StatusType> _StatusEffects = new List<Statuses.StatusType>();
    public List<Statuses.StatusType> StatusEffects
    {
        get {return _StatusEffects; }
    }

    [SerializeField]
    protected float _AttSpeed;
    public float AttSpeed
    {
        get {return _AttSpeed; }
    }

    public override string FormattedDesc
    {
        get
        {
            string d = base.FormattedDesc;
            d += "\n\nBase Damage: " + damage;
            if (StatusEffects.Count > 0)
            {
                d += "\n\nStatuses:";
                foreach (Statuses.StatusType s in StatusEffects)
                {
                    d += string.Format("\n+{0} {1}", s.Level, Statuses.StatusString[s.Type]); 
                }
            }
            return d;
        }
    }

    //If the projectile collides with anything not whitelisted
    protected virtual void OnTriggerEnter(Collider other)
	{
		try
		{
			if (whitelisttags.IndexOf(other.GetComponent<Collider>().tag) == -1 && ExtraWhiteListtags.IndexOf(other.GetComponent<Collider>().tag) == -1)
			{
				if(other.tag == "Player")
				{
					other.GetComponent<PlayerBehaviour>().takedamage(damage);
				}
                //Objective 1.3.2.3.1
                foreach (Statuses.StatusType s in StatusEffects)
                {
                    if (other.GetComponent<Statuses>() != null)
                    {
                        other.GetComponent<Statuses>().ApplyStatus(s, this.gameObject);
                    }
                }
			}
		}
		catch
		{
          Debug.Log("ERROR in weapon: error dealing damage.");  
		}
	}

    //Check if something is whitelisted
    public bool WhiteListed(GameObject thing)
    {
        if(whitelisttags.Contains(thing.tag) || ExtraWhiteListtags.Contains(thing.tag))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddToWhiteList(string tag)
    {
        ExtraWhiteListtags.Add(tag);
    }

    public virtual void RandomiseStats(GenericItem.Rarities rarity, float multiplier)
    {}

    public void SetStatuses(List<Statuses.StatusType> statuseslist)
    {
        _StatusEffects = statuseslist;
    }

    //Add a status effect to the weapon
    public void AddStatus(Statuses.StatusType s)
    {
        _StatusEffects.Add(s);
    }

    public override string GetSaveData()
    {
        string data = base.GetSaveData();
        data += "," + damage;
        foreach (Statuses.StatusType s in StatusEffects)
        {
            data += string.Format(",{0}/{1}", s.Type, s.Level);
        }
        return data;
    }

    public void SetDamage(int dmg)
    {
        _damage = dmg;
    }

}
