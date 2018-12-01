using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour {

    public static List<GameObject> Beacons = new List<GameObject>();
    public static Dictionary<float, GameObject> BeaconsDist = new Dictionary<float, GameObject>();
    public Color MaterialColour
    {
        get
        {
            return GetComponent<Renderer>().material.color;
        }
        set
        {
            GetComponent<Renderer>().material.color = value;
        }
    }

    private const int NumBeacons = 3;
    private bool StartExplosion;
    private Color FinalColour;

	void Start ()
    {
        MaterialColour = new Color(MaterialColour.r, MaterialColour.g, MaterialColour.b, 0);
        Beacons.Add(gameObject);
        GetComponent<Renderer>().enabled = false;
	}

    private void OnDestroy()
    {
        Beacons.Remove(gameObject);
    }

    private void Update()
    {
        //If an explosion should be started at this beacon, increase the beacon colour's alpha value to indicate where the explosion will be
        if (StartExplosion)
        {
            MaterialColour = Color.Lerp(MaterialColour, FinalColour, 0.05f);
        }      
        //Once the alpha value is large enough, instantiate the explosion
        if (MaterialColour.a >= 0.95)
        {
            Instantiate(Resources.Load(FileDir.Sphere) as GameObject, transform.position, Quaternion.identity).AddComponent<ExplosionBlast>().SetDetails(30, true);
            GetComponent<Renderer>().enabled = false;
            MaterialColour = new Color(MaterialColour.r, MaterialColour.g, MaterialColour.b, 0);
            StartExplosion = false;
        }
    }

    //Algorithm to get the beacons closest to the player
    public static GameObject[] BeaconsClosestToPlayer()
    {
        BeaconsDist.Clear();
        List<float> distances = new List<float>();
        for (int a = 0; a < NumBeacons; a++)
        {
            float dist = Vector3.Distance(Beacons[a].transform.position, PlayerSave.staticplayer.transform.position);
            BeaconsDist.Add(dist, Beacons[a]);
            distances.Add(dist);
        }
        distances.Sort();
        for (int a = NumBeacons; a < Beacons.Count; a++)
        {
            float dist = Vector3.Distance(Beacons[a].transform.position, PlayerSave.staticplayer.transform.position);
            if (dist < distances[distances.Count - 1])
            {
                BeaconsDist.Remove(distances[distances.Count - 1]);
                distances[distances.Count - 1] = dist;
                distances.Sort();
                BeaconsDist.Add(dist, Beacons[a]);
            }
        }
        List<GameObject> returnedbeacons = new List<GameObject>();
        foreach (float dist in distances)
        {
            returnedbeacons.Add(BeaconsDist[dist]);
        }
        return returnedbeacons.ToArray();
    }

    public void Activate(Color c)
    {
        MaterialColour = new Color(MaterialColour.r, MaterialColour.g, MaterialColour.b, 0);
        GetComponent<Renderer>().enabled = true;
        FinalColour = c;
        StartExplosion = true;
    }
}
