using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : Interactable {

    [SerializeField]
    protected int SceneIndex;
    [SerializeField]
    protected Vector3 PlayerPosition;

    public delegate void SceneChangedEvent();
    public static event SceneChangedEvent SceneChanged;

    protected override void Interact()
    {
        //Save the player's details to a temporary file
        PlayerSave.SaveTemp();
        SceneManager.LoadScene(SceneIndex);
        PlayerSave.staticplayer.transform.position = PlayerPosition;
        if (SceneChanged != null)
        {
            SceneChanged();
        }
        //Load the player's details after the scene is loaded
        PlayerSave.staticplayer.AddComponent<LoadPlayer>().Initialise(true);        
    }
}
