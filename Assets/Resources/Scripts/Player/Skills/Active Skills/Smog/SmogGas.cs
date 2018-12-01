using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmogGas : MonoBehaviour {

	private int dmg;
    private int Level;

	public int Damage
	{
		get {return dmg; }
	}
	
    //Fade out
	void Awake () 
	{
		this.gameObject.AddComponent<FadeOut>().Initialise(10f, true);
	}
		
    //Apply the poison status effect to any enemy the cloud comes in contact with
	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<GenericEnemy> () != null) 
		{
            other.GetComponent<Statuses>().ApplyStatus(Statuses.MakeStatus(Statuses.statuses.Poison, Level), dmg * 2);
		}
	}

	public void SetDamage(int damage)
	{
		dmg = damage;
		this.gameObject.SetActive (true);
	}

    public void SetLevel(int lvl)
    {
        Level = lvl;
    }
}
