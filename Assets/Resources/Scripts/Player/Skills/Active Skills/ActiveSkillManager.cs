using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillManager : MonoBehaviour {

    static List<string> ActiveSkillNames = new List<string>();
    static List<string> BoundKeys = new List<string>();
    [SerializeField]
    private static bool Initialised;

    //stores the time an active skill is used
    public static Dictionary<int, float> PlayerSkillCooldowns = new Dictionary<int, float>();

    private void Start () 
	{
        if (!Initialised)
        {
            for (int a = 0; a < SkillManager.SkillDictCount; a++)
            {
                if (SkillManager.SkillDict(a).SkillType == Skill.SkillTypes.Active || SkillManager.SkillDict(a).SkillType == Skill.SkillTypes.Buff)
                {
                    KeyBindings.AddAction(SkillManager.SkillDict(a).SkillName);
                    ActiveSkillNames.Add(SkillManager.SkillDict(a).SkillName);
                }
            }
        }
        Initialised = true;
	}

    //Updates the key bindings of the active skills
    public static void UpdateKeys()
    {
        BoundKeys = new List<string>();
        foreach (string action in KeyBindings.KeyBinds.Keys)
        {
            if (ActiveSkillNames.Contains(action))
            {
                BoundKeys.Add(KeyBindings.KeyBinds[action]);
            }
        }
    }

    //Checks if a key bound to an active skill is pressed. Activate the skill if the key bound to it is pressed
	void Update()
    {
        foreach(string keypress in BoundKeys)
        {
            if (keypress != "")
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), keypress)))
                {
                    foreach (Skill s in SkillManager.Skills.Values)
                    {
                        try
                        {
                            if (s.SkillType == Skill.SkillTypes.Active || s.SkillType == Skill.SkillTypes.Buff)
                            {
                                if (keypress == KeyBindings.KeyBinds[s.SkillName] && s.Level > 0 && ((ActiveSkill)s).HoldKey == false)
                                {
                                    ((ActiveSkill)s).Activate();
                                }
                            }
                        }
                        catch
                        {
                            Debug.Log("couldn't cast skill: " + s.SkillName);
                            throw;
                        }
                    }
                }
                else if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), keypress)))
                {
                    foreach (Skill s in SkillManager.Skills.Values)
                    {
                        try
                        {
                            if (s.SkillType == Skill.SkillTypes.Active)
                            {
                                if (keypress == KeyBindings.KeyBinds[s.SkillName] && s.Level > 0 && ((ActiveSkill)s).HoldKey == true)
                                {
                                    ((ActiveSkill)s).Activate();
                                }
                            }
                        }
                        catch
                        {
                            Debug.Log("couldn't cast skill: " + s.SkillName);
                            throw;
                        }
                    }
                }
            }
        }

    }

}
