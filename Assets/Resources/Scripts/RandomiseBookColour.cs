using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomiseBookColour : MonoBehaviour {

    [SerializeField]
    private float alpha;

    public void RandomiseColour()
    {
        GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), alpha);
    }
}
