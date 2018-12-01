using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomArmourPickup : RandomPickup {

    protected override void setstats(float multiplier)
    {
        item.GetComponent<GenericArmour>().RandomiseStats(item.GetComponent<GenericItem>().Rarity, multiplier);
    }
}
