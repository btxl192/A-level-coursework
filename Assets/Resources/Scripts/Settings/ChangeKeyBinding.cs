using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Objective 1.3.2.8.3
public class ChangeKeyBinding : MonoBehaviour 
{
    [SerializeField]
    private string _Action;
	public string Action
    {
        get
        {
            return _Action;
        }
        set
        {
            _Action = value;
        }
    }

	void Start () 
	{
		UpdateText ();
        this.gameObject.GetComponent<Button>().onClick.AddListener(GetKey);
    }

	void UpdateText()
	{
		this.GetComponentInChildren<TextMeshProUGUI> ().text = Action + ": " + KeyBindings.KeyBinds [Action];
	}

    //Instatiates the GetKeyPress object and listens for the key being broadcasted
    void GetKey()
    {
		GetKeyPress.InstantiateKeyPress(GetKeyPress.Receivers.KeyBind);
		GetKeyPress.SendKey += BindKey;
    }

    //Bind the key to Action
    void BindKey(string key, GetKeyPress.Receivers r)
    {
        GetKeyPress.Receivers thisReceiver = GetKeyPress.Receivers.KeyBind;
        if (r == thisReceiver)
        {
			try
			{
                if (!KeyBindings.IsKeyBound(key) || key == KeyBindings.KeyBinds[Action])
                {
                    KeyBindings.KeyBinds[Action] = key;
                }
                else
                {
                    TempPopup.Show("Key is already bound!", Color.red);
                }
			}
			catch 
			{
				Debug.Log ("Invalid Action bound");
			}
        }
		GetKeyPress.SendKey -= BindKey;
        UpdateText();
    }
}
