using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadSaveGameButton : MonoBehaviour {

    //Set the save number in SettingsManager
    public void OnClick()
    {
        SettingsManager.SetSaveNum(PlayerSave.StringToInt(transform.GetComponentInChildren<TextMeshProUGUI>().text.Remove(0, 4)));
    }
}
