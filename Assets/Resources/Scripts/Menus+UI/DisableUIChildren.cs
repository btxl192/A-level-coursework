using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUIChildren : MonoBehaviour {

	void Start () 
	{
		GenericMenu2.OnOpen += Enable;
		GenericMenu2.OnClose += Disable;
	}

    private void OnDestroy()
    {
        GenericMenu2.OnOpen -= Enable;
        GenericMenu2.OnClose -= Disable;
    }

    //Goes through each child UI element and enables them
    void Enable(GameObject g)
	{
		for (int a = 1; a < this.GetComponentsInChildren<RectTransform>(true).Length; a++)
		{
            this.GetComponentsInChildren<RectTransform>(true)[a].gameObject.SetActive(true);
		}
	}

    //Goes through each child UI element and disables them
    void Disable(GameObject g)
	{
		for (int a = 1; a < this.GetComponentsInChildren<RectTransform>(true).Length; a++)
		{
			this.GetComponentsInChildren<RectTransform>(true)[a].gameObject.SetActive(false);
		}
	}

    //Goes through each child UI element and toggles them
	void Switch()
	{
		for (int a = 1; a < this.GetComponentsInChildren<RectTransform>(true).Length; a++)
		{
			this.GetComponentsInChildren<RectTransform>(true)[a].gameObject.SetActive(!this.GetComponentsInChildren<RectTransform>(true)[a].gameObject.activeSelf);
		}
	}
}
