using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour {

    public Text thistext { get; set; }
    private int count = 0;

    [SerializeField, HideInInspector]
    private List<string> _Script = new List<string>();
    public List<string> Script
    {
        get
        {
            return _Script;
        }
    }

    public delegate void DialogClosedEvent();
    public static event DialogClosedEvent DialogClosed;

    public delegate void DialogEvent(List<string> script);
    public static event DialogEvent SendDialog;

    //Add a line to the script
    public void AddLine(string s)
    {
        _Script.Add(s);
    }

    //Remove a line from the script
    public void RemoveAt(int i)
    {
        try
        {
            if (i > 0)
            {
                _Script.RemoveAt(i - 1);
            }           
        }
        catch
        {
            Debug.Log("No such line number!");
        }
    }

    void Start()
    {
        DialogBox.SendDialog += SetScript;
    }

    private void OnDestroy()
    {
        DialogBox.SendDialog -= SetScript;
    }

    //Move to the next line in the script
    public void NextLine()
    {
        count = (count + 1) % Script.Count;
        if (count == 0)
        {
            this.gameObject.GetComponent<Image>().enabled = false;
            this.gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
            Pause.ResumeGame();
            DialogClosed();
            GenericMenu2.SetOpenMenu(null);
        }
        else
        {
            thistext.text = Script[count];
        }     
    }

    //Move to the previous line in the script
    public void PrevLine()
    {
        if (count > 0)
        {
            count = (count - 1) % Script.Count;
            thistext.text = Script[count];
        }
    }

    //Sets the script then displays the dialog box
    void SetScript(List<string> script)
    {
        Pause.PauseGame();
        this.gameObject.GetComponent<Image>().enabled = true;
        this.gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
        GenericMenu2.SetOpenMenu(gameObject);
        _Script = script;
        thistext.text = Script[0];
    }

    //Static method to set the dialog box's script
    public static void SetDialog(List<string> script)
    {
        SendDialog(script);
    }
}
