using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class LoadAllSaves : MonoBehaviour {

    [SerializeField]
    private static Transform SaveGamesUI;

    private void Awake()
    {
        SaveGamesUI = transform.GetComponent<RectTransform>();
    }

    //Clears the UI and displays each save file on the canvas
    public static void ShowSaves()
    {
        foreach (Transform child in SaveGamesUI)
        {
            Destroy(child.gameObject);
        }
        foreach (string s in Directory.GetDirectories(FileDir.SaveDirectory))
        {
            GameObject temp = Instantiate(Resources.Load(FileDir.OverwriteSaveButton) as GameObject, SaveGamesUI);
            temp.transform.Find("SaveName").GetComponent<TextMeshProUGUI>().text = s.Split("/".ToCharArray()[0])[1];
            try
            {
                StreamReader reader = new StreamReader(s + "/SaveTime.txt");
                temp.transform.Find("SaveTime").GetComponent<TextMeshProUGUI>().text = reader.ReadLine();
                reader.Close();
            }
            catch
            {
                temp.transform.Find("SaveTime").GetComponent<TextMeshProUGUI>().text = "Never saved";
            }
        }
    }

    //Non static method, because Unity's built in event system cannot access static methods
    public void ShowSaves2()
    {
        SetPosition();
        foreach (Transform child in SaveGamesUI)
        {
            Destroy(child.gameObject);
        }
        foreach (string s in Directory.GetDirectories(FileDir.SaveDirectory))
        {
            GameObject temp = Instantiate(Resources.Load(FileDir.OverwriteSaveButton) as GameObject, SaveGamesUI);
            temp.transform.Find("SaveName").GetComponent<TextMeshProUGUI>().text = s.Split("/".ToCharArray()[0])[1];
            try
            {
                StreamReader reader = new StreamReader(s + "/SaveTime.txt");
                temp.transform.Find("SaveTime").GetComponent<TextMeshProUGUI>().text = reader.ReadLine();
                reader.Close();
            }
            catch
            {
                temp.transform.Find("SaveTime").GetComponent<TextMeshProUGUI>().text = "Never saved";
            }           
        }
    }

    void SetPosition()
    {
        this.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        this.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
    }
}
