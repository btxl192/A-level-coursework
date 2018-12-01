using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.3.5
public class Slow : StatusBase
{
    protected override float lifetime { get { return 5; } }

    public override string Desc
    {
        get
        {
            return string.Format("Speed decreased by {0}%", (1f / ((float)Level + 1f)) * 100);
        }
    }

    private float defaultspeed;

    //Slow down the object
    protected override void OnStatusApplied()
    {
        if (this.gameObject.GetComponent<GenericEnemy>() != null)
        {
            defaultspeed = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed;
            this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = defaultspeed * (1f / ((float)Level + 1f));
        }
        if (this.gameObject.GetComponent<PlayerBehaviour>() != null)
        {
            defaultspeed = this.gameObject.GetComponent<PlayerMovement>().speed;
            this.gameObject.GetComponent<PlayerMovement>().speed = defaultspeed * (1f / ((float)Level + 1f));
        }
    }

    //Reset the object to its default speed
    protected override void OnStatusEnd()
    {
        if (this.gameObject.GetComponent<GenericEnemy>() != null)
        {
            this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = defaultspeed;
        }
        if (this.gameObject.GetComponent<PlayerBehaviour>() != null)
        {
            this.gameObject.GetComponent<PlayerMovement>().speed = defaultspeed;
        }
    }

    protected override void InitialiseStatusIcon()
    {
        StatusIconReference = Resources.Load(FileDir.SlowStatusIcon) as GameObject;
    }
}
