using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericMenu2 : MonoBehaviour {

    [SerializeField]
    protected string TypeOfMenu;

    public static GameObject OpenMenu { get; protected set; }

	public delegate void MenuEvent(GameObject menu);
    public delegate void BlankEvent();
	public static event BlankEvent pauser;
	public static event BlankEvent unpauser;
	public static event MenuEvent OnOpen;
	public static event MenuEvent OnClose;

	protected virtual void Start () 
	{
		this.gameObject.GetComponent<Canvas>().enabled = false; 
	}

    //Check if the player pressed the menu key eveyr frame
	protected virtual void Update () 
	{
        //Open the menu
        //Objective 1.3.2.10.1, 1.3.2.10.2
		if (((Input.inputString.ToUpper () == KeyBindings.KeyBinds[TypeOfMenu].ToUpper () || (Input.GetKeyDown (KeyCode.Escape) && KeyBindings.KeyBinds[TypeOfMenu].ToUpper () == "ESC")) && this.gameObject.GetComponent<Canvas> ().enabled == false && OpenMenu == null)) //if the key is pressed and the menu is closed, open the menu and pause the game
        {
			OpenMenu = this.gameObject;
			this.gameObject.GetComponent<Canvas> ().enabled = true;
            this.GetComponentsInChildren<RectTransform>(true)[1].gameObject.SetActive(true);
            RunPauser ();
            if (OnOpen != null)
            {
                OnOpen(this.gameObject);
            }
                				
		} 
        //Close the menu
		else if ((Input.inputString.ToUpper () == KeyBindings.KeyBinds[TypeOfMenu].ToUpper () || (Input.GetKeyDown (KeyCode.Escape))) && this.gameObject.GetComponent<Canvas>().enabled == true && OpenMenu == this.gameObject) //if the key is pressed and the menu is open, close the menu and resume the game
        {
            OpenMenu = null;
            this.GetComponentsInChildren<RectTransform>(true)[1].gameObject.SetActive(false);
            this.gameObject.GetComponent<Canvas> ().enabled = false;
			RunUnpauser ();
            if (OnClose != null)
            {
                OnClose(this.gameObject);
            }
		} 
	}

    //Pause the game
	protected void RunPauser()
	{
		pauser();
	}

    //Resume the game
	protected void RunUnpauser()
	{
		unpauser();
	}

    //Sets OpenMenu
    public static void SetOpenMenu(GameObject Menu)
    {
        OpenMenu = Menu;
    }

    public string GetTypeOfMenu()
    {
        return TypeOfMenu;
    }
}
