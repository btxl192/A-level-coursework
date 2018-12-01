using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantUI : MonoBehaviour {

    public enum Mode {Buy, Sell, BuyBack}

	private int MerchantID;
    public static Mode CurrentMode
    {
        get;
        private set;
    }
	private static bool _CurrentlyOpen;
	public static bool CurrentlyOpen
	{
		get {return _CurrentlyOpen; }
	}

    public delegate void UpdateItems(int ID);
    public static event UpdateItems UpdateBuy;
    public static event UpdateItems UpdateSell;

    public delegate void BlankEvent();
    public static event BlankEvent DeselectButtons;
    public static event BlankEvent SetBuyYellow;

    public void Awake()
	{
		Merchant.OpenTradeMenu += OpenMenu;
		Merchant.CloseTradeMenu += CloseMenu;
        BuyButton.SwitchBuy += SwitchMode;
        SellButton.SwitchSell += SwitchMode;
        BuyBackButton.SwitchBuyBack += SwitchMode;
		this.gameObject.GetComponent<Canvas>().enabled = false;
	}

    //Set the buy button to yellow
    public void Start()
    {
        SetBuyYellow();
    }

    //Open the menu and set the merchant id to the merchant whose wares should be displayed
    public void OpenMenu(int merchantid)
	{
		_CurrentlyOpen = true;
		MerchantID = merchantid;
		this.gameObject.GetComponent<Canvas>().enabled = true;
        UpdateBuy(MerchantID);
        GenericMenu2.SetOpenMenu(this.gameObject);
	}

    //Close the menu
	public void CloseMenu(int merchantid)
	{
		this.gameObject.GetComponent<Canvas>().enabled = false;
		_CurrentlyOpen = false;
        GenericMenu2.SetOpenMenu(null);
    }

    //Update each of the menu's item buttons
	public void UpdateSlots(Mode m)
	{
		foreach(ItemButton i in this.GetComponentsInChildren<ItemButton>())
		{
            i.UpdateSlot(MerchantID, m);
		}
	}

    //Switch the mode of merchant UI
    void SwitchMode(Mode m)
    {
        DeselectButtons();
        CurrentMode = m;
        if (m == Mode.Buy || m == Mode.BuyBack)
        {
            UpdateBuy(MerchantID);
        }
        else if (m == Mode.Sell)
        {
            UpdateSell(MerchantID);
        }
    }

    private void OnDestroy()
    {
        Merchant.OpenTradeMenu -= OpenMenu;
        Merchant.CloseTradeMenu -= CloseMenu;
        BuyButton.SwitchBuy -= SwitchMode;
        SellButton.SwitchSell -= SwitchMode;
        BuyBackButton.SwitchBuyBack -= SwitchMode;
    }
}
