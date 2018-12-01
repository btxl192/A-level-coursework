using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestButton : MonoBehaviour {

    public delegate void ButtonPressedEvent(int i, GameObject chest);
    public static event ButtonPressedEvent ButtonPressed;

    public int Index { get; private set; }
    public GameObject ChestObject { get; private set; }

    //Set the index of the button
    public void SetIndex(int i, GameObject chest)
    {
        Index = i;
        ChestObject = chest;
    }

    //When the button is clicked, broadcast that the button was clicked, along with its index and which chest it belongs to
    public void OnClick()
    {
        if (ButtonPressed != null)
        {
            ButtonPressed(Index, ChestObject);
        }        
    }

}
