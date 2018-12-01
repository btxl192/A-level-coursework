using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDrop : GenericEnemy
{
    [SerializeField]
    private int amounttodrop;
	private Dictionary<GameObject, int> test = new Dictionary<GameObject, int>();

    protected override void Start()
    {
        base.Start();
        for (int a = 0; a < amounttodrop; a++)
        {
            DropRandomItem();   
        }
		foreach (GameObject key in test.Keys) 
		{
			Debug.Log (key + ": " + test [key]);
		}
    }
}
