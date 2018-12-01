using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoreButton : MonoBehaviour {

    private GenericItem SkillCore;
    private Text thistext;
    private PlayerStats player;
    private PlayerInventory playerinv;
    private bool HasSkillCore;

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

    //Update the text on the button
    void UpdateText()
    {
        try
        {
            SkillCore = player.GetComponent<PlayerEquipment>().SkillCore.GetComponent<GenericItem>();
            this.GetComponent<HoverText>().SetText(SkillCore.GetComponent<GenericItem>().FormattedDesc);
            HasSkillCore = true;
            thistext.text = SkillCore.itemname;
        }
        catch
        {
            HasSkillCore = false;
            this.GetComponent<HoverText>().SetText("");
            thistext.text = "No Skill Core";
        }
    }

    //When the button is clicked, unequip the current skill core if there is one equipped
    public void OnClick()
    {
        if (HasSkillCore)
        {
            if (!playerinv.isinvfull())
            {
                playerinv.UnEquip(typeof(SkillCore));
            }
            else
            {
                TempPopup.Show("Inventory is full!", Color.red);
            }
        }
    }

    private void OnDestroy()
    {
        PlayerInventory.ChangedEQ -= UpdateText;
        ExpBar.increaselevel -= UpdateText;
        PlayerStats.CheckStats -= UpdateText;
    }
}
