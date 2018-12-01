using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class LoadGameButton : MonoBehaviour {

    [SerializeField]
    private Transform SaveGamesUI;

    //When the button is clicked, create LoadSaveGameButtons according to the number of save files the player has
    public void OnClick()
    {
        foreach (Transform child in SaveGamesUI)
        {
            Destroy(child.gameObject);
        }
        foreach (string s in Directory.GetDirectories(FileDir.SaveDirectory))
        {
            Instantiate(Resources.Load(FileDir.MainMenuSaveButton) as GameObject, SaveGamesUI).GetComponentInChildren<TextMeshProUGUI>().text = s.Split("/".ToCharArray()[0])[1];
        }
    }
}
