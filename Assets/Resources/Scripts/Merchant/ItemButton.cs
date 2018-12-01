using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {

	public delegate void ItemEvent(int merchantid, int itemindex);
	public static event ItemEvent SellItem;
    public static event ItemEvent BuyItem;

    public delegate void UpdateUIEvent(int merchantid);
    public static event UpdateUIEvent UpdateBuy;
    public static event UpdateUIEvent UpdateSell;

    private Ware ware;

    public int SlotIndex {get; set;}

	public int MerchantID {get; private set;}

    void Awake () 
	{
        this.gameObject.GetComponent<Button>().onClick.AddListener (ButtonClicked);
		ware = MerchantList.Merchants [MerchantID].Wares [SlotIndex];
	}

    private void Start()
    {
        UpdateHoverText();
    }

    //Buy or sell the item, depending on the merchant's mode
    void ButtonClicked()
    {
        switch (MerchantUI.CurrentMode)
        {
            case MerchantUI.Mode.Buy:
            case MerchantUI.Mode.BuyBack:
                SellItem(MerchantID, SlotIndex);
                UpdateBuy(MerchantID);
                break;
            case MerchantUI.Mode.Sell:
                BuyItem(MerchantID, SlotIndex);
                UpdateSell(MerchantID);
                break;
            default:
                Debug.Log("MERCHANT BUTTON NOT WORKING");
                break;
        }
    }

    //Update the item the button should be reading from
	public void UpdateSlot(int merchantid, MerchantUI.Mode m)
	{
		MerchantID = merchantid;
        if (m == MerchantUI.Mode.Buy)
        {
            ware = MerchantList.Merchants[MerchantID].Wares[SlotIndex];
        }
        else if (m == MerchantUI.Mode.Sell)
        {
            int amount;
            try
            {
                amount = PlayerSave.staticplayer.GetComponent<PlayerInventory>().inventory[SlotIndex].GetComponent<StackableItem>().Amount;
            }
            catch
            {
                amount = 1;
            }
            ware = new Ware(PlayerSave.staticplayer.GetComponent<PlayerInventory>().inventory[SlotIndex], amount, true);
        }
        else if (m == MerchantUI.Mode.BuyBack)
        {
            ware = MerchantList.Merchants[MerchantID].BuyBack[SlotIndex];
        }
        //Objective 1.3.2.6.5
        this.gameObject.GetComponentInChildren<Text>().text = ware.Item.itemname + " (" + ware.Item.Price + " gold)";
        //Objective 1.3.2.6.7
        if (ware.Quantity == -1)
        {
            this.gameObject.GetComponentsInChildren<Text>()[1].text = "Stock: Unlimited";
        }
        else
        {
            this.gameObject.GetComponentsInChildren<Text>()[1].text = "Stock: " + ware.Quantity;
        }      
        UpdateHoverText();
	}

    //Update the text on the buttons HoverText
    //Objective 1.3.2.6.6
    void UpdateHoverText()
    {
        this.GetComponent<HoverText>().SetText(ware.Item.GetComponent<GenericItem>().FormattedDesc);
    }
}
