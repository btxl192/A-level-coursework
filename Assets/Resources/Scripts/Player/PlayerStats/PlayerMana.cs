using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour {

    [SerializeField]
    private int BaseMP;

    public int maxmana
    {
        get
        {
            //Objective 1.3.2.7.4.a
            int m = (int)(BaseMP * System.Math.Pow(1.1f, PlayerSave.staticplayer.GetComponent<PlayerStats>().level));
            foreach (PlayerStats.FlatBonus i in FlatMPBonuses)
            {
                m += i.Amount;
            }
            foreach (PlayerStats.MultBonus f in MultMPBonuses)
            {
                m = (int)(m * f.Amount);
            }
            return m;
        }
    }
    public int mana { get; private set; }

    void Start ()
    {
        CallMPChanged();
    }

    public void ChangeMana(int amount)
    {
        if (mana + amount > maxmana)
        {
            mana = maxmana;
        }
        else
        {
            mana += amount;
        }
        CallMPChanged();
    }

    private List<PlayerStats.FlatBonus> FlatMPBonuses = new List<PlayerStats.FlatBonus>();
    public List<PlayerStats.FlatBonus> FlatBonuses
    {
        get
        {
            return FlatMPBonuses;
        }
    }

    private List<PlayerStats.MultBonus> MultMPBonuses = new List<PlayerStats.MultBonus>();
    public List<PlayerStats.MultBonus> MultBonuses
    {
        get
        {
            return MultMPBonuses;
        }
    }

    public delegate void MPChangedEvent();
    public static event MPChangedEvent MPChanged;

    public void AddFlatMP(string identifier, int i)
    {
        FlatMPBonuses.Add(new PlayerStats.FlatBonus(identifier, i));
        CallMPChanged();
    }

    public void AddMultMP(string identifier, float f)
    {
        MultMPBonuses.Add(new PlayerStats.MultBonus(identifier, f));
        CallMPChanged();
    }

    //Call the event that says the player's health has changed
    void CallMPChanged()
    {
        if (MPChanged != null)
        {
            MPChanged();
        }
    }

    public void RemoveFlatMP(string identifier)
    {
        List<int> ToRemove = new List<int>();
        foreach (PlayerStats.FlatBonus b in FlatMPBonuses)
        {
            if (b.Identifier == identifier)
            {
                ToRemove.Add(FlatMPBonuses.IndexOf(b));
            }
        }
        foreach (int i in ToRemove)
        {
            FlatMPBonuses.RemoveAt(i);
        }
    }

    public void RemoveMultMP(string identifier)
    {
        List<int> ToRemove = new List<int>();
        foreach (PlayerStats.MultBonus b in MultMPBonuses)
        {
            if (b.Identifier == identifier)
            {
                ToRemove.Add(MultMPBonuses.IndexOf(b));
            }
        }
        foreach (int i in ToRemove)
        {
            MultMPBonuses.RemoveAt(i);
        }
    }

    public void SetMP(int i)
    {
        mana = i;
        CallMPChanged();
    }
}
