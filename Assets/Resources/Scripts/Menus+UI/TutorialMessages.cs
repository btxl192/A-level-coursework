using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Objective 1.3.2.11.6
public class TutorialMessages : MonoBehaviour {

    private static GameObject ThisMessage;
    private static Image image;
    private static Image bg;
    private static TextMeshProUGUI text;

	void Start ()
    {
        ThisMessage = GameObject.FindGameObjectWithTag("TutorialMessage");
        image = ThisMessage.transform.Find("Outline").GetComponent<Image>();
        bg = ThisMessage.transform.Find("bg").GetComponent<Image>();
        text = ThisMessage.GetComponentInChildren<TextMeshProUGUI>();
	}

    //Set the transparency of the text and the background to the image's transparency
    private void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, image.color.a);
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, text.color.a);
    }

    //Show the tutorial message
    public static void Show(string message, Color messageColour, Color outlinecolour)
    {
        outlinecolour.a = 0;
        messageColour.a = 0;
        image.color = outlinecolour;
        text.color = messageColour;
        text.text = message;
        image.gameObject.AddComponent<FadeIn>().Initialise(0.2f);
    }

    //Hide the tutorial message
    public static void Hide()
    {
        image.gameObject.AddComponent<FadeOut>().Initialise(0.2f, false);
    }

}
