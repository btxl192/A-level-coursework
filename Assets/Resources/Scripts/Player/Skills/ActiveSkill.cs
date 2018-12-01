using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.7.8.a
public abstract class ActiveSkill : Skill {

    public abstract bool HoldKey { get;}
    public abstract int ManaCost { get; }
    public int FinalManaCost
    {
        get
        {
            return (int)(ManaCost * (1 - ((ManaEfficiency)SkillManager.Skills[10]).ManaDiscount));
        }
    }
    protected GameObject Player { get { return PlayerSave.staticplayer; } }
    protected PlayerStats PStats { get { return Player.GetComponent<PlayerStats>(); } }
    public bool SetupDone { get; protected set; }
    public virtual float Cooldown
    {
        get
        {
            return 0;
        }
    }
    protected bool CanUseSkill;

    public ActiveSkill(int Lv) : base(Lv)
    {      
    }

    //Uses the skill
    public void Activate()
    {
        //Adds the skill to the skill cooldowns dictionary
        if (!ActiveSkillManager.PlayerSkillCooldowns.ContainsKey(SkillManager.GetSkillID(this)))
        {
            ActiveSkillManager.PlayerSkillCooldowns.Add(SkillManager.GetSkillID(this), 0);
        }
        if (Level > 0 && !Pause.Paused)
        {
            //If the cooldown has passed, use the skill
            if ((Time.timeSinceLevelLoad - ActiveSkillManager.PlayerSkillCooldowns[SkillManager.GetSkillID(this)]) >= Cooldown)
            {
                if ((PlayerSave.staticplayer.GetComponent<PlayerStats>().MP.mana - FinalManaCost) >= 0)
                {
                    UseSkill();
                    CanUseSkill = false;
                    ActiveSkillManager.PlayerSkillCooldowns[SkillManager.GetSkillID(this)] = Time.timeSinceLevelLoad;
                    PlayerSave.staticplayer.GetComponent<PlayerStats>().MP.ChangeMana(-FinalManaCost);
                }
                else
                {
                    TempPopup.Show("Not enough mana to cast skill!", Color.red);
                }
            }
            else
            {
                TempPopup.Show("Skill still on cooldown!", Color.red);
            }
        }
    }

    public abstract void UseSkill();

    //Initialise the skill
    public void RunSetup()
    {
        if (!SetupDone)
        {
            Setup();
            SetupDone = true;
        }
    }

    //Any initialisation the skill may require
    protected virtual void Setup() { }
}
