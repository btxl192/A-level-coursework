using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSkillButtons : MonoBehaviour {

    List<List<Skill>> NumDep = new List<List<Skill>>(); //List used to organise skills by their dependencies; skills in index 0 have 0 dependencies, etc
    List<GameObject> AllButtons = new List<GameObject>();

	void Awake () 
	{
		SkillManager.RunAddSkillButton += UpdateSkills;
        GenericMenu2.OnClose += DestroyButtons;
        NumDep.Add(new List<Skill>());
    }
	
    public void UpdateSkills(List<Skill> Skills)
    {
        DestroyButtons(null);
        ClearButtons();
        //initialises NumDep
        foreach (Skill s in Skills)
        {
            if (s.GetDependencyCount() > 0)
            {
                while ((NumDep.Count - 1) < s.GetDependencyCount())
                {
                    NumDep.Add(new List<Skill>());
                }
                NumDep[s.GetDependencyCount()].Add(s);
            }
            else
            {
                NumDep[0].Add(s);
            }
        }
        //List used to store the indices of buttons that need to be checked; only buttons with dependencies need to be checked
        List<int> ListButtonsToCheck = new List<int>();
        foreach (List<Skill> g in NumDep)
        {
            //List used to update ListButtonsToCheck
            List<int> NewButtonsToCheck = new List<int>();
            foreach (Skill s in g)
            {                
                //Creates the row of skills with no dependencies first
                if (s.GetDependencyCount() == 0)
                {
                    GameObject temp = Instantiate(Resources.Load(FileDir.SkillButton) as GameObject, Instantiate(Resources.Load(FileDir.SkillRow) as GameObject, transform).transform);
                    temp.GetComponent<SkillButton>().SkillID = SkillManager.GetSkillID(s);
                    AllButtons.Add(temp);
                    ListButtonsToCheck.Add(AllButtons.Count-1);
                }
                else
                {                   
                    foreach (int button in ListButtonsToCheck)
                    {                       
                        GameObject actualbutton = AllButtons[button];
                        if (s.DependentOn(SkillManager.SkillDict(actualbutton.GetComponent<SkillButton>().SkillID)))
                        {
                            if (actualbutton.transform.Find("SkillRow") == null)
                            {
                                GameObject temprow = Instantiate(Resources.Load(FileDir.SkillRow) as GameObject, actualbutton.transform);
                                temprow.name = "SkillRow";
                                temprow.transform.localPosition = new Vector3(0f, -(temprow.transform.parent.GetComponent<RectTransform>().sizeDelta.y * 1.5f), 0f);
                            }
                            GameObject temp = Instantiate(Resources.Load(FileDir.SkillButton) as GameObject, actualbutton.transform.Find("SkillRow").transform);
                            temp.GetComponent<SkillButton>().SkillID = SkillManager.GetSkillID(s);
                            temp.transform.Find("Dependency").gameObject.SetActive(true);
                            temp.GetComponent<SkillButton>().UpdateText();
                            AllButtons.Add(temp);
                            //Each time a skill with a dependency is found, add it to NewButtonsToCheck
                            NewButtonsToCheck.Add(AllButtons.Count-1);
                        }
                    }
                }
            }
            if (NewButtonsToCheck.Count > 0)
            {
                ListButtonsToCheck.Clear();
                ListButtonsToCheck.AddRange(NewButtonsToCheck);
            }
        }
    }

    void ClearButtons()
    {
        NumDep.Clear();
        NumDep.Add(new List<Skill>());
        AllButtons.Clear();
    }

    void DestroyButtons(GameObject go)
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject != gameObject)
            {
                Destroy(t.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        SkillManager.RunAddSkillButton -= UpdateSkills;
        GenericMenu2.OnClose -= DestroyButtons;
    }
}
