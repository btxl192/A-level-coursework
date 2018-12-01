using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour {

    private GenericItem Weapon;
    private Text thistext;
    private PlayerStats player;
    private PlayerInventory playerinv;
    private bool HasWeapon;

    private void Start()
    {
        thistext = this.gameObject.GetComponentInChildren<Text>();
        player = PlayerSave.staticplayer.GetComponent<PlayerStats>();
        playerinv = PlayerSave.staticplayer.GetComponent<PlayerInventory>();
        PlayerInventory.ChangedEQ += UpdateText;
        ExpBar.increaselevel += UpdateText;
        PlayerStats.CheckStats += UpdateText;
        UpdateText();
    }

    void OnDestroy()
    {
        PlayerInventory.ChangedEQ -= UpdateText;
        ExpBar.increaselevel -= UpdateText;
        PlayerStats.CheckStats -= UpdateText;
    }

    //Update the text on the button
    public void UpdateText()
    {
        try
        {
            Weapon = player.GetComponent<PlayerEquipment>().WeaponObject.GetComponent<GenericItem>();
            this.GetComponent<HoverText>().SetText(Weapon.GetComponent<GenericItem>().FormattedDesc);
            thistext.text = Weapon.itemname;
            HasWeapon = true;
        }
        catch
        {
            HasWeapon = false;
            this.GetComponent<HoverText>().SetText("");
            thistext.text = "No Weapon";
        }
    }

    //When the button is clicked, unequip the current weapon if there is one equipped
    public void OnClick()
    {
        if (HasWeapon)
        {
            if (!playerinv.isinvfull())
            {
                playerinv.UnEquip(typeof(GenericWeapon));
            }
            else
            {
                TempPopup.Show("Inventory is full!", Color.red);
            }
        }        
    }
}
