using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemySave : MonoBehaviour {

    private static string savedir
    {
        get
        { 
            return FileDir.SaveFile + "/Enemies.txt";
        }
    } 

    //Save the enemies to a text file
    public static void SaveEnemies()
    {
        StreamWriter writer = new StreamWriter(savedir);
        foreach (GameObject g in Enemies.GetEnemies())
        {
            if (g.GetComponent<Boss>() == null)
            {
                writer.WriteLine(g.GetComponent<GenericEnemy>().GetSaveData());
            }            
        }
        writer.Close();
    }

    //Load the enemies from a text file
    public static void LoadEnemies()
    {
        foreach (string s in File.ReadAllLines(savedir))
        {
            if (s != "")
            {
                string[] details = s.Split("/".ToCharArray()[0]);
                GameObject tempenemy;                
                switch (details[0])
                {
                    case "RangedEnemy":
                        tempenemy = Instantiate(Resources.Load(FileDir.RangedEnemy) as GameObject);
                        break;
                    default:
                        tempenemy = Instantiate(Resources.Load(FileDir.BasicEnemy) as GameObject);
                        break;
                }
                tempenemy.SetActive(false);
                tempenemy.GetComponent<GenericEnemy>().SetDetails(details);
                tempenemy.SetActive(true);
            }

        }
        
    }
}
