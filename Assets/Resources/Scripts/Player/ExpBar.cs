using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//Objective 1.3.2.7.5
public class ExpBar : MonoBehaviour {

	private Slider expbar;
	private Text expnum;
	private PlayerStats player;

	public delegate void increaselevelevent();
	public static event increaselevelevent increaselevel;

    private void Awake()
    {
        PlayerStats.UpdateExp += UpdateText;
        PlayerStats.addexp += addexpPROC;
    }

    private void Start () 
	{
		player = PlayerSave.staticplayer.GetComponent<PlayerStats>();
		expbar = this.gameObject.GetComponent<Slider>();
		expnum = this.gameObject.GetComponentInChildren<Text> ();
		expbar.maxValue = player.maxexp;		
        UpdateText();
	}

    //Starts the addexp coroutine
    private void addexpPROC(int exp)
	{
		StartCoroutine (addexp (exp));
	}

    //Increment the exp bar's value gradually
	IEnumerator addexp(int exp)
	{
        expbar.maxValue = player.maxexp;
        for (int a = 0; a < exp; a++)
		{
			expbar.value+=1;           
            yield return new WaitForSeconds (1/exp);
            if (expbar.value >= expbar.maxValue)
            {
                expbar.value = 0f;
                player.increaselevel();
                expbar.maxValue = player.maxexp;
                Instantiate(Resources.Load(FileDir.LevelupParticles) as GameObject, player.gameObject.transform);
                UpdateText();
            }
            UpdateText();
        }
	}

    //Update the text value on the exp bar
    public void UpdateText()
    {
        expbar.value = player.exp;
        expbar.maxValue = player.maxexp;
        if (expnum != null)
        {
            expnum.text = player.exp + "/" + player.maxexp + "  Lv. " + player.level;
        }       
    }

    private void OnDestroy()
    {
        PlayerStats.UpdateExp -= UpdateText;
        PlayerStats.addexp -= addexpPROC;
    }
}