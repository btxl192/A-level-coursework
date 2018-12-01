using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPageButton : MonoBehaviour {

	private Button thisbutton;

	protected void Start () 
	{
		thisbutton = this.gameObject.GetComponent<Button> ();
		thisbutton.onClick.AddListener (RunEvent);
	}

	void RunEvent()
	{
		NextPage ();
	}

	public delegate void NextPageEvent();
	public static event NextPageEvent NextPage;
}
