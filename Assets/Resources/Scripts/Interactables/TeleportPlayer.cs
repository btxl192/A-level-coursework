using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : Interactable {

    [SerializeField]
    private Vector3 Position;

    //Move the player after interacting
    protected override void Interact()
    {
        base.Interact();
        PlayerSave.staticplayer.transform.position = Position;
    }
}
