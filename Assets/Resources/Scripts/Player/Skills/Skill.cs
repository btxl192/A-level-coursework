using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Skill {

	public enum SkillSets { General, Assassin, Pyromaniac, Infrigidare, Combat };
    public enum SkillTypes { Active, Passive, Buff};
    protected static Dictionary<SkillSets, string> SkillSetNames = new Dictionary<SkillSets, string>()
    {
        {SkillSets.General, "General" },
        {SkillSets.Assassin, "Assassin" },
        {SkillSets.Infrigidare, "Infrigidare"},
        {SkillSets.Pyromaniac, "Pyromaniac" }      
    };

    //Objective 1.3.2.7.8.f
	public abstract SkillSets SkillSet{get;}
	public abstract SkillTypes SkillType{get;}
	public abstract string SkillName{get;}
    public int Level
    {
        get { return CurrentLevel + OverLevel; }
    }

    //Objective 1.3.2.7.8.d
    [SerializeField]
    protected int _CurrentLevel;
	public int CurrentLevel
    {
        get
        {
            return _CurrentLevel;
        }
    }
    public int OverLevel { get; protected set; }
	public abstract int MaxLevel{get; }
	public string SkillDesc{get; protected set; }
    public virtual int ReqLevel { get;}
    public virtual List<Skill> ReqSkills
    {
        get
        {
            return new List<Skill>();
        }
    }
    public bool DontCheckReqSkills { get; }

	public Skill(int Lv)
	{       
		PlayerStats.CheckStats += UpdateSkillDesc;
		PlayerInventory.ChangedEQ += UpdateSkillDesc;
		SkillManager.Ready += UpdateSkillDesc;
        _CurrentLevel = Lv;
	}

    //Increase the level of the skill and takes away a skill point from the player
    //Objective 1.3.2.7.8.d.i
    public void IncreaseLevel(int i)
	{
		for(int a = 0; a < i; a++)
		{
			if (CurrentLevel < MaxLevel && PlayerSave.staticplayer.GetComponent<PlayerStats>().level >= ReqLevel && ReqSkillsMet())
			{
                _CurrentLevel += 1;
                PlayerSave.staticplayer.GetComponent<PlayerStats>().ChangeSkillPoints(-1);
                OnLevelIncrease();
				try
				{
					UpdateSkillDesc();
				}
				catch
				{
					Debug.Log("problem with upgrading skill");
				}
			}
		}
	}

    public void SetLevel(int i)
    {
        _CurrentLevel = i;
    }

    //Runs when the level of the skill increases
    protected virtual void OnLevelIncrease()
    { }

    public void SetOverLevel(int i)
    {
        OverLevel = i;
    }

    //Updates the description of the skill
	public virtual void UpdateSkillDesc()
	{
		try
		{
			if(CurrentLevel == 0)
			{
				SkillDesc = "\n[Level 1]\n" + GetDescLevel(1);
			}
			else
			{
				SkillDesc = "\n[Level "+ CurrentLevel + "]\n" + GetDescLevel(CurrentLevel);
				if(CurrentLevel < MaxLevel)
				{
					SkillDesc+="\n\n[Level "+(CurrentLevel + 1)+"]\n" + GetDescLevel(CurrentLevel + 1);
				}
			}
		}
		catch
		{
			Debug.Log(SkillName + "'s skill description could not be loaded");
		}
	}

    //Creates a copy of the skill
    public Skill Clone()
    {
        Skill temp = MakeClone();
        try
        {
            ((ActiveSkill)temp).RunSetup();
        }
        catch { }
        return temp;
    }

    protected abstract Skill MakeClone();

    //Returns the description of the skill at the specified level
    public string GetSkillDescription(int lvl)
    {
        if (lvl <= 0)
        {
            return GetDescLevel(1);
        }
        else
        {
            return GetDescLevel(lvl);
        }
    }

	protected abstract string GetDescLevel(int lvl);

    //Get the names of each skill set
    public static List<string> GetSkillSetNames()
    {
        List<string> temp = new List<string>();
        foreach (SkillSets s in SkillSetNames.Keys)
        {
            temp.Add(SkillSetNames[s]);
        }
        return temp;
    }

    //Gets each skill set
    public static List<SkillSets> GetSkillSets()
    {
        List<SkillSets> temp = new List<SkillSets>();
        foreach (SkillSets s in SkillSetNames.Keys)
        {
            temp.Add(s);
        }
        return temp;
    }

    //Determines whether the skill's required skills have all been upgraded
    public bool ReqSkillsMet()
    {
        int count = 0;
        foreach (Skill s in ReqSkills)
        {
            if (SkillManager.GetPlayerSkillIndex(s, true) > -1)
            {
                count += 1;
            }
        }
        return count == ReqSkills.Count;
    }

    //Checks if the skill is dependent on the skill specified
    public bool DependentOn(Skill skill)
    {
        foreach (Skill s in ReqSkills)
        {
            if (s.GetType() == skill.GetType())
            {
                return true;
            }
        }
        return false;
    }

    //Gets the degree of the skill's dependency
    public int GetDependencyCount()
    {

        int i = 0;
        foreach (Skill s in ReqSkills)
        {
            i += 1;
            i += s.GetDependencyCount();            
        }
        return i;
    }
}
