using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMerchantButtons : MonoBehaviour 
{
	private GameObject MerchantButtonsReference;
    private PlayerInventory Player;

	void Awake()
	{
		MerchantButtonsReference = Resources.Load(FileDir.MerchantButton) as GameObject;
        MerchantUI.UpdateBuy += UpdateItemsBuy;
        MerchantUI.UpdateSell += UpdateItemsSell;
        ItemButton.UpdateBuy += UpdateItemsBuy;
        ItemButton.UpdateSell += UpdateItemsSell;
    }

    private void Start()
    {
        Player = PlayerSave.staticplayer.GetComponent<PlayerInventory>();
    }

    //Loads the items from the merchant's list of items
    public void UpdateItemsBuy(int merchantid)
	{
        foreach (ItemButton i in this.gameObject.GetComponentsInChildren<ItemButton>())
        {
            Destroy(i.gameObject);
        }
        if (MerchantUI.CurrentMode == MerchantUI.Mode.Buy)
        {
            for (int a = 0; a < MerchantList.Merchants[merchantid].Wares.Count; a++)
            {
                GameObject temp;
                temp = Instantiate(MerchantButtonsReference, this.transform);
                temp.GetComponent<ItemButton>().SlotIndex = a;
                temp.GetComponent<ItemButton>().UpdateSlot(merchantid, MerchantUI.Mode.Buy);
            }
        }
        else if (MerchantUI.CurrentMode == MerchantUI.Mode.BuyBack)
        {
            for (int a = 0; a < MerchantList.Merchants[merchantid].BuyBack.Count; a++)
            {
                GameObject temp;
                temp = Instantiate(MerchantButtonsReference, this.transform);
                temp.GetComponent<ItemButton>().SlotIndex = a;
                temp.GetComponent<ItemButton>().UpdateSlot(merchantid, MerchantUI.Mode.BuyBack);
            }
        }
	}

    //Loads the player's items to sell
    public void UpdateItemsSell(int merchantid)
    {
        foreach (ItemButton i in this.gameObject.GetComponentsInChildren<ItemButton>())
        {
            Destroy(i.gameObject);
        }
        for (int a = 0; a < Player.inventory.Count; a++)
        {
            GameObject temp;
            temp = Instantiate(MerchantButtonsReference, this.transform);
            temp.GetComponent<ItemButton>().SlotIndex = a;
            temp.GetComponent<ItemButton>().UpdateSlot(merchantid, MerchantUI.Mode.Sell);
        }
    }

    private void OnDestroy()
    {
        MerchantUI.UpdateBuy -= UpdateItemsBuy;
        MerchantUI.UpdateSell -= UpdateItemsSell;
        ItemButton.UpdateBuy -= UpdateItemsBuy;
        ItemButton.UpdateSell -= UpdateItemsSell;
    }
}
