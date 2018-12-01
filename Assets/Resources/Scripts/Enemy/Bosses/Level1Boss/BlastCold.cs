using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastCold : MonoBehaviour {

    private float RotationDirection;
    private GameObject ColdProjectile;
    private GameObject Blaster;
    private Color OriginalBlasterColour;

    private Color BlasterColour
    {
        get
        {
            return Blaster.GetComponent<Renderer>().material.color;
        }
        set
        {
            Blaster.GetComponent<Renderer>().material.color = value;
        }
    }

	void Start ()
    {
        RotationDirection = -90;
        ColdProjectile = Instantiate(Resources.Load(FileDir.L1BossColdProjectile) as GameObject);
        Blaster = transform.Find("Blaster").gameObject;
        OriginalBlasterColour = Color.cyan;
        BlasterColour = Color.black;
    }

    private void OnDestroy()
    {
        Destroy(ColdProjectile);
    }

    //Rotate the arm upwards and fire the cold projectile at 90 degrees.
    void Update ()
    {
        Quaternion rotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(RotationDirection, 0f, 0f), Time.deltaTime * 2.2f);
        transform.localRotation = rotation;
        if (transform.localRotation == Quaternion.Euler(-90f, 0f, 0f))
        {
            Shoot();
            RotationDirection = 0;
            BlasterColour = Color.black;
        }
        if (RotationDirection == -90)
        {
            BlasterColour = OriginalBlasterColour * ((transform.localRotation.eulerAngles.x - 360) / RotationDirection);
        }
        if (transform.localRotation == Quaternion.Euler(0f, 0f, 0f))
        {
            Destroy(this);
        }
    }

    void Shoot()
    {
        ColdProjectile.GetComponent<GenericProjectile>().Shoot(ColdProjectile, Blaster, PlayerSave.staticplayer, ColdProjectile.GetComponent<GenericWeapon>().damage, false, 1000f);
    }
}
