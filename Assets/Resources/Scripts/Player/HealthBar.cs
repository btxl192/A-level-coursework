using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Objective 1.3.2.7.3
public class HealthBar : MonoBehaviour {

    [SerializeField]
	private Slider healthbar;
    [SerializeField]
	private Text healthnum;

	void Start () 
	{
		healthbar = this.gameObject.GetComponent<Slider>();
		healthnum = this.gameObject.GetComponentInChildren<Text> ();
		healthbar.maxValue = PlayerSave.staticplayer.GetComponent<PlayerStats>().HP.maxhealth;
        PlayerHealth.HPChanged += UpdateHealth;
        PlayerStats.CheckStats += UpdateHealth;
        UpdateHealth();
	}

    //Update the health bar to show the player's current health
    void UpdateHealth()
    {
        healthbar.value = PlayerSave.staticplayer.GetComponent<PlayerStats>().HP.health;
        healthbar.maxValue = PlayerSave.staticplayer.GetComponent<PlayerStats>().HP.maxhealth;
        healthnum.text = PlayerSave.staticplayer.GetComponent<PlayerStats>().HP.health + "/" + PlayerSave.staticplayer.GetComponent<PlayerStats>().HP.maxhealth;
    }

    private void OnDestroy()
    {
        PlayerHealth.HPChanged -= UpdateHealth;
        PlayerStats.CheckStats -= UpdateHealth;
    }
}
