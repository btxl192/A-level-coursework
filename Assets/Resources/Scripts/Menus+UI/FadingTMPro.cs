using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadingTMPro : MonoBehaviour {

	private float FadePeriod;
    private bool SetupDone;

    //Reduce the alpha value of the text's colour. Destroy the object when the value hits zero
    void Update () 
	{
        this.gameObject.GetComponent<TextMeshProUGUI>().color = new Color(this.gameObject.GetComponent<TextMeshProUGUI>().color.r, this.gameObject.GetComponent<TextMeshProUGUI>().color.g, this.gameObject.GetComponent<TextMeshProUGUI>().color.b, this.gameObject.GetComponent<TextMeshProUGUI>().color.a - (1 / FadePeriod) * Time.unscaledDeltaTime);
        if (this.gameObject.GetComponent<TextMeshProUGUI>().color.a <= 0)
        {
            Destroy(this.gameObject);
        }
	}

    //Initialise properties
    public void Setup(float fadePeriod)
    {
        FadePeriod = fadePeriod;
    }

    //Static method used to create a new fading text
    public static GameObject InstFadingText(float fadePeriod, GameObject parent)
    {
        GameObject temp = Instantiate(Resources.Load(FileDir.FadingTMPro) as GameObject, parent.transform);
        temp.GetComponent<FadingTMPro>().Setup(fadePeriod);
        return temp;
    }

    //Static method used to create a new fading text in the middle of the screen
    public static GameObject InstFadingTextMid(float fadePeriod, GameObject parent)
    {
        GameObject temp = Instantiate(Resources.Load(FileDir.FadingTMProMid) as GameObject, parent.transform);
        temp.GetComponent<FadingTMPro>().Setup(fadePeriod);
        return temp;
    }
}
