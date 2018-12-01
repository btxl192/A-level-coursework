using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWorldSwitch : Interactable {

    [SerializeField]
    protected GameObject ToSwitch;
    [SerializeField]
    protected bool EnableOrDisable;

    protected override void Interact()
    {
        ToSwitch.SetActive(EnableOrDisable);
    }
}
