using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class TextMeshProButton {

    [MenuItem("GameObject/UI/TextMeshPro - Button")]
    private static void CreateButton()
    {
        MonoBehaviour.Instantiate(Resources.Load("Prefabs/UI/TMProButton") as GameObject, Selection.activeTransform).name = "TMProButton";
    }
}
