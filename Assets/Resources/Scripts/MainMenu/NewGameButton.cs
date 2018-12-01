using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour {

    //Sets NewGame to true and load the tutorial level
    public void StartNewGame()
    {
        LoadPlayer.NewGame = true;
        SceneManager.LoadScene(Scenes.Tutorial);        
    }

}
