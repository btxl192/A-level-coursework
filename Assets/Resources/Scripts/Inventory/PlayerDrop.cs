using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDrop : MonoBehaviour {

    int ItemLoc;
    ItemDrop player;
    PlayerInventory playerinv;

	void Start ()
    {
        ItemDetails.PassEq += GetItemLoc;
        player = PlayerSave.staticplayer.GetComponent<ItemDrop>();
        playerinv = PlayerSave.staticplayer.GetComponent<PlayerInventory>();
        this.GetComponent<Button>().onClick.AddListener(DropItem);
        ItemDetails.ChangeText += CheckText;
    }

    private void OnDestroy()
    {
        ItemDetails.ChangeText -= CheckText;
    }

    //Get the index of the item in the player's inventory
    void GetItemLoc(int i)
    {
        ItemLoc = i;
    }

    void DropItem()
    {
        try
        {
            player.DropItem(playerinv.inventory[ItemLoc], 60f, 3f);
            playerinv.removeat(ItemLoc);
        }
        catch
        {
            TempPopup.Show("No item selected!", Color.red);
        }
    }

    void CheckText(string t)
    {
        if (t == "")
        {
            this.gameObject.GetComponent<Button>().enabled = false;
            this.gameObject.GetComponent<Image>().enabled = false;
            this.gameObject.GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<Button>().enabled = true;
            this.gameObject.GetComponent<Image>().enabled = true;
            this.gameObject.GetComponentInChildren<Text>().enabled = true;
        }
    }
}
