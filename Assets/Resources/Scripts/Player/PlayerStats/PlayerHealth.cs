using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [SerializeField]
    private int BaseHP;

    public int maxhealth
    {
        get
        {
            //Objective 1.3.2.7.3.a
            int h = (int)(BaseHP * System.Math.Pow(1.1f, PlayerSave.staticplayer.GetComponent<PlayerStats>().level));
            foreach (PlayerStats.FlatBonus b in FlatHPBonuses)
            {
                h += b.Amount;
            }
            foreach (PlayerStats.MultBonus b in MultHPBonuses)
            {
                h = (int)(h * b.Amount);
            }
            return h;
        }
    }
    [SerializeField]
    public int health { get; private set; }

    void Start ()
    {
        CallHPChanged();
    }

    public void ChangeHealth(int amount)
    {
        if (health + amount > maxhealth)
        {
            health = maxhealth;
        }
        else
        {
            health += amount;
        }
        CallHPChanged();
    }

    private List<PlayerStats.FlatBonus> FlatHPBonuses = new List<PlayerStats.FlatBonus>();
    public List<PlayerStats.FlatBonus> FlatBonuses
    {
        get
        {
            return FlatHPBonuses;
        }
    }

    private List<PlayerStats.MultBonus> MultHPBonuses = new List<PlayerStats.MultBonus>();
    public List<PlayerStats.MultBonus> MultBonuses
    {
        get
        {
            return MultHPBonuses;
        }
    }

    public delegate void HPChangedEvent();
    public static event HPChangedEvent HPChanged;

    public void AddFlatHP(string identifier, int i)
    {
        FlatHPBonuses.Add(new PlayerStats.FlatBonus(identifier, i));
        CallHPChanged();
    }

    public void AddMultHP(string identifier, float f)
    {
        MultHPBonuses.Add(new PlayerStats.MultBonus(identifier, f));
        CallHPChanged();
    }

    //Call the event that says the player's health has changed
    void CallHPChanged()
    {
        if (HPChanged != null)
        {
            HPChanged();
        }
    }

    public void RemoveFlatHP(string identifier)
    {
        List<int> ToRemove = new List<int>();
        foreach (PlayerStats.FlatBonus b in FlatHPBonuses)
        {
            if (b.Identifier == identifier)
            {
                ToRemove.Add(FlatHPBonuses.IndexOf(b));
            }
        }
        foreach (int i in ToRemove)
        {
            FlatHPBonuses.RemoveAt(i);
        }
    }

    public void RemoveMultHP(string identifier)
    {
        List<int> ToRemove = new List<int>();
        foreach (PlayerStats.MultBonus b in MultHPBonuses)
        {
            if (b.Identifier == identifier)
            {
                ToRemove.Add(MultHPBonuses.IndexOf(b));
            }
        }
        foreach (int i in ToRemove)
        {
            MultHPBonuses.RemoveAt(i);
        }
    }

    public void SetHP(int i)
    {
        health = i;
        CallHPChanged();
    }
}
