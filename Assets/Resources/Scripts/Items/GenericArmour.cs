using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1.3.2.1.4
[System.Serializable]
public class GenericArmour : GenericItem {

    public override string FormattedDesc
    {
        get
        {
            string d = base.FormattedDesc;
            d += "\n\nBase Armour: " + Armour;
            return d;
        }
    }

    [SerializeField]
    protected int _Armour;
    public int Armour
    {
        get {return _Armour; }
    }

	protected override void Awake () 
	{
        base.Awake();
		_category = CategoryEnum.Armour;
	}
	
    //Method to run if want to randomise stats
	public void RandomiseStats(GenericItem.Rarities rarity, float multiplier)
	{
		_category = CategoryEnum.Armour;
        _itemname = rarity + " Armour";
		if (rarity == Rarities.Common)
        {
            _Price = 50;
            _Armour = Random.Range(110, 200);
            _desc = "A common piece of armour";
        }
        if (rarity == Rarities.Rare)
        {
            _Price = 100;
            _Armour = Random.Range(210, 300);
            _desc = "A rare piece of armour";
        }
        if (rarity == Rarities.Epic)
        {
            _Price = 200;
            _Armour = Random.Range(310, 600);
            _desc = "An epic piece of armour!";
        }
        _Price += (int)(_Armour * 0.2f);
        _Armour = (int)(_Armour * multiplier);
    }

    public override string GetSaveData()
    {
        return base.GetSaveData() + "," + Armour;
    }

    public void SetArmour(int i)
    {
        _Armour = i;
    }

}
