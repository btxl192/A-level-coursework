using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Objective 1.3.2.7.4
public class ManaBar : MonoBehaviour {

	private Slider manabar;
	private Text mananum;
	private PlayerStats player;

	void Start () 
	{
		player = PlayerSave.staticplayer.GetComponent<PlayerStats>();
		manabar = this.gameObject.GetComponent<Slider>();
		mananum = this.gameObject.GetComponentInChildren<Text> ();
		manabar.maxValue = player.MP.maxmana;
        PlayerMana.MPChanged += UpdateMana;
        PlayerStats.CheckStats += UpdateMana;
        UpdateMana();
	}

    //Update the mana bar with the player's current mana values
    void UpdateMana()
    {
        manabar.value = player.MP.mana;
        manabar.maxValue = player.MP.maxmana;
        mananum.text = player.MP.mana + "/" + player.MP.maxmana;
    }

    private void OnDestroy()
    {
        PlayerMana.MPChanged -= UpdateMana;
        PlayerStats.CheckStats -= UpdateMana;
    }
}
