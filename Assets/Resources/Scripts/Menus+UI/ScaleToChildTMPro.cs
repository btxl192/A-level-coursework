using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScaleToChildTMPro : MonoBehaviour {

    private TextMeshProUGUI childText;
    [SerializeField]
    bool ScaleWidth;
    [SerializeField]
    bool ScaleHeight;
    private void Start()
    {
        childText = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    //Update the image's height and width depending on which one is true
    private void Update()
    {
        if (ScaleHeight && ScaleWidth)
        {
            if (this.gameObject.GetComponent<RectTransform>().sizeDelta != childText.gameObject.GetComponent<RectTransform>().sizeDelta)
            {
                this.gameObject.GetComponent<RectTransform>().sizeDelta = childText.gameObject.GetComponent<RectTransform>().sizeDelta;
            }
        }
        else if (ScaleHeight)
        {
            if (this.gameObject.GetComponent<RectTransform>().sizeDelta.y != childText.gameObject.GetComponent<RectTransform>().sizeDelta.y)
            {
                this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.gameObject.GetComponent<RectTransform>().sizeDelta.x, childText.gameObject.GetComponent<RectTransform>().sizeDelta.y);
            }
        }
        else if (ScaleWidth)
        {
            if (this.gameObject.GetComponent<RectTransform>().sizeDelta.x != childText.gameObject.GetComponent<RectTransform>().sizeDelta.x)
            {
                this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(childText.gameObject.GetComponent<RectTransform>().sizeDelta.x, this.gameObject.GetComponent<RectTransform>().sizeDelta.y);
            }
        }
    }
}
