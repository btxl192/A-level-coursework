using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutMessage : MonoBehaviour {

    [SerializeField]
    private string Message;
    [SerializeField]
    private float ShowRadius;
    private UnityEngine.UI.Image bg;
    private bool Showing;
    private GameObject Player;

	void Start ()
    {
        Player = PlayerSave.staticplayer;
        bg = Instantiate(Resources.Load(FileDir.BlackBG) as GameObject).GetComponent<UnityEngine.UI.Image>();
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0);
    }
	
    //Show the message if the player is within the ShowRadius
	void Update ()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) < ShowRadius)
        {
            if (!Showing)
            {
                TutorialMessages.Show(Message, Color.cyan, Color.black);
                Showing = true;
            }
            
        }
        else
        {
            if (Showing)
            {
                TutorialMessages.Hide();
                Showing = false;
            }
        }
	}
}
