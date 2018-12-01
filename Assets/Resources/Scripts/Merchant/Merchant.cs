using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : Interactable {

    public enum Presets { PotionSeller, TestPreset};

    [SerializeField]
    private Presets Preset;

    [SerializeField]
	private List<Ware> _Wares = new List<Ware>();
	public List<Ware> Wares
	{
		get {return _Wares;}
	}

	[SerializeField]
	private static List<Ware> _BuyBack = new List<Ware>();
	public List<Ware> BuyBack
	{
		get {return _BuyBack;}
	}

    private PlayerInventory PlayerInv;
    private MinimapIcon minimapicon;
	public delegate void MerchantEvent(int merchantid);
	public static event MerchantEvent OpenTradeMenu;
	public static event MerchantEvent CloseTradeMenu;

    [SerializeField]
	public int MerchantID
	{
		get;
		private set;
	}

	protected override void Awake () 
	{
		PopupText = "Trade";
		base.Awake();
		MerchantList.Merchants.Add(this.gameObject.GetComponent<Merchant>());
		MerchantID = MerchantList.Merchants.IndexOf(this);
		ItemButton.SellItem += SellWare;
        ItemButton.BuyItem += BuyFromPlayer;
        LoadWares(Preset);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        ItemButton.SellItem -= SellWare;
        ItemButton.BuyItem -= BuyFromPlayer;
    }

    private void Start()
    {
        Minimap.CreateMiscIcon(Resources.Load(FileDir.MerchantIcon) as GameObject, out minimapicon);
        minimapicon.ResizeIcon(new Vector2(10f, 10f));
        PlayerInv = PlayerSave.staticplayer.GetComponent<PlayerInventory>();
    }

    //Show the merchant UI when interacting
    protected override void Interact()
	{
		if(!MerchantUI.CurrentlyOpen)
		{
			OpenTradeMenu(MerchantID);
		}
		else if(MerchantUI.CurrentlyOpen)
		{
			Interacting = false;
			CloseTradeMenu(MerchantID);
		}		
	}

    //Close the merchant UI when out of range
	protected override void OutOfRange()
	{
		CloseTradeMenu(MerchantID);
	}

    //Sell an item to the player
    //Objective 1.3.2.6.2
	void SellWare(int merchantid, int WareIndex)
	{
        List<Ware> temp;
        if (MerchantUI.CurrentMode == MerchantUI.Mode.Buy)
        {
            temp = Wares;
        }
        //Objective 1.3.2.6.4
        else if (MerchantUI.CurrentMode == MerchantUI.Mode.BuyBack)
        {
            temp = BuyBack;
        }
        else
        {
            temp = null;
        }
        if (PlayerInv.Gold >= temp[WareIndex].Item.Price && merchantid == MerchantID)
        {
            if (!PlayerInv.isinvfull())
            {
                PlayerInv.AddGold(-temp[WareIndex].Item.Price);
                PlayerInv.additem(Instantiate(temp[WareIndex].ItemObject), true);
                //Objective 1.3.2.6.7
                if (temp[WareIndex].Quantity != -1)
                {
                    temp[WareIndex].ChangeQuantity(-1);
                    if (temp[WareIndex].Quantity <= 0)
                    {
                        temp.RemoveAt(WareIndex);
                    }
                }
            }
            else
            {
                TempPopup.Show("Inventory is full! Couldn't purchase item.", Color.red);
            }
        }
        else
        {
            TempPopup.Show("Not enough gold to purchase!", Color.red);
        }
	}

    //buy an item from  the player
    //Objective 1.3.2.6.3
    void BuyFromPlayer(int merchantid, int PlayerInvIndex)
    {
        if (merchantid == MerchantID)
        {
            PlayerInv.AddGold(PlayerInv.inventory[PlayerInvIndex].GetComponent<GenericItem>().Price);           
            if (PlayerInv.inventory[PlayerInvIndex].GetComponent<StackableItem>() != null)
            {
                GameObject temp = Instantiate(PlayerInv.inventory[PlayerInvIndex]);
                bool exists = false;
                temp.GetComponent<StackableItem>().SetAmount(1);
                foreach (Ware w in BuyBack)
                {
                    if (w.ItemObject.GetComponent<GenericItem>().itemname == temp.GetComponent<GenericItem>().itemname)
                    {
                        exists = true;
                        w.ChangeQuantity(1);
                    }
                }
                if (!exists)
                {
                    BuyBack.Add(new Ware(temp, 1, true));
                }               
                PlayerInv.inventory[PlayerInvIndex].GetComponent<StackableItem>().ChangeAmount(-1);
            }
            else
            {
                BuyBack.Add(new Ware(PlayerInv.inventory[PlayerInvIndex], 1, true));
                PlayerInv.inventory.RemoveAt(PlayerInvIndex);
            }
            
        }
    }

    //Load the merchant's items
    public void LoadWares(Presets MerchantPreset)
    {
        if (MerchantPreset == Presets.PotionSeller)
        {
            _Wares = new List<Ware>
            {
                new Ware(Resources.Load(FileDir.HealingPotion) as GameObject,-1,false),
                new Ware(Resources.Load(FileDir.GreaterHealingPotion) as GameObject, 10, false),
                new Ware(Resources.Load(FileDir.AssassinTome) as GameObject, 1, false)
            };
        }
        else if (MerchantPreset == Presets.TestPreset)
        {
            _Wares = new List<Ware>
            {
            };
        }
    }

    //Close the UI
    protected override void UnInteract()
    {
        OutOfRange();
    }

    //Update the merchant's minimap icon
    protected override void AdditionalBehaviour()
    {
        Minimap.UpdateMiscIcon(minimapicon, transform.position);
    }

}
