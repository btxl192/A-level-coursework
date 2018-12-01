using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRayHit : MonoBehaviour {

	void Update () {
        Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;
        if (Physics.Raycast(mouse, out rayhit, 100f))
        {
            Debug.Log(rayhit.transform.gameObject.name);
        }
    }
}
