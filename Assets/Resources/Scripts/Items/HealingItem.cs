using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.1.3
public class HealingItem : UsableItem {

    public override string FormattedDesc
    {
        get
        {
            string d = base.FormattedDesc;
            if (HpHealAmount > 0)
            {
                d += "\n\nRestores " + HpHealAmount + "HP";
            }
            if (MpHealAmount > 0)
            {
                d += "\n\nRestores " + MpHealAmount + "MP";
            }
            return d;
        }
    }

    [SerializeField]
    protected int _HpHealAmount;
    public int HpHealAmount
    {
        get { return _HpHealAmount; }
    }

    [SerializeField]
    protected int _MpHealAmount;
    public int MpHealAmount
    {
        get { return _MpHealAmount; }
    }

    [SerializeField]
    private bool Percentage;

    protected override void Awake()
    {
        base.Awake();
		_category = CategoryEnum.HealingItem;
    }

    public override string GetSaveData()
    {
        string data = base.GetSaveData();
        data += string.Format(",{0},{1},{2}", Percentage, _HpHealAmount, _MpHealAmount);
        return data;
    }

    public void SetHealDetails(bool percentage, int HPHeal, int MPHeal)
    {
        Percentage = percentage;
        _HpHealAmount = HPHeal;
        _MpHealAmount = MPHeal;
    }

    //Decrease the amount by 1 and heal the player
    public override void UseItem()
    {
        PlayerStats PStats = PlayerSave.staticplayer.GetComponent<PlayerStats>();
        float healmult = 1 + ((MedicalExpert)SkillManager.Skills[SkillManager.GetSkillID(new MedicalExpert(0))]).HealMult;
        if (PStats.HP.health < PStats.HP.maxhealth)
        {            
            PStats.HP.ChangeHealth((int)(HpHealAmount * healmult));
        }
        if (PStats.MP.mana < PStats.MP.maxmana)
        {
            PStats.MP.ChangeMana((int)(MpHealAmount * healmult));
        }
        ChangeAmount(-1);
    }

}
