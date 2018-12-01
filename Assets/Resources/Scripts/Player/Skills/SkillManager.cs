using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Objective 1.3.2.7.8
public class SkillManager : MonoBehaviour {

    private static readonly Dictionary<int, Skill> _SkillDict = new Dictionary<int, Skill> ()
	{
		{0, new Toxic(0)},
		{1, new Blizzard(0)},
		{2, new Explosion(0)},
		{3, new Smog(0)},
        {4, new ChainLightning(0) },
        {5, new ManaVampire(0) },
        {6, new Durable(0) },
        {7, new ManaReservoir(0) },
        {8, new Rage(0) },
        {9, new MedicalExpert(0) },
        {10, new ManaEfficiency(0) },
        {11, new Deadly(0) },
        {12, new TriggerHappy(0) },
        {13, new GetHyped(0) },
        {14, new SwiftDefence(0) },
        {15, new NervesOfSteel(0) },
        {16, new SneakAttack(0) }
    };

    public static int GetSkillID(Skill s)
    {
        int count = 0;
        foreach (Skill s2 in _SkillDict.Values)
        {
            if (s.GetType() == s2.GetType())
            {
                return count;
            }
            else
            {
                count += 1;
            }
        }
        return -1;
    }

    //Returns the skill from the skill dictionary at the index specified
    public static Skill SkillDict(int SkillID)
    {
        return _SkillDict[SkillID].Clone();
    }

    public static int SkillDictCount
    {
        get
        {
            return _SkillDict.Count;
        }
    }

	protected static Dictionary<int, Skill> PlayerSkills = new Dictionary<int, Skill> ();
    public static Dictionary<int, Skill> Skills
    {
        get { return PlayerSkills;  }
    }

    protected static List<Skill.SkillSets> AvailableSkillSets = new List<Skill.SkillSets>()
    {
        Skill.SkillSets.General,
        Skill.SkillSets.Combat
    };

    public static List<Skill.SkillSets> PlayerSkillSets
    {
        get
        {
            return AvailableSkillSets;
        }
    }

    public static void AddPlayerSkillSet(Skill.SkillSets s)
    {
        AvailableSkillSets.Add(s);
        if (SkillSetsChanged != null)
        {
            SkillSetsChanged();
        }
    }

    public delegate void BlankEvent();
	public static event BlankEvent UpdateButtonText;
	public static event BlankEvent Ready;
    public static event BlankEvent UpdateSkillCores;
    public static event BlankEvent SkillSetsChanged;

    public delegate void AddSkillButtonsEvent(List<Skill> Skills);
	public static event AddSkillButtonsEvent RunAddSkillButton;

	protected static GameObject Player;
	protected static SkillCore SkillCoreEquipped;

    //Adds all skills in the skill dictionary to the player's skills
    public static void InitialiseSkills()
    {
        if (PlayerSkills.Count == 0)
        {
            for (int a = 0; a < _SkillDict.Count; a++)
            {
                PlayerSkills.Add(a, SkillDict(a).Clone());
            }
        }
    }

	protected void Start()
	{
        
		Player = PlayerSave.staticplayer;
		SkillButton.IncreaseSkill += IncreaseSkill;
		SkillsetDropdown.UpdateSkills += UpdateSkills;
        InitialiseSkills();
        foreach (Skill s in PlayerSkills.Values)
        {
            if (s.SkillType == Skill.SkillTypes.Active)
            {
                ((ActiveSkill)s).RunSetup();
            }
        }
        if (Ready != null)
        {
            Ready();
        }        
        RefreshSkillcore(null);
        PlayerEquipment.ChangedSkillCore += RefreshSkillcore;
        if (UpdateSkillCores != null)
        {
            UpdateSkillCores();
        }
        
	}

    //Updates the player's skill levels according to the player's skill core
    //Objective 1.3.2.7.8.c
	public void RefreshSkillcore(GameObject i)
	{
        try
        {
            if (Player.GetComponent<PlayerEquipment>().SkillCore != null)
            {
                SkillCoreEquipped = Player.GetComponent<PlayerEquipment>().SkillCore.GetComponent<SkillCore>();
                foreach (Skill s in SkillCoreEquipped.Skills)
                {
                    foreach (Skill s2 in PlayerSkills.Values)
                    {
                        if (s.GetType() == s2.GetType())
                        {
                            s2.SetOverLevel(s.Level);
                        }
                    }
                }
                RunUpdateButtonText();
                UpdatePlayerStats();
            }
        }
        catch
        {
            Debug.Log("ERROR: problem with skill core");
        }
    }

