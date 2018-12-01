using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour {

	private Button thisbutton;
	private int EquipmentLoc;

    public delegate void SendIntEvent(int i);
    public static event SendIntEvent ChangeEq;
    public static event SendIntEvent UseItem;
    public static event SendIntEvent SendSlot;

    public delegate void BlankEvent();
    public static event BlankEvent UpdateInv;

    public delegate void UpdateDetailsEvent(GameObject e, int i);
    public static event UpdateDetailsEvent UpdateDetails;

    protected void Start()
	{
		thisbutton = this.gameObject.GetComponent<Button> ();
		ItemDetails.PassEq += GetEqLoc;
		ItemDetails.ChangeText += ChangeText;
		EquipmentLoc = -1;
		thisbutton.onClick.AddListener (ButtonIsClicked);
	}

    private void OnDestroy()
    {
        ItemDetails.PassEq -= GetEqLoc;
        ItemDetails.ChangeText -= ChangeText;
    }

    void GetEqLoc(int i)
	{
		EquipmentLoc = i;
	}

    //When the button is clicked, use the selected item if it is a usable item, and equip the item if it is equpiment
	void ButtonIsClicked()
	{
		try
		{
			GameObject currentitem = PlayerSave.staticplayer.GetComponent<PlayerInventory>().inventory[EquipmentLoc];
			if(currentitem.GetComponent<GenericWeapon>() != null || currentitem.GetComponent<GenericArmour>() != null || currentitem.GetComponent<SkillCore>() != null)
			{
                //Objective 1.3.2.5.3
				ChangeEq (EquipmentLoc);
				UpdateInv ();
			}
			else if(currentitem.GetComponent<UsableItem>() != null)
			{
                //Objective 1.3.2.5.4
				UseItem(EquipmentLoc);
				UpdateInv ();
			}
			try
			{
				UpdateDetails (PlayerSave.staticplayer.GetComponent<PlayerInventory>().inventory[EquipmentLoc], EquipmentLoc);
			}
			catch
			{
				UpdateDetails (null, -1);
			}
			SendSlot(EquipmentLoc % 10);
		}
		catch
		{
			this.gameObject.GetComponentInChildren<Text> ().text = "";
			this.gameObject.GetComponent<Button>().enabled = false;
			this.gameObject.GetComponent<Image>().enabled = false;
            UpdateDetails(null, -1);
        }
	}

    //Change the text on the button
	void ChangeText(string t)
	{
		this.gameObject.GetComponentInChildren<Text> ().text = t;
		if(t == "")
		{
			this.gameObject.GetComponent<Button>().enabled = false;
			this.gameObject.GetComponent<Image>().enabled = false;
		}
		else
		{
			this.gameObject.GetComponent<Button>().enabled = true;
			this.gameObject.GetComponent<Image>().enabled = true;
		}
	}
}
