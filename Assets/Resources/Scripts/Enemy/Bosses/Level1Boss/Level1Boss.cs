using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level1Boss : Boss {

    [SerializeField]
    private GameObject Head;
    [SerializeField]
    private GameObject LeftArm;
    [SerializeField]
    private GameObject RightArm;
    [SerializeField]
    private GameObject LeftLeg;
    [SerializeField]
    private GameObject RightLeg;

    private float TimeSinceColdShoot;
    private float TimeSinceExplosionShoot;
    private float TimeSinceLaserShoot;
    private bool shotcold;
    private bool shotexplosion;
    private float ColdShoot;
    private float ExplosionShoot;
    private float LaserCooldown = 5f;
    private float SystemCooldown = 5f;
    private float SystemCooldownCount;

    private void Awake()
    {
        ColdShoot = Random.Range(2, 6);
        ExplosionShoot = Random.Range(4, 10);
        FaceLaser.Deactivate += Deactivate;
        firstmeet = true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        FaceLaser.Deactivate -= Deactivate;
    }

    //Aim for the player while the boss is aggroed
    protected override void WhileAggroed()
    {
        base.WhileAggroed();
        TimeSinceColdShoot += Time.deltaTime;
        TimeSinceExplosionShoot += Time.deltaTime;
        TimeSinceLaserShoot += Time.deltaTime;
        if (Vector3.Distance(PlayerSave.staticplayer.transform.position, transform.position) <= 3)
        {
            gameObject.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        }

        if (TimeSinceColdShoot >= ColdShoot)
        {
            if (!shotcold && LeftArm.GetComponent<BlastCold>() == null)
            {
                LeftArm.AddComponent<BlastCold>();
                shotcold = true;
            }
            if (LeftArm.GetComponent<BlastCold>() == null)
            {
                ColdShoot = Random.Range(2, 6);
                TimeSinceColdShoot = 0;
                shotcold = false;
            }
        }

        if (TimeSinceExplosionShoot >= ExplosionShoot)
        {
            if (!shotexplosion && RightArm.GetComponent<ShootFireball>() == null)
            {
                RightArm.AddComponent<ShootFireball>();
                shotexplosion = true;
            }
            if (RightArm.GetComponent<ShootFireball>() == null)
            {
                ExplosionShoot = Random.Range(4, 10);
                TimeSinceExplosionShoot = 0;
                shotexplosion = false;
            }
        }

        if (TimeSinceLaserShoot >= LaserCooldown)
        {
            Head.AddComponent<FaceLaser>();
            TimeSinceLaserShoot = 0;
        }
    }

    //Don't move if the boss is not aggroed
    protected override void WhileNotAggroed()
    {
        SystemCooldownCount += Time.deltaTime;
        gameObject.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        if (Head.GetComponent<FaceLaser>() != null)
        {
            Destroy(Head.GetComponent<FaceLaser>());
        }
        if (SystemCooldownCount >= SystemCooldown)
        {
            Activated = true;
            LeftArm.transform.Find("Arm").GetComponent<Renderer>().material.color = Color.white;
            RightArm.transform.Find("Arm").GetComponent<Renderer>().material.color = Color.white;
            Head.GetComponent<Renderer>().material.color = Color.white;
            SystemCooldownCount = 0;
        }
    }

    //Changes the colour of the boss and set Activated to false
    public void Deactivate()
    {
        Activated = false;
        LeftArm.transform.Find("Arm").GetComponent<Renderer>().material.color = Color.black;
        RightArm.transform.Find("Arm").GetComponent<Renderer>().material.color = Color.black;
        Head.GetComponent<Renderer>().material.color = Color.black;
    }
}
