using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEnemy : GenericEnemy
{

    public static int Count { get; private set; }
    private bool Triggered;

    protected override void Start()
    {
        isdead = false;
        if (TargetTag != "" && TargetTag != null)
        {
            Target = GameObject.FindGameObjectWithTag(TargetTag);
        }
        this.gameObject.tag = "Enemy";
        thismaterial = this.gameObject.GetComponent<Renderer>().material;
    }

    //Built in Unity procedure that runs when the object cannot be seen by any cameras
    void OnBecameInvisible()
    {
        if (!Triggered)
        {
            Triggered = true;
            Count += 1;
        }
    }

    public static void ResetCount()
    {
        Count = 0;
    }

}
