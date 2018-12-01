using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBlast : MonoBehaviour {

	private int dmg;
    private bool AffectPlayer;

	void Awake () 
	{
		gameObject.AddComponent<FadeOut>().Initialise(1f, true);
	}
		
    //Damage the player and any enemies caught in the explosion
	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<GenericEnemy> () != null) 
		{
			other.GetComponent<GenericEnemy> ().TakeDamage (dmg);
		}
        if (AffectPlayer && other.GetComponent<PlayerBehaviour>() != null)
        {
            other.GetComponent<PlayerBehaviour>().takedamage(dmg);
            AffectPlayer = false;
        }
	}

	public void SetDamage(int damage)
	{
		dmg = damage;
		gameObject.SetActive (true);
	}

    public void SetDetails(int damage, bool affectplayer)
    {
        dmg = damage;
        AffectPlayer = affectplayer;
        gameObject.SetActive(true);
    }
}
