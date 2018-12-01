using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ware {

	public bool IsPlayerItem { get; private set; }
	public GameObject ItemObject { get; private set; }
	public GenericItem Item { get; private set; }
    public int Quantity { get; private set; }

	public Ware(GameObject item, int quantity, bool isPlayerItem)
	{
		ItemObject = MonoBehaviour.Instantiate(item);
        if (isPlayerItem)
        {
            ItemObject.transform.SetParent(PlayerSave.staticplayer.transform);
        }
		Item = ItemObject.GetComponent<GenericItem>();
        Quantity = quantity;
		IsPlayerItem = isPlayerItem;
	}

    public void ChangeQuantity(int Amount)
    {
        Quantity += Amount;
    }

	public void SetIsPlayer(bool b)
	{
		IsPlayerItem = b;
	}
}
