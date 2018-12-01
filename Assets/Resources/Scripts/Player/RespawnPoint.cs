using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPoint : Interactable {

    [SerializeField]
    private static Dictionary<int, Dictionary<string, GameObject>> SpawnPointDict = new Dictionary<int, Dictionary<string, GameObject>>();

    public static Dictionary<int, Dictionary<string, GameObject>> SpawnDict
    {
        get
        {
            return SpawnPointDict;
        }
    }

    private static GameObject FirstSpawn;
    [SerializeField]
    private static GameObject _CurrentSpawnPoint;
    public static GameObject CurrentSpawnPoint
    {
        get
        {
            return _CurrentSpawnPoint;
        }
    }
    private static GameObject RespawnCanvas;
    public string ID
    {
        get
        {
            return string.Format("{0}{1}{2}", transform.position.x, transform.position.y, transform.position.z);
        }
    }
    [SerializeField]
    private bool ForceRespawn;

    public delegate void BlankEvent();
    public static event BlankEvent CheckSpawn;

    protected override void Awake()
    {
        base.Awake();
        if (ForceRespawn)
        {
            PopupText = "";
        }
        //Initialise the spawn points for each scene
        if (SpawnPointDict.Count == 0)
        {
            //https://answers.unity.com/questions/1128694/how-can-i-get-a-list-of-all-scenes-in-the-build.html
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < sceneCount; i++)
            {
                SpawnPointDict.Add(i, new Dictionary<string, GameObject>());
            }
        }
        RespawnCanvas = GameObject.FindGameObjectWithTag("MainUI").transform.Find("RespawnCanvas").gameObject;
        FirstSpawn = GameObject.FindGameObjectWithTag("FirstSpawn");
        CheckSpawn += CheckSpawnPoint;     
        //Add the spawn point to the spawn point dictionary if it does not exist
        if (!SpawnPointDict[SceneManager.GetActiveScene().buildIndex].ContainsKey(ID))
        {
            SpawnPointDict[SceneManager.GetActiveScene().buildIndex].Add(ID, gameObject);
        }
        //Overwrite the spawn point if it does exist
        else
        {
            SpawnPointDict[SceneManager.GetActiveScene().buildIndex][ID] = gameObject;
        }
        if (CurrentSpawnPoint == null)
        {
            _CurrentSpawnPoint = FirstSpawn;
        }
        CheckSpawn();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        CheckSpawn -= CheckSpawnPoint;
    }

    //If the respawn is a force respawn, automatically set the player's spawn if the player is within its radius
    protected override void Update()
    {
        base.Update();
        if (ForceRespawn && Vector3.Distance(PlayerSave.staticplayer.transform.position, transform.position) <= InteractRange)
        {
            SetSpawn(gameObject);
        }
    }

    //Sets the spawn point
    protected override void Interact()
    {
        if (!ForceRespawn)
        {
            SetSpawn(gameObject);
            TempPopup.Show("Spawn point set!", Color.green);
        }
    }

    //Sets _CurrentSpawnPoint then checks all spawn points to change their colours
    public static void SetSpawn(GameObject spawnpoint)
    {
        _CurrentSpawnPoint = spawnpoint;
        CheckSpawn();
    }

    //Respawn the player at the current spawn point
    //Objective 1.3.2.7.7
    public void Respawn()
    {
        if (_CurrentSpawnPoint == null)
        {
            _CurrentSpawnPoint = FirstSpawn;
        }
        PlayerSave.staticplayer.transform.position = _CurrentSpawnPoint.transform.position;
        PlayerSave.staticplayer.GetComponent<PlayerStats>().HP.SetHP(PlayerSave.staticplayer.GetComponent<PlayerStats>().HP.maxhealth);
        PlayerSave.staticplayer.GetComponent<PlayerStats>().MP.SetMP(PlayerSave.staticplayer.GetComponent<PlayerStats>().MP.maxmana);
        PlayerSave.staticplayer.GetComponent<PlayerStats>().SetExp((int)(PlayerSave.staticplayer.GetComponent<PlayerStats>().exp * 0.75f));
        PlayerSave.staticplayer.GetComponent<PlayerMovement>().enabled = true;
    }

    //Changes the colour of the particles according to the current spawn point
    void CheckSpawnPoint()
    {
        try
        {
            var main = transform.Find("Particles").GetComponent<ParticleSystem>().main;
            if (_CurrentSpawnPoint == this.gameObject)
            {
                main.startColor = Color.green;
            }
            else
            {
                main.startColor = Color.gray;
            }
        }
        catch
        {
            Debug.Log("No spawn points in this scene!");
        }
    }

    //Show the respawn button
    public static void ShowRespawnCanvas()
    {
        RespawnCanvas.SetActive(true);
        GenericMenu2.SetOpenMenu(RespawnCanvas);
    }

    //Hide the respawn button
    public void HideRespawnCanvas()
    {
        RespawnCanvas.SetActive(false);
        GenericMenu2.SetOpenMenu(null);
    }
}
