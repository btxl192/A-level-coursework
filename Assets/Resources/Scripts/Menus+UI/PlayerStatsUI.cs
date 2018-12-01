using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsUI : MonoBehaviour {

    private TextMeshProUGUI statstext;
    private GameObject statuscontent;
    private string info = "";
    private PlayerStats pstats;
    private PlayerEquipment playereq;

    private void Awake()
    {
        statstext = transform.Find("Scroll View/Viewport/Content/info").gameObject.GetComponent<TextMeshProUGUI>();
        statuscontent = transform.Find("Status Scroll View/Viewport/Content").gameObject;
        GenericMenu2.OnOpen += Refresh;
    }

    private void Start()
    {
        pstats = PlayerSave.staticplayer.GetComponent<PlayerStats>();
        playereq = PlayerSave.staticplayer.GetComponent<PlayerEquipment>();
    }

    private void OnDestroy()
    {
        GenericMenu2.OnOpen -= Refresh;
    }

    //Reload all stats
    public void Refresh(GameObject g)
    {
        if (g.name == "InvCanvas")
        {
            info = "";
            LoadWeapon();
            LoadArmour();
            LoadHealth();
            LoadMana();
            statstext.text = info;
            LoadStatuses();
        }
    }

    //Load the player's weapon stats and damage bonuses
    private void LoadWeapon()
    {
        if (playereq.Weapon != null)
        {
            info += string.Format("Final Damage: {0} \n", pstats.Combat.damage);
            info += string.Format("Attack Speed: {0} \n\n", pstats.Combat.attspeed);
        }
        else
        {
            info += "Final Damage: 0 \n\n";
        }
        if (pstats.Combat.GetFlatDamageBonuses().Count > 0)
        {
            info += "Flat Damage Bonuses: \n";
            foreach (PlayerStats.FlatBonus b in pstats.Combat.GetFlatDamageBonuses())
            {
                info += string.Format("{0}: {1}\n", b.Identifier, b.Amount);
            }
            info += "\n";
        }
        if (pstats.Combat.GetMultDamageBonuses().Count > 0)
        {
            info += "Multiplicative Damage Bonuses: \n";
            foreach (PlayerStats.MultBonus b in pstats.Combat.GetMultDamageBonuses())
            {
                string sign;
                if (b.Amount >= 0)
                {
                    sign = "+";
                }
                else
                {
                    sign = "-";
                }
                info += string.Format("{0}: {1}{2}%\n", b.Identifier, sign, (b.Amount - 1) * 100);
            }
            info += "\n";
        }
    }

    //Load the player's armour stats and defence bonuses
    private void LoadArmour()
    {
        info += string.Format("Final Armour: {0} \n\n", pstats.ArmourVals.armour);
        if (pstats.ArmourVals.GetMultArmourBonuses.Count > 0)
        {
            info += "Armour Multiplicative Bonuses:\n";
        }
        foreach (PlayerStats.MultBonus b in pstats.ArmourVals.GetMultArmourBonuses)
        {
            if (b.Amount > 0)
            {
                info += b.Identifier + ": +" + b.Amount * 100 + "%\n";
            }
            else if (b.Amount < 0)
            {
                info += b.Identifier + ": -" + b.Amount * 100 + "%\n";
            }
        }
        if (pstats.ArmourVals.GetFlatArmourBonuses.Count > 0)
        {
            info += "Armour Flat Bonuses:\n";
        }
        foreach (PlayerStats.FlatBonus b in pstats.ArmourVals.GetFlatArmourBonuses)
        {
            if (b.Amount > 0)
            {
                info += b.Identifier + ": +" + b.Amount + "\n";
            }
            else if (b.Amount < 0)
            {
                info += b.Identifier + ": -" + b.Amount + "\n";
            }
        }
        if (pstats.ArmourVals.GetMultDefenceBonuses.Count > 0)
        {
            info += "Defence Multiplicative Bonuses:\n";
        }
        foreach (PlayerStats.MultBonus b in pstats.ArmourVals.GetMultDefenceBonuses)
        {
            if (b.Amount > 0)
            {
                info += b.Identifier + ": +" + b.Amount * 100 + "%\n";
            }
            else if (b.Amount < 0)
            {
                info += b.Identifier + ": -" + b.Amount * 100 + "%\n";
            }
        }
        if (pstats.ArmourVals.GetFlatDefenceBonuses.Count > 0)
        {
            info += "Defence Flat Bonuses:\n";
        }
        foreach (PlayerStats.FlatBonus b in pstats.ArmourVals.GetFlatDefenceBonuses)
        {
            if (b.Amount > 0)
            {
                info += b.Identifier + ": -" + b.Amount + " damage taken\n";
            }
            else if (b.Amount < 0)
            {
                info += b.Identifier + ": +" + b.Amount + " damage taken\n";
            }
        }
    }

    //Load the player's health and health bonuses
    private void LoadHealth()
    {
        if (pstats.HP.FlatBonuses.Count > 0)
        {
            info += "Flat Health Bonuses: \n";
            foreach (PlayerStats.FlatBonus b in pstats.HP.FlatBonuses)
            {
                if (b.Amount != 0)
                {
                    info += string.Format("{0}: {1}", b.Identifier, b.Amount);
                }
            }
            info += "\n\n";
        }
        if (pstats.HP.MultBonuses.Count > 0)
        {
            info += "Multiplicative Health Bonuses: \n";
            foreach (PlayerStats.MultBonus b in pstats.HP.MultBonuses)
            {
                if (b.Amount != 0)
                {
                    string sign;
                    if (b.Amount >= 0)
                    {
                        sign = "+";
                    }
                    else
                    {
                        sign = "-";
                    }
                    info += string.Format("{0}: {1}{2}%", b.Identifier, sign, (b.Amount - 1) * 100);
                }
            }
            info += "\n\n";
        }
    }

    //Load the player's mana and mana bonuses
    private void LoadMana()
    {
        if (pstats.MP.FlatBonuses.Count > 0)
        {
            info += "Flat Mana Bonuses: \n";
            foreach (PlayerStats.FlatBonus b in pstats.MP.FlatBonuses)
            {
                if (b.Amount != 0)
                {
                    info += string.Format("{0}: {1}", b.Identifier, b.Amount);
                }
            }
            info += "\n\n";
        }
        if (pstats.MP.MultBonuses.Count > 0)
        {
            info += "Multiplicative Mana Bonuses: \n";
            foreach (PlayerStats.MultBonus b in pstats.MP.MultBonuses)
            {
                if (b.Amount != 0)
                {
                    string sign;
                    if (b.Amount >= 0)
                    {
                        sign = "+";
                    }
                    else
                    {
                        sign = "-";
                    }
                    info += string.Format("{0}: {1}{2}%", b.Identifier, sign, (b.Amount - 1) * 100);
                }
            }
            info += "\n\n";
        }
    }

    //Load the player's statuses
    private void LoadStatuses()
    {
        foreach (Transform t in statuscontent.transform)
        {
            Destroy(t.gameObject);
        }
        foreach (StatusBase s in pstats.gameObject.GetComponent<Statuses>().CurrentStatusEffects)
        {
            GameObject temp = Instantiate(Resources.Load(FileDir.StatusDetailsText) as GameObject, statuscontent.transform);
            temp.GetComponent<TextMeshProUGUI>().text = s.FullDesc;
            temp.GetComponentInChildren<UnityEngine.UI.Image>().sprite = s.Icon.GetComponentInChildren<UnityEngine.UI.Image>().sprite;
        }
    }
}
