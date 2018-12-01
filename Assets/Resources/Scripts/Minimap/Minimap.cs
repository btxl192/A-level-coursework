using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

    [SerializeField]
    private Image _player;    
	private static Vector3 playerposition;
    [SerializeField]
    private static Dictionary<GameObject, List<GameObject>> groundlines = new Dictionary<GameObject, List<GameObject>>();
    private static float ZoomLevel;
    private static Queue<GameObject> GroundCreateQueue = new Queue<GameObject>();
    [SerializeField]
    private static List<MinimapIcon> MiscIcons = new List<MinimapIcon>();

    public delegate void RemovedIconEvent(int index);
    public static event RemovedIconEvent RemovedMiscIcon;

    [SerializeField]
    private static Transform _map;
    public static Transform map
    {
        get
        {
            if (_map == null)
            {
                try
                {
                    _map = GameObject.FindGameObjectWithTag("Minimap").transform;
                }
                catch
                {
                    return null;
                }               
            }
            return _map;
        }
    }

    void Awake () 
    {
        InstPlayerIcon();
        PlayerMovement.sendA += GetAngle;
		PlayerMovement.sendPos += GetPlayerPos;
        Ground.SendPos += UpdateGroundLines;
        SwitchScene.SceneChanged += ClearMap;
        Ground.DeleteFromMinimap += DeleteGroundFromMap;
        GenericMenu2.OnClose += ShowMap;
        GenericMenu2.OnOpen += HideMap;
        ZoomLevel = 10f;       
	}

    private void OnDestroy()
    {
        PlayerMovement.sendA -= GetAngle;
        PlayerMovement.sendPos -= GetPlayerPos;
        Ground.SendPos -= UpdateGroundLines;
        SwitchScene.SceneChanged -= ClearMap;
        Ground.DeleteFromMinimap -= DeleteGroundFromMap;
        GenericMenu2.OnClose -= ShowMap;
        GenericMenu2.OnOpen -= HideMap;
    }

    public float GetZoom()
    {
        return ZoomLevel;
    }

    public void SetZoom(float f)
    {
        ZoomLevel = f;
    }

    //Update the ground lines from the ground queue
    private void Update()
    {
        if (GroundCreateQueue.Count > 0)
        {
            CreateGroundLines(GroundCreateQueue.Dequeue());
        }
    }

    public static void AddGroundLines(GameObject g)
    {
        GroundCreateQueue.Enqueue(g);
    }

    public static bool CheckGroundLines(GameObject g)
    {
        return GroundCreateQueue.Contains(g);
    }

    //Instantiate the player's minimap icon
    void InstPlayerIcon()
    {
        _player = Instantiate((Resources.Load(FileDir.PlayerMinimapIcon) as GameObject).GetComponent<Image>(), transform);
        _player.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        _player.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        _player.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
    }

    //Receives the player's facing angle
    void GetAngle(Quaternion a)
    {
        a.eulerAngles = new Vector3(0f, 0f, -a.eulerAngles.y);
        _player.transform.rotation = a;
    }

    //Receives the player's position
    //Objective 1.3.2.4.1
	void GetPlayerPos(Vector3 p)
	{
		playerposition = p;
	}

    //Creates the outlines of the gruond
    public void CreateGroundLines(GameObject parent)
    {
        groundlines.Add(parent, SameGroundLines());
    }

    //Returns a list of the coloured ground lines that should be displayed when the ground is above the player
    List<GameObject> HighGroundLines()
    {
        List<GameObject> temp = new List<GameObject>();
        for (int a = 0; a < 4; a++)
        {
            temp.Add(Instantiate(Resources.Load(FileDir.HighGroundOutline) as GameObject, map));
            temp[a].transform.transform.SetAsFirstSibling();
        }
        return temp;
    }

    //Returns a list of the coloured ground lines that should be displayed when the ground is on the same level as the player
    List<GameObject> SameGroundLines()
    {
        List<GameObject> temp = new List<GameObject>();
        for (int a = 0; a < 4; a++)
        {
            temp.Add(Instantiate(Resources.Load(FileDir.GroundOutline) as GameObject, map));
            temp[a].transform.transform.SetAsFirstSibling();
        }
        return temp;
    }

    //Returns a list of the coloured ground lines that should be displayed when the ground is below the player
    List<GameObject> LowGroundLines()
    {
        List<GameObject> temp = new List<GameObject>();
        for (int a = 0; a < 4; a++)
        {
            temp.Add(Instantiate(Resources.Load(FileDir.LowGroundOutline) as GameObject, map));
            temp[a].transform.transform.SetAsFirstSibling();
        }
        return temp;
    }

    //Destroys the specified ground lines
    void DestroyGroundLines(List<GameObject> groundlines)
    {
        foreach (GameObject g in groundlines)
        {
            Destroy(g);
        }
    }

    //Updates the position of the ground lines relative to the player
    //Objective 1.3.2.4.1
    public void UpdateGroundLines(GameObject ground, List<Vector3> points, float height)
    {
        try
        { 
            //Objective 1.3.2.4.4
            if (System.Math.Round(ground.GetComponent<Ground>().PrevY,2) != System.Math.Round(ground.transform.position.y - playerposition.y,2))
            {
                DestroyGroundLines(groundlines[ground]);
                if (ground.transform.position.y + 0.5f * height - playerposition.y + 0.1f >= 0.09)
                {
                    groundlines[ground] = HighGroundLines();
                }
                else if (ground.transform.position.y + 0.5f * height - playerposition.y + 0.1f < -0.11)
                {
                    groundlines[ground] = LowGroundLines();
                }
                else
                {
                    groundlines[ground] = SameGroundLines();
                }
            }
            ground.GetComponent<Ground>().PrevY = ground.transform.position.y - playerposition.y;
            for (int a = 0; a < 4; a++)
            {
                GameObject line = groundlines[ground][a];
                line.GetComponent<RectTransform>().anchoredPosition = new Vector3((points[a].x - playerposition.x) * ZoomLevel, (points[a].z - playerposition.z) * ZoomLevel, points[a].z);
                if (a < 2)
                {
                    line.GetComponent<RectTransform>().sizeDelta = new Vector2(0.5f, Mathf.Sqrt((points[2] - points[3]).sqrMagnitude) * ZoomLevel);
                    line.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -ground.transform.rotation.eulerAngles.y);
                }
                else
                {
                    line.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Sqrt((points[0] - points[1]).sqrMagnitude) * ZoomLevel, 0.5f);
                    line.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -ground.transform.rotation.eulerAngles.y);
                }        
            }
        }
        catch { print("MINIMAP: couldn't update lines"); throw; }
    }

    //Create a new minimap icon
    public static void CreateMiscIcon(GameObject Icon, out MinimapIcon ReturnedIcon)
    {
        MinimapIcon icon = new MinimapIcon(Instantiate(Icon, map), -1);
        icon.Icon.transform.SetSiblingIndex(0);
        MiscIcons.Add(icon);
        icon.SetIndex(MiscIcons.IndexOf(icon));
        ReturnedIcon = icon;
    }

    //Update the position of the specified map icon
    public static void UpdateMiscIcon(MinimapIcon MapIcon, Vector3 Location)
    {
        try
        {
            if (map != null)
            {
                int Index = MapIcon.Index;
                //Objective 1.3.2.4.3
                Vector3 Pos = (Location - playerposition) * ZoomLevel;
                Pos.y = Pos.z;
                MiscIcons[Index].Icon.GetComponent<RectTransform>().anchoredPosition = Pos;
            }
        }
        catch
        {
            Debug.Log("Problem loading minimap icon: " + MapIcon.Icon);
        }
    }

    //Remove the specified map icon
    public static void RemoveMiscIcon(MinimapIcon MapIcon)
    {
        int Index = MapIcon.Index;
        MiscIcons[Index].DestroyIcon();
        MiscIcons.RemoveAt(Index);        
        if (RemovedMiscIcon != null)
        {
            RemovedMiscIcon(Index);
        }
    }

    //Clear the minimap
    void ClearMap()
    {
        GroundCreateQueue.Clear();
        foreach (MinimapIcon m in MiscIcons)
        {
            m.DestroyIcon();
        }
        MiscIcons.Clear();
    }

    //Hide the minimap
    public void HideMap(GameObject g)
    {
        foreach (Image i in GetComponentsInChildren<Image>(true))
        {
            i.enabled = false;
        }
    }

    //Show the minimap
    public void ShowMap(GameObject g)
    {
        foreach (Image i in GetComponentsInChildren<Image>(true))
        {
            i.enabled = true;
        }
    }

    //Remove a ground
    void DeleteGroundFromMap(GameObject g)
    {
        foreach (GameObject go in groundlines[g])
        {
            Destroy(go);
        }
        groundlines.Remove(g);
    }
}
