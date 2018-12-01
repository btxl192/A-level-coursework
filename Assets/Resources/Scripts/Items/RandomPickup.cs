using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RandomPickup : Pickup {

    [SerializeField]
    protected GameObject ItemReference;
    [SerializeField]
    protected float mult;

    public void SetMultiplier(float amount)
    {
        mult = amount;
    }

    protected override void Start()
    {
        Initialise();
        item = Instantiate(ItemReference, new Vector3(0f, 1000f, 0f), Quaternion.identity);
        int rarityvalue = Random.Range(1, 1000);
        item.GetComponent<GenericItem>().Rarity = GenericItem.Rarities.Common;
        if (rarityvalue > 800)
        {
            item.GetComponent<GenericItem>().Rarity = GenericItem.Rarities.Epic;
            this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(119f, 0f, 255f);
        }
        else if (rarityvalue > 350)
        {
            item.GetComponent<GenericItem>().Rarity = GenericItem.Rarities.Rare;
            this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0f, 255f, 0f);
        }
        setstats(mult);
        item.SetActive(false);
        SetCategory(item.GetComponent<GenericItem>().category);
    }

    protected abstract void setstats(float multiplier);
}
