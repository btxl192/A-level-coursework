using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingText : MonoBehaviour {

	private float FadePeriod;
    private bool SetupDone;

    //Reduce the alpha value of the text's colour. Destroy the object when the value hits zero
    void Update () 
	{
        if (SetupDone)
        {
            this.gameObject.GetComponent<Text>().color = new Color(this.gameObject.GetComponent<Text>().color.r, this.gameObject.GetComponent<Text>().color.g, this.gameObject.GetComponent<Text>().color.b, this.gameObject.GetComponent<Text>().color.a - (1 / FadePeriod) * Time.unscaledDeltaTime);
            if (this.gameObject.GetComponent<Text>().color.a <= 0)
            {
                Destroy(this.gameObject);
            }
        }
	}

    //Initialise properties
    void Setup(float fadePeriod)
    {
        FadePeriod = fadePeriod;
        this.gameObject.GetComponent<RectTransform>().position = new Vector3(this.gameObject.GetComponent<RectTransform>().position.x, this.gameObject.GetComponent<RectTransform>().position.y - this.gameObject.GetComponent<RectTransform>().sizeDelta.y, this.gameObject.GetComponent<RectTransform>().position.z);
        SetupDone = true;
    }

    //Static method used to create a new fading text
    public static GameObject InstFadingText(float fadePeriod, GameObject parent)
    {
        GameObject temp = Instantiate(Resources.Load(FileDir.FadingText) as GameObject, parent.transform);
        temp.GetComponent<FadingText>().Setup(fadePeriod);
        return temp;
    }

    //Static method used to create a new fading text in the middle of the screen
    public static GameObject InstFadingTextMid(float fadePeriod, GameObject parent)
    {
        GameObject temp = Instantiate(Resources.Load(FileDir.FadingTextMid) as GameObject, parent.transform);
        temp.GetComponent<FadingText>().Setup(fadePeriod);
        return temp;
    }
}
