using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweep : MonoBehaviour {

    private float RotationDirection;
    private GameObject mainboss;
    private float Sweeptime = 3f;
    private float CurrentSweeptime;

    void Start()
    {
        RotationDirection = -90;
    }

    public void SetMainBoss(GameObject g)
    {
        mainboss = g;
    }

    //Rotate the arm until it is 90 degrees. After that, sweep the arm around the map. 
    void Update()
    {
        if (transform.localRotation == Quaternion.Euler(-90f, 0f, 0f))
        {
            CurrentSweeptime += Time.deltaTime;
            transform.GetChild(0).transform.localScale = new Vector3(transform.GetChild(0).transform.localScale.x, 3f, transform.GetChild(0).transform.localScale.z);
            GetComponent<CapsuleCollider>().height = 8f;
            mainboss.transform.rotation = Quaternion.Slerp(mainboss.transform.localRotation, Quaternion.Euler(0f, mainboss.transform.rotation.eulerAngles.y + 90, 0f), Time.deltaTime * 1.5f);
            if (CurrentSweeptime >= Sweeptime)
            {
                transform.localRotation = Quaternion.identity;
                transform.GetChild(0).transform.localScale = new Vector3(transform.GetChild(0).transform.localScale.x, 1f, transform.GetChild(0).transform.localScale.z);
                GetComponent<CapsuleCollider>().height = 4f;
                Destroy(this);
            }
        }
        else
        {
            Quaternion rotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(RotationDirection, 0f, 0f), Time.deltaTime * 2.2f);
            transform.localRotation = rotation;
        }
        if (transform.localRotation == Quaternion.Euler(0f, 0f, 0f))
        {
            Destroy(this);
        }
    }

    //Damage the player on contact
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSave.staticplayer.GetComponent<PlayerBehaviour>().takedamage(20);
        }
    }
}
