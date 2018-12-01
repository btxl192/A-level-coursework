using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InvSlot : MonoBehaviour {

    [SerializeField]
    private int _invslot; 
    private GameObject _item; 
	private static GameObject[] invslots = new GameObject[10];

	public delegate void DisplayDetailsEvent(GameObject i, int j);
	public static event DisplayDetailsEvent DisplayDetails;
	
	private Button thisbutton;
    private float PreferredWidth;
    private GameObject itemtypeicon;

	protected void Start () 
    {
        PlayerInventory.updatei += RunSlotUpdate;
		PageCount.UpdateInv += RunSlotUpdate;
		EquipButton.UpdateInv += RunSlotUpdate;
		EquipButton.SendSlot += Selected;
		SortInvButton.UpdateInv += RunSlotUpdate;
        GenericMenu2.OnOpen += UpdateSlot;
		thisbutton = this.gameObject.GetComponent<Button>();
		thisbutton.onClick.AddListener (details);        
        invslots[_invslot] = this.gameObject;
        itemtypeicon = transform.Find("ItemTypeIcon").gameObject;
        RunSlotUpdate();
    }

    private void OnDestroy()
    {
        PlayerInventory.updatei -= RunSlotUpdate;
        PageCount.UpdateInv -= RunSlotUpdate;
        EquipButton.UpdateInv -= RunSlotUpdate;
        EquipButton.SendSlot -= Selected;
        SortInvButton.UpdateInv -= RunSlotUpdate;
        GenericMenu2.OnOpen -= UpdateSlot;
    }

    //Update the slot with an item according to its index in the player's inventory 
    //Objective 1.3.2.5.1
    void RunSlotUpdate()
    {
        this.gameObject.GetComponent<Image>().color = Color.white;
        if (itemtypeicon == null)
        {
            itemtypeicon = transform.Find("ItemTypeIcon").gameObject;
        }
        try
        {
            _item = PlayerSave.staticplayer.GetComponent<PlayerInventory>().inventory[_invslot + (PageCount.PageNum - 1) * 10];
            thisbutton.GetComponentInChildren<TextMeshProUGUI>().text = _item.GetComponent<GenericItem>().itemname;
            thisbutton.GetComponentInChildren<TextMeshProUGUI>().color = GenericItem.GetRarityColour(_item.GetComponent<GenericItem>().Rarity);
            itemtypeicon.GetComponent<Image>().sprite = GenericItem.ItemTypeIcon[_item.GetComponent<GenericItem>().category];
            
        }
        catch
        {
            _item = null;
            thisbutton.GetComponentInChildren<TextMeshProUGUI>().text = "";
            itemtypeicon.GetComponent<Image>().sprite = Resources.Load<Sprite>(FileDir.BlankIcon);
        }
    }

    //Update the slot if the GameObject passed is the inventory
    public void UpdateSlot(GameObject g)
    {
        if (g.GetComponent<GenericMenu2>().GetTypeOfMenu() == "Inventory")
        {
            RunSlotUpdate();
        }
    }

    //Display the item's details
    //Objective 1.3.2.5.2
	void details()
	{
		try
		{
			foreach(GameObject slot in invslots)
			{
				slot.GetComponent<Image>().color = Color.white;
			}
			this.gameObject.GetComponent<Image>().color = Color.yellow;
			DisplayDetails(null,-1);
			DisplayDetails (_item, _invslot + (PageCount.PageNum - 1) * 10);
		}
		catch
		{
			Debug.Log("ERROR: couldn't display _item details");
		}
	}

	void Selected(int i)
	{
		if (_invslot == i)
		{
			this.gameObject.GetComponent<Image>().color = Color.yellow;
		}
	}

	void Clear()
	{
		this.gameObject.GetComponent<Image>().color = Color.white;
	}
}
