using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCmds : MonoBehaviour
{

    private void Start()
    {
        foreach (string s in CheatInput.CommandDescriptions.Keys)
        {
            Instantiate(Resources.Load(FileDir.Text) as GameObject, this.gameObject.transform).GetComponent<Text>().text = s + ":" + CheatInput.CommandDescriptions[s];
        }
    }

}
