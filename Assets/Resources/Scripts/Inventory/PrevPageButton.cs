using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrevPageButton : MonoBehaviour {

	private Button thisbutton;

	protected void Start () 
	{
		thisbutton = this.gameObject.GetComponent<Button> ();
		thisbutton.onClick.AddListener (RunEvent);
	}

	void RunEvent()
	{
		PrevPage ();
	}

	public delegate void PrevPageEvent();
	public static event PrevPageEvent PrevPage;
}
