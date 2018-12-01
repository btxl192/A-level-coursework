using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKeyPress : MonoBehaviour {

    //Enum to store the objects that should receive the key pressed
	public enum Receivers
	{
		SkillButton,
		KeyBind
	};

    private Receivers Receiver;

	public delegate void SendKeyEvent(string key, Receivers r);
    public static event SendKeyEvent SendKey;

    //Check for key presses every frame while the object exists
	void Update ()
    {
        //If any of the mouse buttons are pressed
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            string key = "";
            for (int a = 0; a < 3; a++)
            {
                if (Input.GetMouseButtonDown(a))
                {
                    key = "Mouse" + a;
                }
            }
            try
            {
                SendKey(key, Receiver);
                Destroy(this.gameObject);
            }
            catch
            {
                Debug.Log("ERROR in GetKeyPress: nothing listening!");
            }
        }
        else if (Input.anyKeyDown)
        {
            try
            {
                SendKey(Input.inputString.ToUpper(), Receiver);
            }
            catch
            {
                Debug.Log("ERROR in GetKeyPress: nothing listening!");
            }
            Destroy(this.gameObject);
        }	
	}

    void SetReceiver(Receivers r)
    {
        Receiver = r;
    }

	public static void InstantiateKeyPress(Receivers r)
	{
		Instantiate(Resources.Load(FileDir.KeyPress) as GameObject, GameObject.FindGameObjectWithTag("MainUI").transform).GetComponent<GetKeyPress>().SetReceiver(r);
	}
}
