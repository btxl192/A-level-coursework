using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItemDrop
{
    public GameObject Item { get; private set; }
    public int Skew { get; private set; }
    public int SumSkew { get; set; }

    public EnemyItemDrop(GameObject item, int skew)
    {
        Item = item;
        Skew = skew;
        SumSkew = 0;
    }

}
