using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogBox), true)]
public class DialogBoxInspector : Editor {

    int index;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DialogBox thisDialog = (DialogBox)target;
        EditorGUILayout.LabelField("Script: ");
        int count = 0;
        foreach (string line in thisDialog.Script)
        {
            count += 1;
            EditorGUILayout.LabelField(count + ":" + line);
        }
    }
}
