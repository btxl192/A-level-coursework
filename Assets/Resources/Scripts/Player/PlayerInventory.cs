using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInventory : MonoBehaviour {

    [SerializeField]
    private List<GameObject> _inventory = new List<GameObject>();
    public List<GameObject> inventory { get { return _inventory;  } }
    [SerializeField]
	private int maxitems;
	public int Gold { get; private set; }

	private PlayerEquipment Eq;

	public delegate void ChangedEQevent();
	public static event ChangedEQevent ChangedEQ;

    public delegate void BlankEvent();
    public static event BlankEvent updatei;

	void Start () {
		EquipButton.ChangeEq += ChangeEquipment;
		EquipButton.UseItem += UseItem;
		Eq = this.gameObject.GetComponent<PlayerEquipment>();
        StackableItem.CheckItem += DeleteStackableItems;
    }

    private void OnDestroy()
    {
        EquipButton.ChangeEq -= ChangeEquipment;
        EquipButton.UseItem -= UseItem;
        StackableItem.CheckItem -= DeleteStackableItems;
    }

    //Adds gold and log the amount in the ingame log
    public void AddGold(int i)
    {
        Gold += i;
        if (i > 0)
        {
            IngameLog.Log(string.Format("Added {0} gold", i), new Color(1, 0.549f, 0));
        }
        else
        {
            IngameLog.Log(string.Format("Removed {0} gold", i), new Color(1, 0.549f, 0));
        }
    }

    public void SetGold(int i)
    {
        Gold = i;
    }

    //Add an item to the player's inventory and log it in the ingame log
    public void additem(GameObject i, bool ToLog)
    {
        i.transform.SetParent(PlayerSave.staticplayer.transform);
        if (inventory.Count < maxitems)
        {
            //Check if the item is stackable or not
            if (CheckStackableItem(i) != -1)
            {
                _inventory[CheckStackableItem(i)].GetComponent<StackableItem>().ChangeAmount(i.GetComponent<StackableItem>().Amount);
            }
            else
            {
                _inventory.Add(i);
            }
            if (ToLog)
            {
                string message;
                message = "Added " + i.GetComponent<GenericItem>().itemname;
                if (i.GetComponent<StackableItem>() != null)
                {
                    message += " (" + i.GetComponent<StackableItem>().Amount + ") ";
                }
                message += " to inventory";
                IngameLog.Log(message, Color.white);
            }
            if (updatei != null)
            {
                updatei();
            }
        }

		
    }

    public void removeat(int i)
    {
        _inventory.RemoveAt(i);
        updatei();
    }

    //Check if the player's inventory is full
    public bool isinvfull()
    {
        return _inventory.Count == maxitems;
    }

    //Check if an item exists in the player's inventory
	public int CheckItem(GameObject i)
	{
		foreach (GameObject item in _inventory)
		{
			if (item.GetComponent<GenericItem> ().itemname == i.GetComponent<GenericItem> ().itemname)
			{
				return _inventory.IndexOf(item);
			} 
		}
		return -1;
	}

    //Unequip the equipment of the type specified
    public void UnEquip(System.Type T)
    {
        GameObject PrevEq;
        Eq.UnEquip(T, out PrevEq);
        additem(PrevEq, false);
        ChangedEQ();
    }

    //Equip or swap out a piece of equipment
	public void ChangeEquipment(int i)
	{
        try
        {
            GameObject PrevEq;
			Eq.Equip(_inventory[i], out PrevEq);
			_inventory[i] = PrevEq;
            if (_inventory[i] == null)
            {
                _inventory.RemoveAt(i);
            }
            ChangedEQ();
        }
		catch
		{
            Debug.Log("ERROR: Couldn't change equipment");
            throw;
        }
	}

    //Use a usable item at the index specified
	public void UseItem(int i)
	{
		if (_inventory [i].GetComponent<UsableItem> () != null)
		{
            _inventory[i].GetComponent<UsableItem>().UseItem();
        }
        DeleteStackableItems();
	}

    //Check for stackable items in the player's inventory
	public int CheckStackableItem(GameObject h)
	{
		if(h.GetComponent<StackableItem>() != null)
		{
			foreach(GameObject g in _inventory)
			{
				if(g.GetComponent<GenericItem>().itemname == h.GetComponent<GenericItem>().itemname)
				{
					return _inventory.IndexOf(g);
				}
			}
		}
		return -1;
	}

    //Deletes any stackable item in the player inventory if the amount is 0
    void DeleteStackableItems()
    {
        List<int> toRemove = new List<int>();
        for (int a = 0; a < inventory.Count; a++)
        {
            try
            {
                if (inventory[a].GetComponent<StackableItem>().Amount <= 0)
                {
                    toRemove.Add(a);
                }
            }
            catch { }
        }
        foreach (int i in toRemove)
        {
            inventory.RemoveAt(i);
        }
        updatei();
    }
}
