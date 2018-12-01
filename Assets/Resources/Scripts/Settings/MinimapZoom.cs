using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Objective 1.3.2.8.2
public class MinimapZoom : MonoBehaviour {

    public Minimap minimap;

	void Start ()
    {
        minimap.SetZoom(SettingsManager.GetNumberSetting(SettingsManager.NumberSettingsProperties.MinimapZoom));
        this.gameObject.GetComponentInChildren<Text>().text = "Zoom Level: " + minimap.GetZoom();
	}

    //Increase the zoom level
    public void ZoomIn()
    {
        minimap.SetZoom(minimap.GetZoom() + 1);
        this.gameObject.GetComponentInChildren<Text>().text = "Zoom Level: " + minimap.GetZoom();
        SettingsManager.ChangeNumberSetting(SettingsManager.NumberSettingsProperties.MinimapZoom, minimap.GetZoom());
    }

    //Decrease the zoom level
    public void ZoomOut()
    {
        if (minimap.GetZoom() > 1)
        {
            minimap.SetZoom(minimap.GetZoom() - 1);
            this.gameObject.GetComponentInChildren<Text>().text = "Zoom Level: " + minimap.GetZoom();
            SettingsManager.ChangeNumberSetting(SettingsManager.NumberSettingsProperties.MinimapZoom, minimap.GetZoom());
        }
    }
}
