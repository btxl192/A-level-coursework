using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.1.5
public class Pickup : MonoBehaviour {

    [SerializeField]
    private GameObject _item;
    public GameObject item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
        }
    }

    protected float Lifetime;
    protected float TimeSinceInstantiated;
    protected float Cooldown;
    protected bool StartDecaying;
    [SerializeField]
    protected MinimapIcon MapIcon;
    protected GameObject MainUI;
    protected GameObject PickupPopup;
    protected const string ChildPath = "Horizontal/Icon";

	protected virtual void Start()
	{        
        Initialise();
        try
        {
            item = Instantiate (item,new Vector3(0f,1000f,0f),Quaternion.identity);            
		}
		catch
		{
			Debug.Log("ERROR in pickup: couldn't set item");
		}
        if (Lifetime == 0)
        {
            Lifetime = 60f;
        }
	}

    protected void Initialise()
    {
        MainUI = GameObject.FindGameObjectWithTag("MainUI");
        PickupPopup = Instantiate(Resources.Load(FileDir.PickupPopup) as GameObject, MainUI.transform);
        if (item != null)
        {
            PickupPopup.transform.Find(ChildPath).gameObject.GetComponent<UnityEngine.UI.Image>().sprite = GenericItem.ItemTypeIcon[item.GetComponent<GenericItem>().category];
        }        
        //Objective 1.3.2.4.1
        if (MapIcon == null)
        {
            MapIcon = new MinimapIcon(Resources.Load(FileDir.ItemMinimapIcon) as GameObject, -1);
        }
        Minimap.CreateMiscIcon(Resources.Load(FileDir.ItemMinimapIcon) as GameObject, out MapIcon);
    }

    //Move the popup relatetive the player's position
	protected void Update()
	{
        if (GenericMenu2.OpenMenu != null || !CanRaycastToThis())
        {
            PickupPopup.SetActive(false);
        }
        else
        {
            PickupPopup.SetActive(true);
            //Objective 1.3.2.4.2
            Minimap.UpdateMiscIcon(MapIcon, transform.position);
        }        
        PickupPopup.transform.position = Camera.main.WorldToScreenPoint(this.transform.position) + new Vector3(0f, 20f, 0f);
        if (!Pause.Paused)
		{
            if (TimeSinceInstantiated > Lifetime && StartDecaying)
            {
                this.gameObject.AddComponent<FadeOut>().Initialise(2.5f, true);
            }
            else
            {
                TimeSinceInstantiated += Time.deltaTime;
            }
		}

	}

    //Handles what happens when it meets the player
    protected void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && TimeSinceInstantiated > Cooldown)
        {
            if (item != null)
            {
                if (!other.gameObject.GetComponent<PlayerInventory>().isinvfull())
                {

                    other.gameObject.GetComponent<PlayerInventory>().additem(Instantiate(item), true);
                    Destroy(this.gameObject);
                }
                else
                {
                    TempPopup.Show("Inventory is full. Couldn't pick up item!", Color.red);
                }
            }
            else
            {
                TempPopup.Show("Item not assigned!", Color.red);
            }
        }
    }

    public void SetProperties(float lifetime, float cooldown)
    {
        Lifetime = lifetime;
        Cooldown = cooldown;
        StartDecaying = true;
    }

    public void SetCategory(GenericItem.CategoryEnum Category)
    {
        PickupPopup.transform.Find(ChildPath).gameObject.GetComponent<UnityEngine.UI.Image>().sprite = GenericItem.ItemTypeIcon[Category];
    }

    private void OnDestroy()
    {
        Minimap.RemoveMiscIcon(MapIcon);
        Destroy(item);
        Destroy(PickupPopup);
    }

    //Checks if the item pickup is visible to the camera
    //https://gamedev.stackexchange.com/questions/85784/how-do-i-check-whether-a-camera-has-unobstructed-view-of-a-gameobject
    private bool CanRaycastToThis()
    {
        RaycastHit hit;
        // Calculate Ray direction
        Vector3 direction = Camera.main.transform.position - transform.position;
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if (hit.collider.tag == "MainCamera") //hit something else before the camera
            {
                return true;
            }
        }
        else if (!Physics.Raycast(transform.position, Vector3.up, 100f))
        {
            return true;
        }
        return false;
    }
}
