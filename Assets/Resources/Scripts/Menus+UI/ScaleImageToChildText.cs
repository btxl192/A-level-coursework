using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleImageToChildText : MonoBehaviour {

    private Text childText;

    void Awake()
    {
        childText = this.GetComponentInChildren<Text>();
    }

    //Resize the image
	void Update ()
    {
        if (GetComponent<RectTransform>().sizeDelta != new Vector2(LayoutUtility.GetPreferredWidth(childText.GetComponent<RectTransform>()), LayoutUtility.GetPreferredHeight(childText.GetComponent<RectTransform>())))
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(LayoutUtility.GetPreferredWidth(childText.GetComponent<RectTransform>()), LayoutUtility.GetPreferredHeight(childText.GetComponent<RectTransform>()));
        }
	}
}
