using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Chest), true)]
public class ChestContentsInspector : InteractableDialog {

    public override void OnInspectorGUI()
    {     
        base.OnInspectorGUI();
        Chest thischest = (Chest)target;
        if (!thischest.Contents.Contains(null))
        {
            thischest.Contents.Add(null);
        }
        else
        {
            while (NullCount(thischest.Contents) > 1)
            {
                thischest.Contents.Remove(null);
            }
        }
    }

    int NullCount(List<GameObject> g)
    {
        int count = 0;
        foreach (GameObject go in g)
        {
            if (go == null)
            {
                count += 1;
            }
        }
        return count;
    }
}
