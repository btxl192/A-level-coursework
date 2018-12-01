using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalBookReturn : MonoBehaviour {

    [SerializeField]
    private float BookDelay = 30f;
    private float TimeSinceLastBook;
    [SerializeField]
    private int MaxBooks = 5;

    //Instantiates a book every 30 seconds if the maximum books has not been reached
	void Update ()
    {
        TimeSinceLastBook += Time.deltaTime;
        if (TimeSinceLastBook >= BookDelay && transform.childCount < MaxBooks)
        {
            TimeSinceLastBook = 0;
            Instantiate(Resources.Load(FileDir.Bookbait) as GameObject, transform.position + Vector3.up, Quaternion.identity, transform).GetComponent<RandomiseBookColour>().RandomiseColour();
        }
	}
}
