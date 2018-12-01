using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmourButton : MonoBehaviour {

    private GenericItem Armour;
    private Text thistext;
    private PlayerStats player;
    private PlayerInventory playerinv;
    private bool HasArmour;

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

    private void OnDestroy()
    {
        PlayerInventory.ChangedEQ -= UpdateText;
        ExpBar.increaselevel -= UpdateText;
        PlayerStats.CheckStats -= UpdateText;
    }

    //Update the text on the button
    void UpdateText()
    {
        try
        {
            Armour = player.GetComponent<PlayerEquipment>().ArmourObject.GetComponent<GenericItem>();
            this.GetComponent<HoverText>().SetText(Armour.GetComponent<GenericItem>().FormattedDesc);
            HasArmour = true;
            thistext.text = Armour.itemname;
        }
        catch
        {
            HasArmour = false;
            this.GetComponent<HoverText>().SetText("");
            thistext.text = "No Armour";
        }
    }

    //When the button is clicked, unequip the armour if it exists
    public void OnClick()
    {
        if (HasArmour)
        {
            if (!playerinv.isinvfull())
            {
                playerinv.UnEquip(typeof(GenericArmour));
            }
            else
            {
                TempPopup.Show("Inventory is full!", Color.red);
            }
        }
    }

}
