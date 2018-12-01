using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyLog : MonoBehaviour {

	void Update ()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            for (int a = 0; a < 3; a++)
            {
                if (Input.GetMouseButtonDown(a))
                {
                    print("(Mouse" + a + ")");
                }
            }          
        }
        else if (Input.anyKey && Input.inputString != "")
        {
            print("(" + Input.inputString + ")");
        }
    }
}
