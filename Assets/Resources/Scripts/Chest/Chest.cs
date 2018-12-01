using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chest : Interactable {

    [SerializeField]
    private GameObject ChestUI;
    [SerializeField]
    private ChestUI ChestUIControls;
    [SerializeField]
    private List<GameObject> _Contents = new List<GameObject>();  
    public List<GameObject> Contents
    {
        get
        {
            return _Contents;
        }
    }

    void Start()
    {
        ChestUI = GameObject.FindGameObjectWithTag("ChestUI");
        ChestUIControls = ChestUI.GetComponentInChildren<ChestUI>();
        ChestButton.ButtonPressed += TransferToPlayer;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        ChestButton.ButtonPressed -= TransferToPlayer;
    }

    protected override void Interact()
    {
        ChestUI.GetComponent<Canvas>().enabled = true;        
        GenericMenu2.SetOpenMenu(ChestUI);
        UpdateUI();
    }

    protected override void UnInteract()
    {
        OutOfRange();
    }

    protected override void OutOfRange()
    {
        ChestUIControls.Clear();
        GenericMenu2.SetOpenMenu(null);
        ChestUI.GetComponent<Canvas>().enabled = false;
    }

    //Updates the chest UI with the chest's contents
    public void UpdateUI()
    {
        ChestUI = GameObject.FindGameObjectWithTag("ChestUI");
        ChestUIControls = ChestUI.GetComponentInChildren<ChestUI>();
        ChestUIControls.Clear();
        foreach (GameObject g in Contents)
        {
            if (g != null)
            {
                try
                {
                    ChestUIControls.AddButton(g.GetComponent<GenericItem>(), Contents.IndexOf(g), gameObject);
                }
                catch
                {
                    Debug.Log("A GAMEOBJECT THAT IS NOT AN ITEM WAS PUT IN A CHEST. Object: " + g);
                }
            }
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    //Transfers an item from Contents to the player's inventory
    public void TransferToPlayer(int itemindex, GameObject Chest)
    {
        if (Chest == gameObject)
        {
            if (!PlayerSave.staticplayer.GetComponent<PlayerInventory>().isinvfull())
            {
                PlayerSave.staticplayer.GetComponent<PlayerInventory>().additem(Instantiate(Contents[itemindex]), true);
                Contents.RemoveAt(itemindex);
                UpdateUI();
            }
            else
            {
                TempPopup.Show("Inventory is full!", Color.red);
            }
        }

    }
}
