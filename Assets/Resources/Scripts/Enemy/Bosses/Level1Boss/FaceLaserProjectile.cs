using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceLaserProjectile : GenericWeapon
{

    private float scale = 0.2f;
    private GameObject LaserObject;

    private new void Awake()
    {
        _ExtraWhiteListtags.Add("Enemy");
    }

    //Extends the projectile
    void Update()
    {
        if (!Pause.Paused)
        {
            if (this.GetComponent<Transform>().localScale.y > 0)
            {
                this.GetComponent<Transform>().localScale = new Vector3(this.GetComponent<Transform>().localScale.x, this.GetComponent<Transform>().localScale.y + scale, this.GetComponent<Transform>().localScale.z);
            }
            else
            {
                this.GetComponent<Transform>().localScale = Vector3.zero;
                Destroy(this.gameObject);
            }
            transform.position += (this.transform.up * Mathf.Abs(scale));
        }
    }

    //If the projectile collides with anything not an enemy or projectile, shrink
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.tag != "Projectile" && other.gameObject.tag != "Enemy")
        {
            scale = -Mathf.Abs(scale);
        }
    }

    public void Shoot(GameObject Shooter, GameObject Target, int dmg)
    {        
        _damage = dmg;
        GameObject shotprojectile = Instantiate(Resources.Load(FileDir.L1BossLaserProjectile) as GameObject, Shooter.transform.position + (Target.transform.position - Shooter.transform.position).normalized * Shooter.transform.localScale.x, Shooter.transform.rotation);
        shotprojectile.transform.rotation = Quaternion.LookRotation(Target.transform.position - Shooter.transform.position);
        shotprojectile.transform.Rotate(new Vector3(90f, 0f, 0f));
        shotprojectile.GetComponent<GenericWeapon>().SetDamage(damage);
        shotprojectile.SetActive(true);
    }
}
