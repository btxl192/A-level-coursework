using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class DeleteSave : MonoBehaviour {

    //Deletes the save file according to the save game button
    public void DelSave()
    {
        int index = transform.parent.Find("SaveName").GetComponent<TextMeshProUGUI>().text[transform.parent.Find("SaveName").GetComponent<TextMeshProUGUI>().text.Length - 1] - 48;
        string dir = FileDir.SaveDirectory + "Save" + index;
        foreach (string s in Directory.GetFiles(dir))
        {
            File.Delete(s);
        }
        Directory.Delete(dir);
        LoadAllSaves.ShowSaves();
    }
}
