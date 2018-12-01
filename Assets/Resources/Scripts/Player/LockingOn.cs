using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Objective 1.3.2.7.6
public class LockingOn : MonoBehaviour {
	
    [SerializeField]
	private Image lockedontarget;

	private Canvas mainui;
	private Image _lockedontarget;

	private PlayerBehaviour Player;

	void Start () 
	{
		Player = PlayerSave.staticplayer.GetComponent<PlayerBehaviour>();
		mainui = GameObject.FindGameObjectWithTag ("MainUI").GetComponent<Canvas>();
        GenericMenu2.OnOpen += HideReticle;
        GenericMenu2.OnClose += ShowReticle;

	}

	void Update () 
	{
        if (!Pause.Paused)
        {
            //If the player is locked on to an enemy, set the position of the target to the enemy's position
            if (Player.lockedon != null)
            {
                if (mainui == null)
                {
                    mainui = GameObject.FindGameObjectWithTag("MainUI").GetComponent<Canvas>();
                }
                _lockedontarget.transform.SetParent(mainui.transform);
                _lockedontarget.transform.position = Camera.main.WorldToScreenPoint(Player.lockedon.transform.position);
            }

            //If the player is locked on to anything and the key for locking on is pressed, set the player locked on target to null
            if (KeyBindings.KeyPressed("LockOn") && (Player.lockedon != null))
            {
                Player.lockedon = null;
            }

            if (Player.lockedon == null)
            {
                Destroy(_lockedontarget);
            }

            //If the player is not locked on to anything and the key for locking on is pressed, set the player locked on target to the enemy, and instantiate the target
            if (KeyBindings.KeyPressed("LockOn") && Player.lockedon == null)
            {
                int ClosestIndex = GetIndexOfClosestEnemyToMousePoint();
                if (ClosestIndex != -1)
                {
                    Player.lockedon = Enemies.GetEnemies()[ClosestIndex];
                    _lockedontarget = Instantiate(lockedontarget);
                }
            }
        }
	}

    void OnDestroy()
    {
        GenericMenu2.OnOpen -= HideReticle;
        GenericMenu2.OnClose -= ShowReticle;
    }

    //Show the target
    public void ShowReticle(GameObject menu)
    {
        if (_lockedontarget != null)
        {
            _lockedontarget.enabled = true;
        }        
    }

    //Hide the target
    public void HideReticle(GameObject menu)
    {
        if (_lockedontarget != null)
        {
            _lockedontarget.enabled = false;
        }
    }

    //Returns the index of the closest enemy to where the mouse cursor is
    public static int GetIndexOfClosestEnemyToMousePoint()
    {
        Ray mousetoground = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;
        int index = -1;
        int count = 0;
        if (Physics.Raycast(mousetoground, out rayhit, 100f))
        {
            foreach (GameObject e in Enemies.GetEnemies())
            {
                float lowest = -1f;
                Transform loc = e.transform;
                float distance = Vector3.Distance(loc.position, rayhit.point);
                if ((lowest == -1f || distance < lowest) && distance < 0.7f)
                {
                    lowest = distance;
                    index = count;
                }
                count += 1;
            }
        }
        return index;
    }
}