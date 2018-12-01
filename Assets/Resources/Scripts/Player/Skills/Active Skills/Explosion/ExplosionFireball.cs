using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFireball : GenericProjectile
{

    public bool isPlayer
    {
        get;
        private set;
    }

    public int Level
    {
         get;
         private set;
    }

    public void SetDetails(bool isplayer, int level, int dmg)
    {
        isPlayer = isplayer;
        Level = level;
        _damage = dmg;
    }

    //When the fireball projectile collides with anything, instantiate an explosion
    protected override void OnCollide(Collider other)
	{
		GameObject EXPLOSION = Instantiate(Resources.Load(FileDir.Sphere) as GameObject);
		EXPLOSION.transform.position = transform.position;
		EXPLOSION.AddComponent<ExplosionBlast>();
        if (isPlayer)
        {
            EXPLOSION.GetComponent<ExplosionBlast>().SetDamage((int)(PlayerSave.staticplayer.GetComponent<PlayerStats>().Combat.damage * (1.5 + 0.05 * (Level - 1))));
        }
        else
        {
            EXPLOSION.GetComponent<ExplosionBlast>().SetDamage((int)(damage * (1.5 + 0.05 * (3))));
        }
    }
}
