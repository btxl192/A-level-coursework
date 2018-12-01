using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOpenClose : MonoBehaviour
{
    private Canvas thisCanvas;

	void Start ()
    {
        thisCanvas = this.gameObject.GetComponent<Canvas>();
        GenericMenu2.OnOpen += CloseCanvas;
	}

    void CloseCanvas(GameObject g)
    {
        thisCanvas.enabled = false;
    }

    private void OnDestroy()
    {
        GenericMenu2.OnOpen -= CloseCanvas;
    }
}
