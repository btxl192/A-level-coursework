using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactable), true)]
public class InteractableDialog : Editor {
    string text;
    int index;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Interactable thisInteractable = (Interactable)target;
        if (!thisInteractable.GetDialog().Contains(""))
        { 
            thisInteractable.AddDialog("");
        }
        else
        {
            while (NullCount(thisInteractable.GetDialog()) > 1)
            {
                thisInteractable.RemoveDialog("");
            }
        }
    }

    int NullCount(List<string> g)
    {
        int count = 0;
        foreach (string s in g)
        {
            if (s == "")
            {
                count += 1;
            }
        }
        return count;
    }
}
