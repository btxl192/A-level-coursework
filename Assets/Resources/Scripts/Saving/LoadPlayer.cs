using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayer : MonoBehaviour {

    private bool Loaded;
    [SerializeField]
    private static bool temp;
    [SerializeField]
    private bool initialised;
    public static bool NewGame { get; set; }
    [SerializeField]
    private bool SetPlayerSaveLoaded;

    public delegate void BlankEvent();
    public static event BlankEvent SaveGame;

    private void Awake()
    {
        PlayerSave.SetLoaded(SetPlayerSaveLoaded);
    }

    //Whether to load from the temporary files or not
    public void Initialise(bool Temp)
    {
        temp = Temp;
        initialised = true;
    }

    //Load the player
	void Update ()
    {
        try
        {
            if (!Loaded && initialised)
            {                
                Loaded = true;
                if (!NewGame)
                {
                    if (temp)
                    {
                        PlayerSave.LoadTemp();
                        temp = false;
                    }
                    else
                    {
                        PlayerSave.LoadGame();
                    }
                }
                else
                {
                    System.IO.Directory.CreateDirectory(FileDir.SaveFile);                    
                    PlayerStats PStats = PlayerSave.staticplayer.GetComponent<PlayerStats>();
                    PStats.gameObject.transform.position = new Vector3(-2f, 1f, -2f);
                    PStats.HP.SetHP(PStats.HP.maxhealth);
                    PStats.MP.SetMP(PStats.MP.maxmana);
                    PStats.SetLevel(1);
                    NewGame = false;
                    if (SaveGame != null)
                    {
                        SaveGame();
                    }
                }
                Destroy(this);
            }
        }
        catch
        {
            Debug.Log("Could not load save " + SettingsManager.SaveNum);
            throw;
        }
    }
}
