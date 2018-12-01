using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

//Objective 1.3.2.2.6.a
public class RangedEnemy : GenericEnemy {

    [SerializeField]
	protected GameObject Projectile; 
	protected float atttimer;
    [SerializeField]
    protected float attspeed;
    [SerializeField]
    protected float Range;
    private Color DefaultStaffColour;

	protected override void Start () 
	{
		base.Start();
		atttimer = 0f;
        DefaultStaffColour = transform.Find("Staff/Cube").GetComponent<Renderer>().material.color;

    }
	
    //Fire a projectile every few seconds based on the enemy's attack speed (attspeed)
	protected override void AdditionalBehaviour () 
	{
        atttimer += Time.deltaTime;
        base.AdditionalBehaviour();
		if(Physics.Linecast(transform.position,Target.transform.position))
		{
			if(Vector3.Distance(Target.transform.position,this.gameObject.transform.position) <= Range)
			{
				Vector3 look = Target.transform.position - transform.position;
				look.y = 0;
				transform.rotation = Quaternion.LookRotation((look).normalized);
				this.gameObject.GetComponent<NavMeshAgent> ().SetDestination (transform.position);
				if(atttimer>+attspeed)
				{
					atttimer = 0f;
					Projectile.GetComponent<GenericProjectile>().Shoot(Projectile, transform.Find("Staff/Cube").gameObject, Target, CalcDamage(Projectile.GetComponent<GenericProjectile>().damage, Level), false, 300f);
				}
			}
			else
			{
				this.gameObject.GetComponent<NavMeshAgent> ().SetDestination (Target.transform.position);
			}
		}
        transform.Find("Staff/Cube").GetComponent<Renderer>().material.color = DefaultStaffColour * (atttimer/attspeed);

    }

    //Calculate the damage of the projectile
    public int CalcDamage(int Damage, int Level)
    {
        return (int)(Damage * (1 + ((Level - 1) * 0.2f)));
    }

    public override string GetSaveData()
    {
        string data = base.GetSaveData();
        data += "/" + Projectile.GetComponent<GenericItem>().GetSaveData();
        return data;
    }

    public override void SetDetails(string[] details)
    {
        base.SetDetails(details);
        Projectile = PlayerSave.LoadItem(details[details.Length - 1].Split(",".ToCharArray()[0]));
    }
}
