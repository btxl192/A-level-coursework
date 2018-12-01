using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

    public delegate void DeleteFromMinimapEvent(GameObject g);
    public static event DeleteFromMinimapEvent DeleteFromMinimap;

    public delegate void SendPosEvent(GameObject g, List<Vector3> points, float height);
    public static event SendPosEvent SendPos;
    public float PrevY { get; set; }

    private List<Vector3> points = new List<Vector3>();
    private bool SwitchingScenes = false;

    private void Start()
    {
        SwitchScene.SceneChanged += ScenesSwitched;
        points.Add(this.transform.position + (transform.right * 5 * transform.localScale.x * 0.1f)); //middle of the right side
        points.Add(this.transform.position + (-transform.right * 5 * transform.localScale.x * 0.1f)); //middle of the left side
        points.Add(this.transform.position + (transform.forward * 5 * transform.localScale.z * 0.1f)); //middle of the top side
        points.Add(this.transform.position + (-transform.forward * 5 * transform.localScale.z * 0.1f)); //middle of the bottom side
        Minimap.AddGroundLines(this.gameObject);
    }

    //Updates the position of the ground relative to the player
    void Update () 
    {
        if (SendPos != null && !SwitchingScenes && !Minimap.CheckGroundLines(this.gameObject))
        {
            //Objective 1.3.2.4.2
            SendPos(this.gameObject, points, transform.localScale.y);
        }
    }

    //Destroy the object when the scene has been switched
    void ScenesSwitched()
    {
        SwitchingScenes = true;
        if (DeleteFromMinimap != null)
        {
            DeleteFromMinimap(this.gameObject);
        }
    }

    void OnDestroy()
    {
        SwitchScene.SceneChanged -= ScenesSwitched;
    }

}
