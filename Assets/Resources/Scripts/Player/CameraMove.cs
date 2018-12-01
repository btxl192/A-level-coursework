using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Objective 1.3.2.7.2
public class CameraMove : MonoBehaviour {

	private Vector3 offset;
    private float Height;
    [SerializeField]
    private float HeightOffset;

	void Start () 
	{
        Height = HeightOffset;
		offset = new Vector3(0f,3f,-2.6f);
	}

	void FixedUpdate () 
	{
        //every frame, the position of the camera is changed to maintain the same offset to the player   
        Vector3 pos = PlayerSave.staticplayer.transform.position + offset;   
        //Only change the camera's position if the player can jump, to reduce camera movement
        if(PlayerSave.staticplayer.GetComponent<PlayerMovement>().canjump)
        {
            Height = HeightOffset + PlayerSave.staticplayer.transform.position.y;
        }
        pos.y = Height;
        transform.position = Vector3.Lerp(transform.position, pos, 0.1f);
    }
}
