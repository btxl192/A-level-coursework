using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Boss : Boss {

    private float DefenceLoweredTime = 10f;
    private float TimeSinceDefenceLowered;
    private bool DefenceLowered;

    private float SweepWaitTime = 8f;
    private float TimeSinceSweep;

    private float AOEwaittime = 15f;
    private float TimeSinceAOE;

    [SerializeField]
    private GameObject RightArm;

    [SerializeField]
    private ParticleSystem RightEye;
    [SerializeField]
    private ParticleSystem LeftEye;
    [SerializeField]
    private ParticleSystem DeathParticles;

    //Change the colour of the sky when the boss is aggroed
    protected override void OnFirstAggro()
    {
        base.OnFirstAggro();
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", 3.5f);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", 1f);
        Instantiate(DeathParticles, transform.position, Quaternion.identity);
        WorldFlags.Flags["SpectreDefeated"] = true;
        GameObject gzText = Instantiate(Resources.Load(FileDir.TextMeshPro) as GameObject, GameObject.FindGameObjectWithTag("MainUI").transform);
        gzText.GetComponent<TMPro.TextMeshProUGUI>().text = "THAT'S IT FOR THE GAME. THANKS FOR PLAYING.";
        gzText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 100f);
    }

    //Use its attacks whenever it can
    protected override void WhileAggroed()
    {
        if (RightArm.GetComponent<Sweep>() == null)
        {
            base.WhileAggroed();
        }       
        TimeSinceSweep += Time.deltaTime;
        TimeSinceAOE += Time.deltaTime;
        //If the player wanders too far, pull the player back and damage them
        if (Vector3.Distance(PlayerSave.staticplayer.transform.position, transform.position) >= 8f)
        {
            PlayerSave.staticplayer.GetComponent<Rigidbody>().AddForce((transform.position - PlayerSave.staticplayer.transform.position) * 50f);
            PlayerSave.staticplayer.GetComponent<PlayerBehaviour>().takedamage(20);
        }
        if (TimeSinceSweep >= SweepWaitTime && RightArm.GetComponent<Sweep>() == null)
        {
            TimeSinceSweep = 0;
            RightArm.AddComponent<Sweep>().SetMainBoss(gameObject);
        }
        if(DefenceLowered)
        {
            TimeSinceDefenceLowered += Time.deltaTime;
            if (TimeSinceDefenceLowered >= DefenceLoweredTime)
            {
                DefenceLowered = false;
                SetEyeColour(Color.red);
            }
        }
        if (TimeSinceAOE >= AOEwaittime)
        {
            TimeSinceAOE = 0;
            foreach (GameObject g in Beacon.BeaconsClosestToPlayer())
            {
                g.GetComponent<Beacon>().Activate(Color.red);
            }
        }
    }

    //Take damage from projectiles. Take double the damage if the defence is lowered
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile") && !other.GetComponent<GenericWeapon>().WhiteListed(this.gameObject))
        {
            Instantiate(Resources.Load(FileDir.EnemyHitParticles) as GameObject, transform.position, transform.rotation, transform);
            GenericWeapon weapon = other.gameObject.GetComponent<GenericWeapon>();
            int damagetaken = weapon.damage;
            if (!Aggroed)
            {
                damagetaken = (int)(damagetaken * (1 + ((SneakAttack)SkillManager.Skills[16]).DamageIncrease));
            }
            if (DefenceLowered)
            {
                damagetaken *= 2;
            }
            TakeDamage(damagetaken);
            if (weapon.isPlayerWeapon)
            {
                Aggroed = true;
                CallDamagedByPlayer(damagetaken);
            }
        }
    }

    //If a book collides with the boss, lower the boss' defence
    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if (other.gameObject.GetComponent<BookBait>() != null)
        {
            DefenceLowered = true;
            SetEyeColour(Color.green);
        }
    }

    private void SetEyeColour(Color c)
    {
        var main1 = RightEye.GetComponent<ParticleSystem>().main;
        var main2 = LeftEye.GetComponent<ParticleSystem>().main;
        main1.startColor = c;
        main2.startColor = c;
    }

    protected override void SetDrops()
    {
        DropTable = new List<EnemyItemDrop>()
        {
            new EnemyItemDrop(Resources.Load(FileDir.RandomWeaponPickup) as GameObject,2),
            new EnemyItemDrop(Resources.Load(FileDir.RandomArmourPickup) as GameObject,1),
        };
        SetItemSkews();
    }
}
