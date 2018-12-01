using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.9.1
public class IngameLog : MonoBehaviour {

	public static Transform ThisLog;

	public void Awake()
	{
		ThisLog = GameObject.FindGameObjectWithTag("Log").transform;
	}

    //Instantiate text at the bottom left of the screen
	public static void Log(string message, Color messageColour)
	{
        GameObject temp = FadingTMPro.InstFadingText(2.5f, ThisLog.gameObject);
        temp.transform.SetAsLastSibling();
        temp.GetComponent<TMPro.TextMeshProUGUI>().text = message;
		temp.GetComponent<TMPro.TextMeshProUGUI>().color = messageColour;
	}
}
