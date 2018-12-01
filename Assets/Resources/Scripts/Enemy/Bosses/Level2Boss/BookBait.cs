using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookBait : Interactable {

    public static List<GameObject> AllBooks = new List<GameObject>();
    private bool BeingCarried;
    [SerializeField]
    private float forwardfactor;
    private float DespawnTime = 15f;
    private float TimeAlive;

    protected override void Awake()
    {
        base.Awake();
        AllBooks.Add(gameObject);
    }

    protected override void Update()
    {
        base.Update();
        TimeAlive += Time.deltaTime;
        //Keep the book in front of the player while it is being carried
        if (BeingCarried)
        {
            if (PlayerSave.staticplayer.GetComponent<PlayerBehaviour>().Interacting != this.gameObject)
            {
                PlayerSave.staticplayer.GetComponent<PlayerBehaviour>().Interacting = this.gameObject;
            }
            transform.rotation = PlayerSave.staticplayer.transform.rotation;
            transform.position = PlayerSave.staticplayer.transform.position + (forwardfactor * transform.localScale.z * 0.5f * transform.forward);
        }
        if (TimeAlive >= DespawnTime)
        {
            Destroy(gameObject);
        }
    }

    //Switch between carrying or throwing
    protected override void Interact()
    {
        base.Interact();
        BeingCarried = !BeingCarried;
        if (!BeingCarried)
        {
            Debug.Log(transform.forward + 1f * Vector3.up);
            GetComponent<Rigidbody>().AddForce((transform.forward + 0.7f * Vector3.up).normalized * 500f);
            InteractRange = 0.5f;
            PopupText = "Pick up";
        }
        else
        {
            InteractRange = 5f;
            PopupText = "Throw";
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AllBooks.Remove(gameObject);
    }
}
