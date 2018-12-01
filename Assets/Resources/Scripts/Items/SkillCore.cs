using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillCore : GenericItem {

    [SerializeField, HideInInspector]
    protected List<int> _SkillIDs = new List<int>();
    public List<int> SkillIDs
    {
        get
        {
            return _SkillIDs;
        }
    }

    [SerializeField, HideInInspector]
    protected List<int> _SkillLevels = new List<int>();
    public List<int> SkillLevels
    {
        get
        {
            return _SkillLevels;
        }
    }

    public override string FormattedDesc
    {
        get
        {
            string d = base.FormattedDesc;
            d += "\n\nSkills Upgraded: ";
            foreach (Skill s in Skills)
            {
                d += "\n+" + s.Level + " " + s.SkillName + " skill";
            }
            return d;
        }
    }

    [SerializeField]
    protected List<Skill> _Skills = new List<Skill>();
    public List<Skill> Skills
    {
        get
        {
            UpdateSkills();
            return _Skills;
        }
    }

    public SkillCore()
    {
        SkillManager.UpdateSkillCores += UpdateSkills;
    }

    public void ClearSkills()
    {
        _Skills.Clear();
    }

    //Update the skills in the skill core
    public void UpdateSkills()
    {
        ClearSkills();
        for (int a = 0; a < SkillIDs.Count; a++)
        {
            Skill temp;
            temp = SkillManager.SkillDict(SkillIDs[a]).Clone();
            temp.SetLevel(SkillLevels[a]);
            _Skills.Add(temp);
        }
    }

    public override string GetSaveData()
    {
        string temp = base.GetSaveData();
        foreach (Skill s in Skills)
        {
            temp += string.Format(",{0}/{1}/{2}", SkillManager.GetSkillID(s), s.CurrentLevel, s.MaxLevel);
        }
        return temp;
    }

    public void SetSkills(List<Skill> SkillsList)
    {
        _Skills = SkillsList;
    }
}
