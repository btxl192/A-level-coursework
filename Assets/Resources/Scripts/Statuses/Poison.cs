using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.3.4
public class Poison : StatusBase {

    public int damage;
    private float lastUpdate;

    public override string Desc
    {
        get
        {
            return string.Format("Taking {0} damage every second", (int)(damage * (0.25f + 0.2 * (Level - 1))));
        }
    }

    protected override float lifetime{get {return 10; }}

    protected override void InitialiseStatusIcon()
    {
        StatusIconReference = Resources.Load(FileDir.PoisonStatusIcon) as GameObject;
    }

    //Every second, damage the object the status is on
    protected override void Update ()
    {
        base.Update();
        //https://answers.unity.com/questions/134474/add-to-score-every-second.html
        if (Time.time - lastUpdate >= 1f)
        {
            CauseDamage();
            lastUpdate = Time.time;
        }
    }

    //Damages the object the status is on
    void CauseDamage()
    {
        if (gameObject.GetComponent<PlayerStats>() != null)
        {
            GetComponent<PlayerStats>().HP.ChangeHealth(-(int)(damage * (0.25f + 0.2 * (Level - 1))));
        }
        if (gameObject.GetComponent<GenericEnemy>() != null)
        {
            int toxiclv = SkillManager.Skills[0].Level;
            GetComponent<GenericEnemy>().TakeDamage((int)(damage * (0.5f + 0.2 * (Level - 1 + toxiclv))));
        }
    }
}
