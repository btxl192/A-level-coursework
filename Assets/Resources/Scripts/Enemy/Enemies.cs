using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour 
{
	private static List<GameObject> enemies = new List<GameObject> (); //list of all enemies on the scene
	public static int MaxEnemies {get; private set; }

	void Awake()
	{
		MaxEnemies = 50;
	}

    //Add an enemy to the enemies list
	public static void AddEnemy(GameObject e)
	{
		enemies.Add (e);
	}

    //remove an enemy from the enemies list
    public static void RemoveEnemy(GameObject e)
	{
		enemies.Remove (e);	
	}

	public static List<GameObject> GetEnemies()
	{
		return enemies;
	}

	public static void SetMaxEnemies(int i)
	{
		MaxEnemies = i;
	}

    //Instantiates a basic enemy
    public static void SpawnBasicEnemy(Vector3 Position)
    {
        Instantiate(Resources.Load(FileDir.BasicEnemy) as GameObject).transform.position = Position;
    }

    //Instantiates a main menu enemy
    public static void SpawnMainMenuEnemy(Vector3 Position)
    {
        Instantiate(Resources.Load(FileDir.MainMenuEnemy) as GameObject).transform.position = Position;
    }
}
