using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.1.1
public class StackableItem : GenericItem
{
    public override string FormattedDesc
    {
        get
        {
            string d = base.FormattedDesc;
            d += "\n\nAmount: " + Amount;
            return d;
        }
    }

    public delegate void CheckItemEvent();
    public static event CheckItemEvent CheckItem;

    [SerializeField]
    protected int _Amount;
    public int Amount
    {
        get
        {
            return _Amount;
        }
        protected set
        {
            _Amount = value;
        }
    }

    //Change the amount of the item
    public void ChangeAmount(int amount)
    {
        Amount += amount;
        CheckItem();
    }

    //Sets the amount of the item
    public void SetAmount(int amount)
    {
        Amount = amount;
    }

    public override string GetSaveData()
    {
        return base.GetSaveData() + "," + Amount;
    }

}
