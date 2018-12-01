using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.11.5
public class FadeAbovePlayer : MonoBehaviour {

    //Stores the centre of each side of the square
    private List<Vector3> points = new List<Vector3>();
    private bool Fading;
    private Material OriginalMaterial;
    [SerializeField]
    private Material TransparentMaterial;
    //The default Unity3D layer the object is on
    private int DefaultLayer;

    void Start ()
    {
        points.Add(transform.position + (transform.right * 0.5f * transform.localScale.x)); //middle of the right side
        points.Add(transform.position + (-transform.right * 0.5f * transform.localScale.x)); //middle of the left side
        points.Add(transform.position + (transform.forward * 0.5f * transform.localScale.z)); //middle of the top side
        points.Add(transform.position + (-transform.forward * 0.5f * transform.localScale.z)); //middle of the bottom side
        OriginalMaterial = GetComponent<Renderer>().material;
        DefaultLayer = gameObject.layer;
    }
	
   
	void Update ()
    {
        //Check if the player is underneath, or if the object is too high above the player. Fade out if so.
        if (((PlayerSave.staticplayer.transform.position.x < points[0].x && PlayerSave.staticplayer.transform.position.x > points[1].x) && (PlayerSave.staticplayer.transform.position.z < points[2].z && PlayerSave.staticplayer.transform.position.z > points[3].z) && (PlayerSave.staticplayer.transform.position.y < transform.position.y - transform.localScale.y * 0.5f)) || transform.position.y - PlayerSave.staticplayer.transform.position.y  >= 1.5f)
        {
            if (!Fading)
            {
                if (TransparentMaterial != null)
                {
                    GetComponent<Renderer>().material = TransparentMaterial;
                }
                Fading = true;
                gameObject.AddComponent<FadeOut>().Initialise(0.2f, false);
                gameObject.layer = 2;
            }
        }
        else
        {
            if (Fading)
            {
                if (TransparentMaterial != null)
                {
                    GetComponent<Renderer>().material = OriginalMaterial;
                }
                Fading = false;
                gameObject.AddComponent<FadeIn>().Initialise(0.2f);
                gameObject.layer = DefaultLayer;
            }
        }
    }
}
