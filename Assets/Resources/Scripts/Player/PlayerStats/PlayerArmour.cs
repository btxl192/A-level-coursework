using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmour : MonoBehaviour {

    public delegate void BlankEvent();
    public static event BlankEvent ArmourUpdated;

    //Objective 1.3.2.1.4.a
    private int _armour;
    public int armour
    {
        get
        {
            int a = _armour;
            foreach (PlayerStats.MultBonus mult in MultArmourBonuses)
            {
                a = (int)((float)a * mult.Amount);
            }
            foreach (PlayerStats.FlatBonus bonus in FlatArmourBonuses)
            {
                a += bonus.Amount;
            }
            return a;
        }
        set
        {
            _armour = value;
        }
    }
    [SerializeField, HideInInspector]
    private GenericArmour PlayerArmourObject;
    private List<PlayerStats.FlatBonus> FlatArmourBonuses = new List<PlayerStats.FlatBonus>();
    public List<PlayerStats.FlatBonus> GetFlatArmourBonuses
    {
        get
        {
            return FlatArmourBonuses;
        }
    }
    private List<PlayerStats.MultBonus> MultArmourBonuses = new List<PlayerStats.MultBonus>();
    public List<PlayerStats.MultBonus> GetMultArmourBonuses
    {
        get
        {
            return MultArmourBonuses;
        }
    }

    private List<PlayerStats.FlatBonus> FlatDefenceBonuses = new List<PlayerStats.FlatBonus>();
    public List<PlayerStats.FlatBonus> GetFlatDefenceBonuses
    {
        get
        {
            return FlatDefenceBonuses;
        }
    }
    private List<PlayerStats.MultBonus> MultDefenceBonuses = new List<PlayerStats.MultBonus>();
    public List<PlayerStats.MultBonus> GetMultDefenceBonuses
    {
        get
        {
            return MultDefenceBonuses;
        }
    }

    void Start ()
    {
        PlayerEquipment.ChangedArmour += UpdateArmour;
        try
        {
            PlayerArmourObject = this.gameObject.GetComponent<PlayerEquipment>().ArmourObject.GetComponent<GenericArmour>();
        }
        catch { }
    }

    public void AddFlatArmour(string identifier, int i)
    {
        FlatArmourBonuses.Add(new PlayerStats.FlatBonus(identifier, i));
    }

    public void RemoveFlatArmour(string identifier)
    {
        List<int> toremove = new List<int>();
        for (int a = 0; a < FlatArmourBonuses.Count; a++)
        {
            if (FlatArmourBonuses[a].Identifier == identifier)
            {
                toremove.Add(a);
            }
        }
        for (int a = toremove.Count - 1; a >= 0; a--)
        {
            FlatArmourBonuses.RemoveAt(a);
        }
    }

    public void AddMultArmour(string identifier, float f)
    {
        MultArmourBonuses.Add(new PlayerStats.MultBonus(identifier, f));
    }

    public void RemoveMultArmour(string identifier)
    {
        List<int> toremove = new List<int>();
        for (int a = 0; a < MultArmourBonuses.Count; a++)
        {
            if (MultArmourBonuses[a].Identifier == identifier)
            {
                toremove.Add(a);
            }
        }
        for (int a = toremove.Count - 1; a >= 0; a--)
        {
            MultArmourBonuses.RemoveAt(a);
        }
    }

    //Update the armour values of the player based on the player's current armour
    void UpdateArmour(GameObject a)
    {
        if (a != null)
        {
            if (a.GetComponent<GenericArmour>() != null)
            {
                PlayerArmourObject = a.GetComponent<GenericArmour>();
                armour = PlayerArmourObject.Armour;
                if (ArmourUpdated != null)
                {
                    ArmourUpdated();
                }
            }
        }
    }

    public void AddFlatDefence(string identifier, int i)
    {
        FlatDefenceBonuses.Add(new PlayerStats.FlatBonus(identifier, i));
    }

    public void RemoveFlatDefence(string identifier)
    {
        List<int> toremove = new List<int>();
        for (int a = 0; a < FlatDefenceBonuses.Count; a++)
        {
            if (FlatDefenceBonuses[a].Identifier == identifier)
            {
                toremove.Add(a);
            }
        }
        for (int a = toremove.Count - 1; a >= 0; a--)
        {
            FlatDefenceBonuses.RemoveAt(a);
        }
    }

    public void AddMultDefence(string identifier, float f)
    {
        MultDefenceBonuses.Add(new PlayerStats.MultBonus(identifier, f));
    }

    public void RemoveMultDefence(string identifier)
    {
        List<int> toremove = new List<int>();
        for (int a = 0; a < MultDefenceBonuses.Count; a++)
        {
            if (MultDefenceBonuses[a].Identifier == identifier)
            {
                toremove.Add(a);
            }
        }
        for (int a = toremove.Count - 1; a >= 0; a--)
        {
            MultDefenceBonuses.RemoveAt(a);
        }
    }

    //Get the resultant damage the player takes based on the player's armour and defence
    //Objective 1.3.2.1.4.b
    public int GetResultantDamage(int damage)
    {       
        float dmg = damage;
        dmg *= 1 - (armour / (300f + armour));
        foreach (PlayerStats.MultBonus b in MultDefenceBonuses)
        {
            dmg *= (1 - b.Amount);
        }
        foreach (PlayerStats.FlatBonus b in FlatDefenceBonuses)
        {
            dmg -= b.Amount;
        }
        return (int)dmg;
    }

    //Check if the specified multiplicative defence bonus exists
    public bool ContainsMultDefenceBonus(string identifier)
    {
        foreach (PlayerStats.MultBonus b in MultDefenceBonuses)
        {
            if (identifier == b.Identifier)
            {
                return true;
            }
        }
        return false;
    }
}
