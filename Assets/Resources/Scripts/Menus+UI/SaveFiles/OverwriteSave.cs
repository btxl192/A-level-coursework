using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverwriteSave : MonoBehaviour {

    //Overwrite the save file according to the index of the save game button
    public void SetSave()
    {
        int index = transform.parent.Find("SaveName").GetComponent<TextMeshProUGUI>().text[transform.parent.Find("SaveName").GetComponent<TextMeshProUGUI>().text.Length - 1] - 48;
        SettingsManager.SetSaveNum(index);
    }

    public void RefreshSavesUI()
    {
        LoadAllSaves.ShowSaves();
    }
}
