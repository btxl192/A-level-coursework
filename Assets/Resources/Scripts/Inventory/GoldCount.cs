using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCount : MonoBehaviour {
	
	private PlayerInventory player;

	void Start()
	{
		player = PlayerSave.staticplayer.GetComponent<PlayerInventory>();
	}

    //Show the amount of gold
	void Update () 
	{
		this.gameObject.GetComponent<UnityEngine.UI.Text>().text = player.Gold.ToString() + " Gold";
	}
}
