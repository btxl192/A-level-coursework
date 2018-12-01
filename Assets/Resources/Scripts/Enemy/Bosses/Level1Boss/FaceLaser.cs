using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceLaser : MonoBehaviour {

    private GameObject FaceProjectile;
    private GameObject Head;
    private float LaserShootTime;
    private float MaxLaserShootTime = 2;

    public delegate void BlankEvent();
    public static event BlankEvent Deactivate;

    private Color HeadColour
    {
        get
        {
            return Head.GetComponent<Renderer>().material.color;
        }
        set
        {
            Head.GetComponent<Renderer>().material.color = value;
        }
    }

    void Start()
    {
        FaceProjectile = Instantiate(Resources.Load(FileDir.L1BossLaserProjectile) as GameObject);
        Head = gameObject;
    }

    private void OnDestroy()
    {
        Destroy(FaceProjectile);
    }

    //Change the colour of the head of the boss; this indicates when the boss is going to fire the laser
    void Update()
    {
        HeadColour = Color.Lerp(HeadColour, Color.magenta, 0.01f);
        if (HeadColour.r >= Color.magenta.r * 0.95f && HeadColour.b >= Color.magenta.b * 0.95f && HeadColour.g < 0.1f)
        {
            LaserShootTime += Time.deltaTime;
            if (LaserShootTime <= MaxLaserShootTime)
            {
                Shoot();
            }
            else
            {
                HeadColour = Color.white;
                Destroy(FaceProjectile);
                if (Deactivate != null)
                {
                    Deactivate();
                }
                Destroy(this);
            }
            
        }
    }

    void Shoot()
    {
        FaceProjectile.GetComponent<FaceLaserProjectile>().Shoot(Head, PlayerSave.staticplayer, FaceProjectile.GetComponent<GenericWeapon>().damage);
    }
}
