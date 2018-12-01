using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    private GenericWeapon PlayerWeapon;
    private int _damage;
    public int damage
    {
        get
        {
            int d = (int)(_damage * (1 + ((this.gameObject.GetComponent<PlayerStats>().level - 1) * 0.2f)));
            foreach (PlayerStats.MultBonus mult in MultDamageBonuses)
            {
                d = (int)((float)d * mult.Amount);
            }
            foreach (PlayerStats.FlatBonus bonus in FlatDamageBonuses)
            {
                d += bonus.Amount;
            }
            return d;
        }
        set
        {
            _damage = value;
        }
    }
    public float attspeed
    {
        get
        {
            float f = PlayerWeapon.AttSpeed;
            foreach (PlayerStats.FlatBonus b in FlatAttSpeedBonuses)
            {
                f -= b.Amount;
            }
            foreach (PlayerStats.MultBonus b in MultAttSpeedBonuses)
            {
                f *= b.Amount;
            }
            return f;
        }
    }

    private List<PlayerStats.FlatBonus> FlatDamageBonuses = new List<PlayerStats.FlatBonus>();
    private List<PlayerStats.MultBonus> MultDamageBonuses = new List<PlayerStats.MultBonus>();

    public void Start()
    {
        PlayerEquipment.ChangedWeapon += UpdateDamage;
        try
        {
            PlayerWeapon = this.gameObject.GetComponent<PlayerEquipment>().Weapon;
        }
        catch { }
        try
        {
            UpdateDamage(PlayerWeapon.gameObject);
        }
        catch { }
    }

    public void AddFlatDamage(string identifier, int i)
    {
        FlatDamageBonuses.Add(new PlayerStats.FlatBonus(identifier, i));
    }

    public void AddMultDamage(string identifier, float f)
    {
        MultDamageBonuses.Add(new PlayerStats.MultBonus(identifier, f));
    }

    private List<PlayerStats.FlatBonus> FlatAttSpeedBonuses = new List<PlayerStats.FlatBonus>();
    private List<PlayerStats.MultBonus> MultAttSpeedBonuses = new List<PlayerStats.MultBonus>();

    public void AddFlatAttSpeed(string identifier, int i)
    {
        FlatAttSpeedBonuses.Add(new PlayerStats.FlatBonus(identifier, i));
    }

    public void AddMultAttSpeed(string identifier, float f)
    {
        MultAttSpeedBonuses.Add(new PlayerStats.MultBonus(identifier, f));
    }

    //Update the player's damage based on the player's currently equipped weapon
    public void UpdateDamage(GameObject weapon)
    {
        if (weapon != null)
        {
            PlayerWeapon = weapon.GetComponent<GenericWeapon>();
        }      
        _damage = PlayerWeapon.damage;
        
    }

    public void RemoveFlatDmgBuff(string identifier)
    {
        List<int> ToRemove = new List<int>();
        foreach (PlayerStats.FlatBonus b in FlatDamageBonuses)
        {
            if (b.Identifier == identifier)
            {
                ToRemove.Add(FlatDamageBonuses.IndexOf(b));
            }
        }
        foreach (int i in ToRemove)
        {
            FlatDamageBonuses.RemoveAt(i);
        }
    }

    public void RemoveMultDmgBuff(string identifier)
    {
        List<int> ToRemove = new List<int>();
        foreach (PlayerStats.MultBonus b in MultDamageBonuses)
        {
            if (b.Identifier == identifier)
            {
                ToRemove.Add(MultDamageBonuses.IndexOf(b));
            }
        }
        foreach (int i in ToRemove)
        {
            MultDamageBonuses.RemoveAt(i);
        }
    }

    public void RemoveFlatAttpdBuff(string identifier)
    {
        List<int> ToRemove = new List<int>();
        foreach (PlayerStats.FlatBonus b in FlatAttSpeedBonuses)
        {
            if (b.Identifier == identifier)
            {
                ToRemove.Add(FlatAttSpeedBonuses.IndexOf(b));
            }
        }
        foreach (int i in ToRemove)
        {
            FlatAttSpeedBonuses.RemoveAt(i);
        }
    }

    public void RemoveMultAttSpdBuff(string identifier)
    {
        List<int> ToRemove = new List<int>();
        foreach (PlayerStats.MultBonus b in MultAttSpeedBonuses)
        {
            if (b.Identifier == identifier)
            {
                ToRemove.Add(MultAttSpeedBonuses.IndexOf(b));
            }
        }
        foreach (int i in ToRemove)
        {
            MultAttSpeedBonuses.RemoveAt(i);
        }
    }

    public List<PlayerStats.FlatBonus> GetFlatDamageBonuses()
    {
        return FlatDamageBonuses;
    }

    public List<PlayerStats.MultBonus> GetMultDamageBonuses()
    {
        return MultDamageBonuses;
    }

    public List<PlayerStats.FlatBonus> GetFlatAttSpeedBonuses()
    {
        return FlatAttSpeedBonuses;
    }

    public List<PlayerStats.MultBonus> GetMultAttSpeedBonuses()
    {
        return MultAttSpeedBonuses;
    }
}
