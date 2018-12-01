using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Interactable : MonoBehaviour {

	protected bool CanInteract;
	protected GameObject PopupReference;
	protected GameObject Popup;
	protected Vector3 PopupOffset;
	protected bool Interacting;
    [SerializeField]
	protected string PopupText;
    protected bool DialogOpen = false;
    [SerializeField]
    protected List<string> _Dialog = new List<string>();
    public List<string> GetDialog()
    {
        return _Dialog;
    }
    protected bool TriggeredOutOfRange = false;
    [SerializeField]
    public List<string> Dialog
    {
        get
        {
            return _Dialog.Take(_Dialog.Count - 1).ToList() ;
        }
        set
        {
            _Dialog = value;
        }
    }
    [SerializeField]
    protected float InteractRange;

	protected virtual void Awake () 
	{
		try{
		PopupReference = Resources.Load(FileDir.InteractablePopup) as GameObject;
		Popup = Instantiate(PopupReference);
		Popup.SetActive(false);}
		catch{print("ERROR in interactable: couldn't load trade popup");}	
		PopupOffset = new Vector3(0f,20f,0f);
        DialogBox.DialogClosed += DialogClosed;
        if (InteractRange == 0)
        {
            InteractRange = 0.5f;
        }
	}

    protected virtual void OnDestroy()
    {
        DialogBox.DialogClosed -= DialogClosed;
        Destroy(Popup);
    }

    //Handles interacting with an interactable. Also picks the single interactable the player can interact with when multiple are close together
    protected virtual void Update ()
    {       
		try
		{
			if(Vector3.Distance(transform.position, PlayerSave.staticplayer.transform.position) < InteractRange)
			{
                TriggeredOutOfRange = false;
				if(CanInteract)
				{
                    PlayerSave.staticplayer.GetComponent<PlayerBehaviour>().Interacting = this.gameObject;
					CanInteract = false;
				}
				if(PlayerSave.staticplayer.GetComponent<PlayerBehaviour>().Interacting == this.gameObject && !Pause.Paused)
				{
                    if (GenericMenu2.OpenMenu == null && PopupText != "")
                    {
                        Popup.GetComponentInChildren<Text>().text = PopupText + " [" + KeyBindings.KeyBinds["Interact"] + "]";
                        Popup.SetActive(true);
                        Popup.GetComponentsInChildren<RectTransform>()[1].position = Camera.main.WorldToScreenPoint(transform.position) + PopupOffset;
                    }
                    else
                    {
                        Popup.SetActive(false);
                    }

                    if (KeyBindings.KeyPressed("Interact"))
                    {
                        if (!Interacting && GenericMenu2.OpenMenu == null)
                        {
                            Interacting = true;
                            if (Dialog.Count > 0)
                            {
                                DialogBox.SetDialog(Dialog);
                                DialogOpen = true;
                            }
                            else
                            {
                                Interact();
                            }
                        }
                        else if (Interacting)
                        {
                            Interacting = false;
                            UnInteract();
                        }
                    }
                }
				else
				{
                    Popup.SetActive(false);
				}
			}
			else
			{
                Popup.SetActive(false);
				CanInteract = true;
				Interacting = false;
                if (!TriggeredOutOfRange)
                {
                    OutOfRange();
                    TriggeredOutOfRange = true;
                }				
			}
            AdditionalBehaviour();
		}
		catch
        {
            Debug.Log("ERROR in interactable");
            throw;
        }		
	}

    //What happens when the player interacts with the object
    protected virtual void Interact()
    { }

    //What happens when the player stops interacting with the object
    protected virtual void UnInteract()
    { }

    //What happens when the player wanders out of range of the object
    protected virtual void OutOfRange()
    { UnInteract(); }

    //Add a line to the dialog
    public void AddDialog(string s)
    {
        _Dialog.Add(s);
    }

    //Remove a line from the dialog
    public void RemoveDialog(string s)
    {
        _Dialog.Remove(s);
    }

    //What happens after the dialog is closed
    public void DialogClosed()
    {
        if (DialogOpen)
        {
            DialogOpen = false;
            Interact();
        }
    }

    protected virtual void AdditionalBehaviour() { }
}
