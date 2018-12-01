using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningProjectile : GenericWeapon {

    private int MaxEnemies;
    private GameObject LightningObject;
    private float scale = 0.1f;
    private List<Branch> EnemiesToHit = new List<Branch>();
    public bool IsInitial = false;

    struct Branch
    {
        public GameObject Shooter;
        public GameObject Target;

        public Branch(GameObject shooter, GameObject target)
        {
            Shooter = shooter;
            Target = target;
        }
    }

	protected new void Awake()
    {
        MaxEnemies = ChainLightning.MaxEnemiesHit;
    }

    //Fires the projectile and sets it as the initial chain lightning
	public void Shoot(GameObject Shooter, GameObject Target, int dmg, bool isInitial)
	{
	    _damage = dmg;
        LightningObject = Resources.Load(FileDir.ChainLightning) as GameObject;
        GameObject shotprojectile = Instantiate(LightningObject, Shooter.transform.position + (Target.transform.position - Shooter.transform.position).normalized*Shooter.transform.localScale.x, Shooter.transform.rotation);
        shotprojectile.transform.rotation = Quaternion.LookRotation(Target.transform.position - Shooter.transform.position);
        shotprojectile.transform.Rotate(new Vector3(90f,0f,0f));
        shotprojectile.GetComponent<ChainLightningProjectile>().IsInitial = isInitial;
        shotprojectile.GetComponent<GenericWeapon>().SetDamage(damage);
        shotprojectile.SetActive(true);
	}

    //Extends the projectile
    private void Update()
    {
        if (this.GetComponent<Transform>().localScale.y > 0)
        {
            this.GetComponent<Transform>().localScale = new Vector3(this.GetComponent<Transform>().localScale.x, this.GetComponent<Transform>().localScale.y + scale, this.GetComponent<Transform>().localScale.z);
        }
        else if (this.GetComponent<Transform>().localScale.y <= 0)
        {
            this.GetComponent<Transform>().localScale = Vector3.zero;
            Destroy(this.gameObject);
        }
        transform.position += (this.transform.up * Mathf.Abs(scale));
    }

    //When the projectile hits anything not another projectile, shrink. If it hits an enemy, look for nearby enemies and fire a projectile at them too
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Projectile")
        {
            scale = -Mathf.Abs(scale);
        }       

        if (other.gameObject.tag == "Enemy")
        {
            if (IsInitial)
            {
                List<GameObject> temp = new List<GameObject>();
				EnemiesToHit = EnemiesCloseTo(other.gameObject, ref temp);
				if (EnemiesToHit.Count + 1 > MaxEnemies) 
				{
					EnemiesToHit = EnemiesToHit.GetRange (0, MaxEnemies - 1);
				}
                if (EnemiesToHit.Count > 0)
                {
					for (int a = 0; a < EnemiesToHit.Count; a++)
                    {
                        Shoot(EnemiesToHit[a].Shooter, EnemiesToHit[a].Target, damage, false);
                    }
                }
            }
        }
    }

    //Looks for enemies nearby
    List<Branch> EnemiesCloseTo(GameObject enemyobject, ref List<GameObject> enemiescounted)
	{
        List<Branch> temp = new List<Branch>();
		enemiescounted.Add (enemyobject);
		foreach (GameObject enemy in Enemies.GetEnemies()) 
		{
			if (Vector3.Distance(enemyobject.transform.position, enemy.transform.position) <= 1.5f && !enemiescounted.Contains(enemy))
			{
				temp.Add(new Branch(enemyobject, enemy));
				temp.AddRange(EnemiesCloseTo(enemy, ref enemiescounted));
			}
		}
		return temp;
	}
}
