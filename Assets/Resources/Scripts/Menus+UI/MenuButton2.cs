using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuButton2 : MonoBehaviour {

    [SerializeField]
	private Canvas ToOpen;
    [SerializeField]
    private bool StayOpen;

	private GameObject bg;
	private Canvas[] siblings;

	void Start () 
	{
		if(this.gameObject.GetComponent<Button>() != null)
		{
			this.gameObject.GetComponent<Button>().onClick.AddListener (OpenMenu);
		}
		bg = this.gameObject.transform.parent.gameObject;
		siblings = bg.GetComponentsInChildren<Canvas> ();
	}

    //Open the menu when the button is clicked
	void OpenMenu()
	{
        if (ToOpen != null)
        {
            //Disable other menus
            foreach (Canvas item in siblings)
            {
                if (item != this.gameObject.GetComponentInParent<Canvas>())
                {
                    item.enabled = false;
                }
            }
            //Disable other UI elements not on the menu to open
            foreach (Transform child in transform.parent)
            {
                if (child.GetComponent<Button>() != null && child.gameObject != this.gameObject)
                {
                    try
                    {
                        if (!child.GetComponent<MenuButton2>().StayOpen)
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                    catch
                    {
                        child.gameObject.SetActive(false);
                    }
                }
                else if (child.gameObject != this.gameObject && child.name != "bg")
                {
                    child.gameObject.SetActive(false);
                }
            }
            //Enables the menu to open
            ToOpen.gameObject.SetActive(true);
            ToOpen.enabled = true;
            foreach (RectTransform g in ToOpen.gameObject.GetComponentsInChildren<RectTransform>(true))
            {
                try
                {
                    if (g.transform.parent.gameObject == ToOpen.gameObject && g.GetComponent<Canvas>() == null)
                    {
                        g.gameObject.SetActive(true);
                    }
                }
                catch
                {
                    if (null == ToOpen.gameObject && g.GetComponent<Canvas>() == null)
                    {
                        g.gameObject.SetActive(true);
                    }
                }

            }
            if (!StayOpen)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

}
