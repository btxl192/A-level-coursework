using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkillCore), true)]
public class SkillCoreInspector : Editor {

    int index = 0;
    int level = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SkillCore thisSK = (SkillCore)target;
        EditorGUILayout.LabelField("Skills: ");       
        for(int a = 0; a < thisSK.SkillIDs.Count; a++)
        {
            EditorGUILayout.LabelField("+" + thisSK.SkillLevels[a] + " " + SkillManager.SkillDict(thisSK.SkillIDs[a]).SkillName);
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.Popup(index, SkillManager.GetSkillNames());
        level = EditorGUILayout.IntField("Level", level);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Add Skill"))
        {
            thisSK.SkillIDs.Add(index);
            thisSK.SkillLevels.Add(level);
        }
        if (GUILayout.Button("Clear Skills"))
        {
            thisSK.SkillIDs.Clear();
            thisSK.SkillLevels.Clear();
        }

    }
}
