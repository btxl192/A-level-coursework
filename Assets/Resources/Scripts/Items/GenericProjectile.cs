using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.1.2
public class GenericProjectile : GenericWeapon {

	protected float lifetime;

    protected void Update()
	{
		if (!Pause.Paused)
		{
			lifetime += Time.deltaTime;
			if (lifetime >= 5f)
			{
				Destroy (this.gameObject); //If the projectile is "alive" for more than 5 seconds, it will be destroyed
			}
		}
	}

    //If the projectile collides with anything, destroy the projectile. 
    protected override void OnTriggerEnter(Collider other) 
	{
        try
        {
            if (whitelisttags.IndexOf(other.GetComponent<Collider>().tag) == -1 && ExtraWhiteListtags.IndexOf(other.GetComponent<Collider>().tag) == -1 && other.gameObject.layer != 2)
            {
                if (other.tag == "Player")
                {
                    other.GetComponent<PlayerBehaviour>().takedamage(damage);
                }
                foreach (Statuses.StatusType s in StatusEffects)
                {
                    try
                    {
                        other.GetComponent<Statuses>().ApplyStatus(s, this.gameObject);
                    }
                    catch
                    { }
                }
                OnCollide(other);
                Destroy(this.gameObject);  
            }
        }
        catch
        {
            Debug.Log("ERROR: projectile collided but something went wrong");
            throw;
        }
    }

    protected virtual void OnCollide(Collider other) {}

    public virtual void Shoot(GameObject Projectile, GameObject Shooter, GameObject Target, int damage, bool isplayerweapon)
    {
        GameObject shotprojectile = Instantiate(Projectile);
        shotprojectile.GetComponent<GenericProjectile>().isPlayerWeapon = isplayerweapon;
        Rigidbody projectilerb = shotprojectile.GetComponent<Rigidbody>();
        float ForceToShootWith;
        shotprojectile.GetComponent<GenericWeapon>().AddToWhiteList(Shooter.tag);    
        if (Shooter.tag == "Enemy")
        {
            ForceToShootWith = 30000f;
        }
        else
        {
            ForceToShootWith = 11000f;
        }
        shotprojectile.GetComponent<GenericWeapon>().SetDamage(damage);
        if (Target != null)
        {
            shotprojectile.transform.rotation = Quaternion.LookRotation(Target.transform.position - Shooter.transform.position);
            shotprojectile.transform.position = ((Target.gameObject.transform.position - Shooter.transform.position).normalized * Shooter.transform.localScale.x * 0.75f) + Shooter.transform.position;
        }
        else
        {
            shotprojectile.transform.SetPositionAndRotation((Shooter.transform.forward * Shooter.transform.localScale.x * 0.75f) + Shooter.transform.position, Shooter.transform.rotation);
        }
        shotprojectile.transform.Rotate(new Vector3(90f, 0f, 0f));
        shotprojectile.SetActive(true);
        if (Target != null)
        {
            if (Target.CompareTag("Player"))
            {
                projectilerb.AddForce((InterpolatePlayerPosition() - Shooter.transform.position).normalized * ForceToShootWith);
            }
            else
            {
                projectilerb.AddForce((Target.gameObject.transform.position - Shooter.transform.position).normalized * ForceToShootWith);
            }
        }
        else
        {
            projectilerb.AddForce(Shooter.transform.forward.normalized * ForceToShootWith);
        }
    }

    public virtual void Shoot(GameObject Projectile, GameObject Shooter, GameObject Target, int damage, bool isplayerweapon, float ForceToShootWith)
    {
        GameObject shotprojectile = Instantiate(Projectile);
        shotprojectile.GetComponent<GenericProjectile>().isPlayerWeapon = isplayerweapon;
        Rigidbody projectilerb = shotprojectile.GetComponent<Rigidbody>();
        shotprojectile.GetComponent<GenericWeapon>().AddToWhiteList(Shooter.tag);
        shotprojectile.GetComponent<GenericWeapon>().SetDamage(damage);
        if (Target != null)
        {
            shotprojectile.transform.rotation = Quaternion.LookRotation(Target.transform.position - Shooter.transform.position);
            shotprojectile.transform.position = ((Target.gameObject.transform.position - Shooter.transform.position).normalized * Shooter.transform.localScale.x * 0.75f) + Shooter.transform.position;
        }
        else
        {
            shotprojectile.transform.SetPositionAndRotation((Shooter.transform.forward * Shooter.transform.localScale.x * 0.75f) + Shooter.transform.position, Shooter.transform.rotation);
        }
        shotprojectile.transform.Rotate(new Vector3(90f, 0f, 0f));
        shotprojectile.SetActive(true);
        if (Target != null)
        {
            if (Target.CompareTag("Player"))
            {
                projectilerb.AddForce((InterpolatePlayerPosition() - Shooter.transform.position).normalized * ForceToShootWith);
            }
            else
            {
                projectilerb.AddForce((Target.gameObject.transform.position - Shooter.transform.position).normalized * ForceToShootWith);
            }            
        }
        else
        {
            projectilerb.AddForce(Shooter.transform.forward.normalized * ForceToShootWith);
        }
    }

    //Randomise the damage values
    //Objective 1.3.2.2.3.iii
    public override void RandomiseStats(GenericItem.Rarities rarity, float multiplier)
    {
        this.gameObject.GetComponent<GenericItem>().SetCategory(GenericItem.CategoryEnum.Projectile);
        if (rarity == GenericItem.Rarities.Common) 
		{
            _Price = 50;
			_damage = Random.Range (22, 40);
            this.gameObject.GetComponent<GenericItem>().SetDetails(rarity + " " + this.gameObject.GetComponent<GenericItem>().category, "A common arrow");
		}
		if (rarity == GenericItem.Rarities.Rare) 
		{
            _Price = 100;
            _damage = Random.Range (41, 50);
            this.gameObject.GetComponent<GenericItem>().SetDetails(rarity + " " + this.gameObject.GetComponent<GenericItem>().category, "A rare arrow");
		}
		if (rarity == GenericItem.Rarities.Epic) 
		{
            _Price = 200;
            _damage = Random.Range (51, 60);
            this.gameObject.GetComponent<GenericItem>().SetDetails(rarity + " " + this.gameObject.GetComponent<GenericItem>().category, "An epic arrow!");
		}
        _Price += (int)(_damage * 0.6f);
        _damage = (int)(_damage * multiplier);
    }

    public static Vector3 InterpolatePlayerPosition()
    {
        return PlayerSave.staticplayer.GetComponent<PlayerMovement>().direction * 0.5f + PlayerSave.staticplayer.transform.position;
    }
}
