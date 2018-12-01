using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempPopup : MonoBehaviour {

    public static Transform ThisPopup;

    public void Awake()
    {
        ThisPopup = GameObject.FindGameObjectWithTag("TempPopup").transform;
    }

    //Instantiate the text
    public static void Show(string message, Color messageColour)
    {
        try
        {
            ThisPopup.GetComponent<Mask>().showMaskGraphic = false;
            GameObject temp = FadingText.InstFadingTextMid(2f, ThisPopup.gameObject);
            temp.GetComponent<Text>().text = message;
            temp.GetComponent<Text>().color = messageColour;
            temp.GetComponent<Text>().fontSize = 16;
            temp.GetComponent<RectTransform>().sizeDelta = new Vector2(ThisPopup.GetComponent<RectTransform>().sizeDelta.x, ThisPopup.GetComponent<RectTransform>().sizeDelta.y);
        }
        catch
        {
            Debug.Log("COULD NOT DISPLAY: " + message);
        }
    }

    //Overloaded method to customise the background image
    public static void Show(string message, Color messageColour, Image bgimage)
    {
        ThisPopup.GetComponent<Image>().sprite = bgimage.sprite;
        ThisPopup.GetComponent<Image>().color = bgimage.color;
        ThisPopup.GetComponent<Mask>().showMaskGraphic = true;
        GameObject temp = FadingText.InstFadingTextMid(2f, ThisPopup.gameObject);
        temp.GetComponent<Text>().text = message;
        temp.GetComponent<Text>().color = messageColour;
        temp.GetComponent<Text>().fontSize = 16;
        temp.GetComponent<RectTransform>().sizeDelta = new Vector2(ThisPopup.GetComponent<RectTransform>().sizeDelta.x, ThisPopup.GetComponent<RectTransform>().sizeDelta.y);
    }

    //Controls when to show the popup
    private void Update()
    {
        if (ThisPopup.GetComponent<Mask>().showMaskGraphic)
        {
            this.gameObject.GetComponent<Image>().color = new Color(this.gameObject.GetComponent<Image>().color.r, this.gameObject.GetComponent<Image>().color.g, this.gameObject.GetComponent<Image>().color.b, this.gameObject.GetComponent<Image>().color.a - (1 / 2f) * Time.unscaledDeltaTime);
            if (this.gameObject.GetComponent<Image>().color.a <= 0)
            {
                ThisPopup.GetComponent<Mask>().showMaskGraphic = false;
            }
        }
    }
}
