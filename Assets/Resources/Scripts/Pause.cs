using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.11.1
public class Pause : MonoBehaviour {

	private static bool _Paused;
	public static bool Paused
	{
		get {return _Paused;}
	}

	protected virtual void Start () 
	{
		GenericMenu2.pauser += PauseGame;
		GenericMenu2.unpauser += ResumeGame;
	}

    private void OnDestroy()
    {
        GenericMenu2.pauser -= PauseGame;
        GenericMenu2.unpauser -= ResumeGame;
    }

    public static void ResumeGame() 
	{
		Time.timeScale = 1;
		_Paused = false;
	}

	public static void PauseGame()
	{
		Time.timeScale = 0;
		_Paused = true;
	}

}
