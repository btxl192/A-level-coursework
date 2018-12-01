using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeaponPickup : RandomPickup {

	protected override void setstats(float multiplier)
	{
        item.GetComponent<GenericWeapon>().RandomiseStats(item.GetComponent<GenericItem>().Rarity, multiplier);
	}
}