	public virtual void IncreaseSkill(bool active, int SkillID, int Levels)
	{
		Increment(SkillID, Levels);
    }

	public void RunUpdateButtonText()
	{
        if (UpdateButtonText != null)
        {
            UpdateButtonText();
        }
	}

    //Reload the skill buttons on the skill UI
    public void UpdateSkills(Skill.SkillSets SkillSet)
    {
        List<Skill> temp = new List<Skill>();
		foreach(Skill s in PlayerSkills.Values)
		{
			if(s.SkillSet == SkillSet)
			{
				temp.Add(s);
			}
		}
        if (RunAddSkillButton != null)
        {
            RunAddSkillButton(temp);
        }		
        UpdatePlayerStats();
		RunUpdateButtonText();
    }

    //Upgrade a skill
	public void Increment(int SkillID, int Levels)
	{
		PlayerSkills[SkillID].IncreaseLevel(Levels);        
        UpdateButtonText();
        UpdatePlayerStats();
	}

    //Gets the index of a skill in PlayerSkills
	public static int GetPlayerSkillIndex(Skill skill, bool matchlevel)
	{
        for(int a = 0; a < PlayerSkills.Count; a++)
        {
            Skill s = PlayerSkills[a];
            if (skill.SkillName == s.SkillName)
            {
                if (matchlevel)
                {
                    if (s.CurrentLevel >= skill.CurrentLevel)
                    {
                        return a;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return a;
                }
            }
        }
        return -1;
	}

    public static string[] GetSkillNames()
    {
        List<string> skillnames = new List<string>();
        foreach (Skill s in _SkillDict.Values)
        {
            skillnames.Add(s.SkillName);
        }
        return skillnames.ToArray();
    }

    //Updates the player's stats according to the skills which change the player's stats
    void UpdatePlayerStats()
    {
        PlayerStats PStats = Player.GetComponent<PlayerStats>();
        if (((Durable)SkillDict(6)).HPincreaseamount * PlayerSkills[6].Level > 0)
        {
            PStats.HP.RemoveFlatHP("Durable");
            PStats.HP.AddFlatHP("Durable", ((Durable)SkillDict(6)).HPincreaseamount * PlayerSkills[6].Level);
        }
        if (((ManaReservoir)SkillDict(7)).MPincreaseamount * PlayerSkills[7].Level > 0)
        {
            PStats.MP.RemoveFlatMP("ManaReservoir");
            PStats.MP.AddFlatMP("ManaReservoir", ((ManaReservoir)SkillDict(7)).MPincreaseamount * PlayerSkills[7].Level);
        }
        if (((Deadly)(PlayerSkills[11])).DamageIncreaseAmount > 0)
        {
            PStats.Combat.RemoveMultDmgBuff("Deadly");
            PStats.Combat.AddMultDamage("Deadly", ((Deadly)(PlayerSkills[11])).DamageIncreaseAmount + 1);
        }
        if (((TriggerHappy)(PlayerSkills[12])).DecreaseAmount > 0)
        {
            PStats.Combat.RemoveMultAttSpdBuff("Trigger Happy");
            PStats.Combat.AddMultAttSpeed("Trigger Happy", 1 - ((TriggerHappy)(PlayerSkills[12])).DecreaseAmount);
        }
        if (((NervesOfSteel)(PlayerSkills[15])).IncreaseAmount > 0)
        {
            PStats.ArmourVals.RemoveFlatArmour("Nerves of Steel");
            PStats.ArmourVals.AddFlatArmour("Nerves of Steel", ((NervesOfSteel)(PlayerSkills[15])).IncreaseAmount);
        }
    }

    void OnDestroy()
    {
        SkillButton.IncreaseSkill -= IncreaseSkill;
        SkillsetDropdown.UpdateSkills -= UpdateSkills;
    }


}
