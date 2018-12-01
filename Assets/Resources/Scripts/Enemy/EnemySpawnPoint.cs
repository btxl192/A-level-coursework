using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.2.7
public class EnemySpawnPoint : MonoBehaviour 
{
    public float SpawnDelay;
    private bool StartSpawning;
	public GameObject EnemyToSpawn;
	public float SpawnPeriod;
    public int EnemyLevel;
    public int NumEnemiesToSpawn;
    private int EnemiesSpawned;
	private float TimeSinceLastSpawn;

	void Update()
	{
        if (StartSpawning)
        {
            if (TimeSinceLastSpawn > SpawnPeriod)
            {
                if (Enemies.GetEnemies().Count < Enemies.MaxEnemies && (EnemiesSpawned < NumEnemiesToSpawn || NumEnemiesToSpawn == -1))
                {
                    //Spawn an enemy if the time since the last spawn is at least the spawn period and the number of enemies on the scene is less than the maximum enemies
                    try
                    {
                        Instantiate(EnemyToSpawn, this.transform.position, Quaternion.identity).GetComponent<GenericEnemy>().SetLevel(EnemyLevel);
                        EnemiesSpawned += 1;
                    }
                    catch
                    {
                        Debug.Log("No enemy assigned");
                    }
                }
                TimeSinceLastSpawn = 0f;
            }
            else
            {
                TimeSinceLastSpawn += Time.deltaTime;
            }
        }
        else
        {
            //Only start spawning enemies if the spawn delay has passed
            if (Time.timeSinceLevelLoad >= SpawnDelay)
            {
                StartSpawning = true;
            }
        }

	}
}
