using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusBase : MonoBehaviour {

    public float timelived {get; protected set;}
    protected abstract float lifetime { get; }
    public int Level { get; protected set; }
    [SerializeField]
    protected GameObject StatusIconReference;
    public GameObject Icon
    {
        get
        {
            return StatusIconReference;
        }
    }
    protected GameObject StatusIcon;
    protected bool OnPlayer = false;
    public abstract string Desc { get; }
    public string FullDesc
    {
        get
        {
            return Statuses.StatusString[Statuses.TypeToStatus[this.GetType()]] + ": " + Desc;
        }
    }

    public delegate void StatusEvent(StatusBase s, GameObject objApplied);
    public static event StatusEvent EndStatus;
    public static event StatusEvent StatusApplied;

    protected void Awake()
    {
        InitialiseStatusIcon();
    }

    protected void Start()
    {
        StatusApplied(this, gameObject);
        if (this.gameObject.tag == "Player")
        {
            OnPlayer = true;
            //Objective 1.3.2.3.2
            StatusIcon = Instantiate(StatusIconReference, GameObject.FindGameObjectWithTag("MainUI").transform.Find("Statuses/Layout").transform);
        }       
        OnStatusApplied();
    }

    protected virtual void Update ()
    {
        timelived += Time.deltaTime;
        //Add the status to the statuses UI if the status is on a player
        if (OnPlayer && StatusIcon != null)
        {
            StatusIcon.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Level.ToString() + "\n" + (System.Math.Round(lifetime - timelived, 1)).ToString();
        }       
        //If the status has been alive for at least its lifetime, destroy it
        if (timelived >= lifetime)
        {
            EndStatus(this, gameObject);
            if (OnPlayer)
            {
                Destroy(StatusIcon);
            }            
            OnStatusEnd();
            Destroy(this);
        }
	}

    public void SetLevel(int i)
    {
        Level = i;
        OnLevelChanged();
    }

    //Runs when the status is applied
    protected virtual void OnStatusApplied() { }
    //Runs when the status ends
    protected virtual void OnStatusEnd() { }
    //Runs when the status' level changes
    protected virtual void OnLevelChanged() { }
    //Used to load the status' icon
    protected abstract void InitialiseStatusIcon();

    public float GetLifetime()
    {
        return lifetime;
    }

    public void SetTimeLived(float i)
    {
        timelived = i;
    }
}
