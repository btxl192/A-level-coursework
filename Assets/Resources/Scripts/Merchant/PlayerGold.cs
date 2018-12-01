using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGold : MonoBehaviour {

	void Update () 
	{
		this.gameObject.GetComponent<UnityEngine.UI.Text>().text = "Gold: " + PlayerSave.staticplayer.GetComponent<PlayerInventory>().Gold.ToString();
	}
}
