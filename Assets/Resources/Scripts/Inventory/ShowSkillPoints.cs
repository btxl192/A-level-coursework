using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSkillPoints : MonoBehaviour {

	private PlayerStats PStats;

	void Start () 
	{
		PStats = PlayerSave.staticplayer.GetComponent<PlayerStats>();
        this.GetComponent<HoverText>().SetText("Leveling up grants you skill points. Use skill points to upgrade skills!");
	}
	
	void Update () 
	{
		this.GetComponent<UnityEngine.UI.Text>().text = "Skill Points: " + PStats.skillpoints.ToString();
	}
}
