using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayer : MonoBehaviour {

    [SerializeField]
    private float Speed;
    [SerializeField]
    private float JumpInterval;
    [SerializeField]
    private float JumpChance;

    private float JumpTimer;
    private bool CanJump;
    private int Count;
    private bool OffCamera;

    void Start()
    { 
        CanJump = true;
    }

	void Update ()
    {
        if (JumpTimer < JumpInterval)
        {
            JumpTimer += Time.deltaTime;
        }
        GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(Speed * 0.1f, 0f, 0f));
        if (JumpTimer >= JumpInterval && CanJump)
        {
            JumpTimer = 0;
            int rnd = UnityEngine.Random.Range(0, 100);
            if (JumpChance >= rnd)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);
                CanJump = false;
            }           
        }
        if (OffCamera && MainMenuEnemy.Count == Enemies.GetEnemies().Count)
        {
            OffCamera = false;
            transform.position = new Vector3(-11f, transform.position.y, 53f);
            Count += 1;
            foreach (GameObject g in Enemies.GetEnemies())
            {
                g.GetComponent<GenericEnemy>().Kill();
            }
            for (int a = 0; a < Count; a++)
            {
                Enemies.SpawnMainMenuEnemy(new Vector3(-11f - 1 * Count, -3f, 53f));
            }
            MainMenuEnemy.ResetCount();
        }
	}

    void OnBecameInvisible()
    {
        OffCamera = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            CanJump = true;
        }
    }
}
