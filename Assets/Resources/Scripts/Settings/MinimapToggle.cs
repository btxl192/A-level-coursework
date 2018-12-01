using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Objective 1.3.2.8.1
public class MinimapToggle : MonoBehaviour {

    public GameObject minimap;

	void Start ()
    {
        minimap.SetActive(SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.MinimapEnabled));
        this.GetComponentInChildren<Text>().text = "Minimap: " + minimap.activeSelf;
        this.GetComponent<Button>().onClick.AddListener(ToggleMinimap);
	}

    //Switches the minimap off or on and update the text on the button
    void ToggleMinimap()
    {
        minimap.SetActive(!minimap.activeSelf);
        this.GetComponentInChildren<Text>().text = "Minimap: " + minimap.activeSelf;
        SettingsManager.ChangeBooleanSetting(SettingsManager.BooleanSettingsProperties.MinimapEnabled, minimap.activeSelf);
    }
}
