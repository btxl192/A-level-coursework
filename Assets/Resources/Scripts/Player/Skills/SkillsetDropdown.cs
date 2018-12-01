using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsetDropdown : MonoBehaviour {

	[SerializeField]
	private Dropdown thisdropdown;
	public Skill.SkillSets SkillSet {get; private set; }

	public delegate void UpdateSkillsEvent(Skill.SkillSets SkillSet);
	public static event UpdateSkillsEvent UpdateSkills;

	void Awake () 
	{
		thisdropdown = this.gameObject.GetComponent<Dropdown>();
		SkillManager.Ready += GetSkillSet;
        UpdateSkillSets();
        GenericMenu2.OnOpen += UpdatePlayerSkills;
        SkillManager.SkillSetsChanged += UpdateSkillSets;
	}

    private void OnDestroy()
    {
        GenericMenu2.OnOpen -= UpdatePlayerSkills;
        SkillManager.Ready -= GetSkillSet;
        SkillManager.SkillSetsChanged -= UpdateSkillSets;
    }

    //Get the current skillset
    public void GetSkillSet()
	{
        SkillSet = (Skill.SkillSets)System.Enum.Parse(typeof(Skill.SkillSets), thisdropdown.transform.Find("Label").GetComponent<Text>().text);
		UpdateSkills(SkillSet);
	}

    //Adds options to the dropdown menu based on which skillsets the player has available
    private void UpdateSkillSets()
    {
        thisdropdown.ClearOptions();
        List<string> skillsetnames = new List<string>();
        foreach (Skill.SkillSets s in SkillManager.PlayerSkillSets)
        {
            skillsetnames.Add(s.ToString());
        }
        thisdropdown.AddOptions(skillsetnames);
    }

    //Update the skills UI with which skillset the dropdown menu is on
    private void UpdatePlayerSkills(GameObject g)
    {
        if (g.GetComponent<GenericMenu2>().GetTypeOfMenu() == "Inventory")
        {
            UpdateSkills(SkillSet);
        }       
    }

    void Enable()
    {
        for (int a = 2; a < this.GetComponentsInChildren<RectTransform>(true).Length; a++)
        {
            this.GetComponentsInChildren<RectTransform>(true)[a].gameObject.SetActive(true);
        }
    }

    void Disable()
    {
        for (int a = 2; a < this.GetComponentsInChildren<RectTransform>(true).Length; a++)
        {
            this.GetComponentsInChildren<RectTransform>(true)[a].gameObject.SetActive(false);
        }
    }
}
