using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenericWeapon), true)]
public class WeaponStatusInspector : Editor {

    List<Statuses.StatusType> StatusEffects = new List<Statuses.StatusType>();
    int level = 0;
    Statuses.statuses index;
    public override void OnInspectorGUI()
    { 
        base.OnInspectorGUI();
        GenericWeapon wpn = (GenericWeapon)target;
        if (wpn.StatusEffects.Count == 0)
        {
            EditorGUILayout.LabelField("Statuses: None");
        }
        else
        {
            EditorGUILayout.LabelField("Statuses: ");
            foreach (Statuses.StatusType s in wpn.StatusEffects)
            {
                EditorGUILayout.LabelField(" - " + Statuses.StatusString[s.Type] + ", Level:" + s.Level);
            }
        }
        EditorGUILayout.LabelField("=================");
        EditorGUILayout.LabelField("ADD STATUS:");
        EditorGUILayout.LabelField("=================");
        index = (Statuses.statuses)EditorGUILayout.EnumPopup("Status type:", index);        
        level = EditorGUILayout.IntField("Status level: ", level);
        if (GUILayout.Button("Add Status"))
        {
            StatusEffects.Add(Statuses.MakeStatus(index, level));
            wpn.SetStatuses(StatusEffects);
        }
        if (GUILayout.Button("Clear Statuses"))
        {
            ResetStatuses();
            wpn.SetStatuses(StatusEffects);
        }
        
    }

    void ResetStatuses()
    {
        StatusEffects = new List<Statuses.StatusType>();
    }
}
