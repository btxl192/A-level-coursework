using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMainMenu : MonoBehaviour {

    public void ToMainMenu()
    {
        int lastscene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(Scenes.MainMenu);
        Pause.ResumeGame();
    }
}
