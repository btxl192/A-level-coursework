using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenericEnemy), true)]
public class EnemyDropInspector : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("MAXIMUM HEALTH: " + ((GenericEnemy)target).maxhealth.ToString());
        GenericEnemy thisenemy = (GenericEnemy)target;
        if (!thisenemy.GuaranteedDrops.Contains(null))
        {
            thisenemy.GuaranteedDrops.Add(null);
        }
        else
        {
            while (NullCount(thisenemy.GuaranteedDrops) > 1)
            {
                thisenemy.GuaranteedDrops.Remove(null);
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
