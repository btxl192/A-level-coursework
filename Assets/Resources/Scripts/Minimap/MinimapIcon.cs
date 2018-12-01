using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon {

    public GameObject Icon { get; private set; }
    public int Index { get; private set; }

    public MinimapIcon(GameObject icon, int index)
    {
        Icon = icon;       
        Index = index;
        Minimap.RemovedMiscIcon += UpdateIndex;
    }

    //Updates the index of the icon depending on which icon has been destroyed
    void UpdateIndex(int RemovedIndex)
    {
        if (RemovedIndex < Index)
        {
            Index -= 1;
        }        
    }

    public void SetIndex(int i)
    {
        Index = i;
    }

    public void DestroyIcon()
    {
        MonoBehaviour.Destroy(Icon);
    }

    //Resizes the icon
    public void ResizeIcon(Vector2 SizeDelta)
    {
        Icon.GetComponent<RectTransform>().sizeDelta = SizeDelta;
    }
}
