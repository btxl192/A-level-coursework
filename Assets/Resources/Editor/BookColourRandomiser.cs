using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomiseBookColour)), CanEditMultipleObjects]
public class BookColourRandomiser : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Randomise"))
        {
            foreach (object t in targets)
            {
                ((RandomiseBookColour)t).RandomiseColour();
            }
        }
    }
}
