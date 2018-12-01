using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Objective 1.3.2.7.8.e
public class SkillButton : MonoBehaviour, IPointerClickHandler {

	public int SkillID { get; set; }
	public bool isActiveSkill { get; set; }
	public delegate void IncreaseSkillEvent(bool active, int id, int i);
	public static event IncreaseSkillEvent IncreaseSkill;
    float[] bounds = new float[4];
    private Canvas SkillsCanvas;

    private Text SkillType; 

	void Awake () 
	{		
		this.GetComponent<RectTransform>().sizeDelta = new Vector2 (Screen.width*0.15f, Screen.height*0.1f);
		SkillManager.UpdateButtonText+=UpdateText;
		PlayerStats.CheckStats += UpdateText;
		UpdateText();
        SkillsCanvas = transform.root.Find("Menus/InvCanvas/bg/SkillsCanvas").gameObject.GetComponent<Canvas>();

    }

    void OnDestroy()
    {
        SkillManager.UpdateButtonText -= UpdateText;
        PlayerStats.CheckStats -= UpdateText;
    }

    //Checks for a right click or a left click
    public void OnPointerClick(PointerEventData eventData)
    {
        //Try to upgrade skill skill on a left click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            IncrSkill();
        }
        //Check for key presses on a right click
        else if (eventData.button == PointerEventData.InputButton.Right && isActiveSkill)
        {
            CheckForKey();
        }
    }

    //Shows the hovertext if the mouse is hovering over the skill
    void Update()
    {
        if (SkillsCanvas.isActiveAndEnabled)
        {
            bounds[0] = gameObject.transform.position.x + gameObject.GetComponent<RectTransform>().sizeDelta.x;
            bounds[1] = gameObject.transform.position.x - gameObject.GetComponent<RectTransform>().sizeDelta.x;
            bounds[2] = gameObject.transform.position.y;
            bounds[3] = gameObject.transform.position.y - gameObject.GetComponent<RectTransform>().sizeDelta.y * 2;
            if (Input.mousePosition.x > bounds[1] && Input.mousePosition.x < bounds[0] && Input.mousePosition.y > bounds[3] && Input.mousePosition.y < bounds[2])
            {
                GetComponent<HoverText>().MouseOver();
            }
            else
            {
                GetComponent<HoverText>().MouseExit(null);
            }
        }

    }

    //Upgrades the skill
    void IncrSkill()
	{
		if(PlayerSave.staticplayer.GetComponent<PlayerStats>().skillpoints > 0)
		{
            if (SkillManager.Skills[SkillID].CurrentLevel < SkillManager.Skills[SkillID].MaxLevel && PlayerSave.staticplayer.GetComponent<PlayerStats>().level >= SkillManager.Skills[SkillID].ReqLevel)
            {
                IncreaseSkill(isActiveSkill, SkillID, 1);                
            }
		}
		else
		{
			IngameLog.Log("Not enough skillpoints!", Color.red);
		}
        UpdateText();
	}

    //Updates the button's text and the HoverText
	public void UpdateText()
	{

        UpdateActiveSkill();
        Skill Skill;
        Skill = SkillManager.Skills[SkillID];
        SkillType = transform.GetChild(1).GetComponent<Text>();
        if (Skill.SkillType == Skill.SkillTypes.Active)
        {
            SkillType.text = "Active";
            SkillType.color = Color.red;
        }
        else if (Skill.SkillType == Skill.SkillTypes.Passive)
        {
            SkillType.text = "Passive";
            SkillType.color = Color.blue;
        }
        else if (Skill.SkillType == Skill.SkillTypes.Buff)
        {
            SkillType.text = "Buff";
            SkillType.color = Color.green;
        }
        if (PlayerSave.staticplayer.GetComponent<PlayerStats>().level < Skill.ReqLevel || !Skill.ReqSkillsMet())
        {
            GetComponentInChildren<Text>().color = Color.gray;
            transform.Find("Dependency").GetComponent<Image>().color = Color.red;
        }
        else
        {
            GetComponentInChildren<Text>().color = Color.black;
            transform.Find("Dependency").GetComponent<Image>().color = Color.green;
        }
        this.GetComponentInChildren<Text>().text = Skill.SkillName;
        this.GetComponentInChildren<Text>().text += "\n" + Skill.SkillDesc;
        this.gameObject.GetComponentInChildren<Text>().text = Skill.SkillName + " (" + Skill.CurrentLevel;
        if (Skill.OverLevel > 0)
        {
            this.gameObject.GetComponentInChildren<Text>().text += "+" + Skill.OverLevel;
        }
        this.gameObject.GetComponentInChildren<Text>().text += "/" + Skill.MaxLevel + ")";
        string hovertexttext = "";
        try
        {
            if (Skill.Level > 0)
            {
                hovertexttext = ("[Level " + Skill.Level + "]");
                hovertexttext += FormatSkillDesc(Skill, Skill.Level);
            }
            else
            {
                if (Skill.ReqLevel > 0)
                {
                    hovertexttext += "{REQUIRES LEVEL " + Skill.ReqLevel + "}\n";
                }
                if (Skill.ReqSkills.Count > 0)
                {
                    hovertexttext += "\nRequired skills: " + Skill.ReqSkills[0].SkillName + ": level " + Skill.ReqSkills[0].CurrentLevel;
                    if (Skill.ReqSkills.Count > 1)
                    {
                        for (int a = 1; a < Skill.ReqSkills.Count; a++)
                        {
                            hovertexttext += string.Format(", {0}: level {1}", Skill.ReqSkills[a].SkillName, Skill.ReqSkills[a].CurrentLevel);
                        }
                    }
                    hovertexttext += "\n";
                }
            }
            if (Skill.Level < Skill.MaxLevel)
            {
                hovertexttext += ("\nNext level: [Level " + (Skill.Level + 1) + "]");
                hovertexttext += FormatSkillDesc(Skill, Skill.Level + 1);
            }
            GetComponentInChildren<HoverText>().SetText(hovertexttext);
        }
        catch
        {
            Debug.Log("SKILL BUTTON COULD NOT BE LOADED");
        }

    }

    //Returns a formatted version of the skill's description
    private string FormatSkillDesc(Skill Skill, int level)
    {
        string s = "";
        if (Skill.SkillType == Skill.SkillTypes.Active || Skill.SkillType == Skill.SkillTypes.Buff)
        {
            string keybind = KeyBindings.KeyBinds[Skill.SkillName];
            if (keybind == "")
            {
                keybind = "Right click this button to assign a key to this skill";
            }
            s += " \n(MP cost: " + ((ActiveSkill)Skill).ManaCost + ")" + " [" + keybind + "]";
            if (((ActiveSkill)Skill).Cooldown > 0)
            {
                s += string.Format("\n(Cooldown: {0} seconds)", ((ActiveSkill)Skill).Cooldown);
            }
        }
        s += "\n" + Skill.GetSkillDescription(level);
        return s;
    }

	void UpdateActiveSkill()
	{
		isActiveSkill = (SkillManager.SkillDict(SkillID).SkillType == Skill.SkillTypes.Active) || (SkillManager.SkillDict(SkillID).SkillType == Skill.SkillTypes.Buff);
    }

	public void Destroy()
	{
		SkillManager.UpdateButtonText -= UpdateText;
		PlayerStats.CheckStats -= UpdateText;
		Destroy(this.gameObject);
	}

    void CheckForKey()
    {
        if (SkillManager.Skills[SkillID].Level > 0)
        {
			GetKeyPress.InstantiateKeyPress(GetKeyPress.Receivers.SkillButton);
			GetKeyPress.SendKey += GetKey;
        }
        else
        {
            TempPopup.Show("Cannot assign keybind to a skill that is level 0!", Color.red);
        }
    }

    //Receive a key press from GetKeyPress
    void GetKey(string key, GetKeyPress.Receivers r)
    {
        GetKeyPress.Receivers thisReceiver = GetKeyPress.Receivers.SkillButton;
        GetKeyPress.SendKey -= GetKey;
        if (r == thisReceiver)
        {
            if (!KeyBindings.IsKeyBound(key))
            {
                KeyBindings.KeyBinds[SkillManager.SkillDict(SkillID).SkillName] = key;               
                UpdateText();
                ActiveSkillManager.UpdateKeys();
            }
            else
            {
                TempPopup.Show(key + " Key is already bound!", Color.red);
            }
        }
    }
}
