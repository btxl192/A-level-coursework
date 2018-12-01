using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    private float _speed;
    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }
    private bool MovementEnabled;
	private int groundmask;
	private GameObject player;
	public Vector3 direction { get; private set; }
	public bool canjump { get; private set; }
    private Rigidbody playerrb;
    private float JumpCheckTime = 2f;
    private float TimeSinceJumpCheck;
    private float PrevY;

    public delegate void BlankEvent();
    public static event BlankEvent PlayerMoving;
    public static event BlankEvent PlayerNotMoving;

    public delegate void sendangle(Quaternion a);
    public static event sendangle sendA;

	public delegate void sendpos(Vector3 P);
	public static event sendpos sendPos;
	
	void Start () 
	{
		player = this.gameObject;
        playerrb = player.GetComponent<Rigidbody>();
		groundmask = LayerMask.GetMask ("Ground");
		canjump = true;
        MovementEnabled = true;
	}

    
	void Update()
	{
        TimeSinceJumpCheck += Time.deltaTime;
        //Every JumpChecktime seconds, check if the player's Y coordinate has not changed since the last check. If it has not, the player can jump again
        if (TimeSinceJumpCheck >= JumpCheckTime)
        {
            if (System.Math.Round(PrevY,2) == System.Math.Round(player.transform.position.y,2))
            {
                canjump = true;
            }
            TimeSinceJumpCheck = 0f;
            PrevY = player.transform.position.y;
        }
        //If the player has its movement enabled, run MoveAndRotate
		if (!Pause.Paused && MovementEnabled)
		{
            //if the spacebar is pressed and the player can jump, run jump
            //Objective 1.3.2.7.9
			if (Input.GetKeyDown(KeyCode.Space) && canjump)
			{
				jump();
			}
			MoveAndRotate();
		}
    }


	void MoveAndRotate()
	{
		float horiz = Input.GetAxisRaw ("Horizontal");
		float verti = Input.GetAxisRaw ("Vertical");
		direction = new Vector3 (horiz, 0f, verti);
        //Used to determine whether the player is moving or not
        if (direction != Vector3.zero)
        {
            if (PlayerMoving != null)
            {
                PlayerMoving();
            }
        }
        else
        {
            if (PlayerNotMoving != null)
            {
                PlayerNotMoving();
            }
        }
		playerrb.MovePosition (player.transform.position + direction.normalized * speed* 0.01f);
        Ray mouseray = Camera.main.ScreenPointToRay(Input.mousePosition); 
		RaycastHit rayhit; //used for storing the ray once it hits the groundmask
		Vector3 facedirection;
		Quaternion facerotation = Quaternion.identity;
		if (player.GetComponent<PlayerBehaviour>().lockedon != null) //checks if the player is locked on to an enemy
		{
			facedirection = (player.GetComponent<PlayerBehaviour>().lockedon.transform.position - player.transform.position).normalized; //gets the vector that points from the player to the enemy then normalises it so that the value is only the direction, not the magnitude
			facedirection.y = 0f; //this is here to prevent the cube from tipping
			facerotation = Quaternion.LookRotation (facedirection);
			playerrb.MoveRotation (facerotation); 
		}
		else if (Physics.Raycast (mouseray, out rayhit, 100f, groundmask)) //if the player is not locked on, a check is done to see if the ray from the mouse hits the gruondmask. 
		{
			facedirection = rayhit.point - player.transform.position; //gets the vector that points from the player to the point that the ray from the mouse hits
			facedirection.y = 0f;
			facerotation = Quaternion.LookRotation (facedirection);
			playerrb.MoveRotation (facerotation);
		}
        //Objective 1.3.2.4.1
		sendA(facerotation);
		sendPos (player.transform.position);
	}

    //Causes the player to jump
	void jump()
	{
        canjump = false;
        playerrb.AddForce (Vector3.up * 50f);       
        StartCoroutine(JumpHelper());
	}

	void OnCollisionEnter(Collision other)
	{
        //Used for checking if the player can jump again
		if (System.Math.Round(other.transform.position.y + 0.5 * other.transform.localScale.y, 2) <= System.Math.Round(player.transform.position.y - 0.5 * player.transform.localScale.y, 2))
		{          
			canjump = true;
		}
        //Knock the player back if the player collides with an enemy and disable movement
		if (other.gameObject.CompareTag ("Enemy"))
		{
            StartCoroutine(DisableMovement(0.3f));
            playerrb.AddForce((player.transform.position - other.transform.position).normalized * 50f);
        }
	}

    //Stops the player from moving
    IEnumerator DisableMovement(float time)
    {
        MovementEnabled = false;
        yield return new WaitForSeconds(time);
        MovementEnabled = true;
    }

    //Checks if the player can jump
    IEnumerator JumpHelper()
    {
        float prevY = player.transform.position.y;
        yield return new WaitForSeconds(0.2f);
        if (System.Math.Round(player.transform.position.y, 1) > System.Math.Round(prevY, 1))
        {
            canjump = false;
        }
        else
        {
            canjump = true;
        }
    }
}
