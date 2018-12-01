using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

//Objective 1.3.2.11.2
public class PlayerSave : MonoBehaviour {

    [SerializeField]
    private static GameObject _staticplayer;
    public static GameObject staticplayer
    {
        get
        {
            if (_staticplayer == null)
            {
                SetStaticPlayer();
            }
            return _staticplayer;
        }
    }
    private static string PlayerPosSaveDir
    {
        get
        {
            return FileDir.SaveFile + "/PlayerPosition.txt";
        }
    }
    private static string PlayerInvSaveDir
    {
        get
        {
            return FileDir.SaveFile + "/PlayerInv.txt";
        }
    }
    private static string PlayerTempInvSaveDir
    {
        get
        {
            return FileDir.SaveFile + "/PlayerInvTemp.txt";
        }
    }
    private static string PlayerEqSaveDir
    {
        get
        {
            return FileDir.SaveFile + "/PlayerEq.txt";
        }
    }
    private static string PlayerTempEqSaveDir
    {
        get
        {
            return FileDir.SaveFile + "/PlayerEqTemp.txt";
        }
    }
    private static string PlayerStatsSaveDir
    {
        get
        {
            return FileDir.SaveFile + "/PlayerStats.txt";
        }
    }
    private static string PlayerStatsSaveDirTemp
    {
        get
        {
            return FileDir.SaveFile + "/PlayerStatsTemp.txt";
        }
    }
    private static string SkillsSaveDir
    {
        get
        {
            return FileDir.SaveFile + "/Skills.txt";
        }
    }
    private static string SkillsSaveDirTemp
    {
        get
        {
            return FileDir.SaveFile + "/SkillsTemp.txt";
        }
    }
    private static string SkillSetsSaveDir
    {
        get
        {
            return FileDir.SaveFile + "/SkillSets.txt";
        }
    }
    private static string SkillSetsSaveDirTemp
    {
        get
        {
            return FileDir.SaveFile + "/SkillSetsTemp.txt";
        }
    }
    private static string SpawnsSaveDir
    {
        get
        {
            return FileDir.SaveFile + "/SpawnPoint.txt";
        }
    }
    private static string WorldFlagsDir
    {
        get
        {
            return FileDir.SaveFile + "/WorldFlags.txt";
        }
    }
    public static bool Loaded { get; private set; }

    private void Awake()
    {
        _staticplayer = GameObject.FindGameObjectWithTag("Player");
        LoadPlayer.SaveGame += SaveGame;
        SetStaticPlayer();
    }

    void OnDestroy()
    {
        LoadPlayer.SaveGame -= SaveGame;
    }

    public void SavePosition()
    {
        _staticplayer = GameObject.FindGameObjectWithTag("Player");
        StreamWriter writer = new StreamWriter(PlayerPosSaveDir);
        writer.WriteLine("scene:" + SceneManager.GetActiveScene().name);
        writer.WriteLine("pos:" + _staticplayer.transform.position);
        writer.Close();
    }

    public void LoadScene()
    {
        foreach (string s in File.ReadAllLines(PlayerPosSaveDir))
        {
            if (s.Contains("scene:"))
            {
                string temp = s.Remove(0, 6);
                SceneManager.LoadScene(temp);
            }
        }
    }

    public static void LoadPosition()
    {
        foreach (string s in File.ReadAllLines(PlayerPosSaveDir))
        {
            if (s.Contains("pos:"))
            {
                string temp = s.Remove(0, 4);
                temp = temp.Replace("(", "");
                temp = temp.Replace(")", "");
                temp = temp.Trim();
                string[] stringcoords = temp.Split(",".ToCharArray()[0]);
                List<float> floatcoords = new List<float>();
                foreach (string s2 in stringcoords)
                {
                    floatcoords.Add(float.Parse(s2));
                }
                Vector3 pos = new Vector3(floatcoords[0], floatcoords[1], floatcoords[2]);
                staticplayer.transform.position = pos;
            }
        }
    }

    public void SaveGame()
    {
        if (!Directory.Exists(FileDir.SaveFile))
        {
            Directory.CreateDirectory(FileDir.SaveFile);
        }
        SavePosition();
        SaveInventory();
        SaveStats();
        SaveSkills();
        StreamWriter writer = new StreamWriter(FileDir.SaveFile + "/SaveTime.txt", false);
        writer.WriteLine(System.DateTime.Now);
        writer.Close();
        EnemySave.SaveEnemies();
        SaveSpawnPoint();
        SaveFlags();
        TempPopup.Show("Game saved!", Color.yellow);
    }

    public static void SaveTemp()
    {
        SaveInventoryTemp();
        SaveSkillsTemp();
        SaveStatsTemp();
    }

    public static void LoadGame()
    {
        Loaded = false;
        LoadPosition();
        LoadStats(false);
        LoadSkills(false);
        LoadInv(false);
        LoadFlags();
        EnemySave.LoadEnemies();
        LoadSpawnPoint();
        Loaded = true;        
    }

    public static void LoadTemp()
    {
        LoadInv(true);
        LoadStats(true);
        LoadSkills(true);
        DeleteTemp();
    }

    public void SaveSkills()
    {
        StreamWriter writer = new StreamWriter(SkillsSaveDir);
        for (int a = 0; a < SkillManager.Skills.Count; a++)
        {
            writer.WriteLine(a + "," + SkillManager.Skills[a].CurrentLevel);
        }
        writer.Close();
        writer = new StreamWriter(SkillSetsSaveDir);
        foreach (Skill.SkillSets s in SkillManager.PlayerSkillSets)
        {
            writer.WriteLine(s);
        }
        writer.Close();
    }

    public static void SaveSkillsTemp()
    {
        StreamWriter writer = new StreamWriter(SkillsSaveDirTemp);
        for (int a = 0; a < SkillManager.Skills.Count; a++)
        {
            writer.WriteLine(a + "," + SkillManager.Skills[a].CurrentLevel);
        }
        writer.Close();
        writer = new StreamWriter(SkillSetsSaveDirTemp);
        foreach (Skill.SkillSets s in SkillManager.PlayerSkillSets)
        {
            writer.WriteLine(s);
        }
        writer.Close();
    }

    public static void LoadSkills(bool temp)
    {
        string savedir;
        string skillsetssavedir;
        if (temp)
        {
            savedir = SkillsSaveDirTemp;
            skillsetssavedir = SkillSetsSaveDirTemp;
        }
        else
        {
            savedir = SkillsSaveDir;
            skillsetssavedir = SkillSetsSaveDir;
        }
        if (Directory.Exists(FileDir.SaveFile))
        {
            SkillManager.InitialiseSkills();
            foreach (string s in File.ReadAllLines(savedir))
            {
                string[] split = s.Split(",".ToCharArray()[0]);
                SkillManager.Skills[StringToInt(split[0])].SetLevel(StringToInt(split[1]));
            }
            foreach (string s in File.ReadAllLines(skillsetssavedir))
            {
                Skill.SkillSets sk = (Skill.SkillSets)(System.Enum.Parse(typeof(Skill.SkillSets), s));
                if (!SkillManager.PlayerSkillSets.Contains(sk))
                {
                    SkillManager.PlayerSkillSets.Add(sk);
                }               
            }
        }
    }

    public void SaveStats()
    {
        PlayerStats PStats = _staticplayer.GetComponent<PlayerStats>();
        StreamWriter writer = new StreamWriter(PlayerStatsSaveDir);
        writer.WriteLine("Health=" + PStats.HP.health);
        writer.WriteLine("Mana=" + PStats.MP.mana);
        writer.WriteLine("Exp=" + PStats.exp);
        writer.WriteLine("Level=" + PStats.level);
        writer.WriteLine("SkillPoints=" + PStats.skillpoints);
        writer.Close();
    }

    public static void SaveStatsTemp()
    {
        PlayerStats PStats = _staticplayer.GetComponent<PlayerStats>();
        StreamWriter writer = new StreamWriter(PlayerStatsSaveDirTemp);
        writer.WriteLine("Health=" + PStats.HP.health);
        writer.WriteLine("Mana=" + PStats.MP.mana);
        writer.WriteLine("Exp=" + PStats.exp);
        writer.WriteLine("Level=" + PStats.level);
        writer.WriteLine("SkillPoints=" + PStats.skillpoints);
        writer.Close();
    }

    public static void LoadStats(bool temp)
    {
        string savedir;
        if (temp)
        {
            savedir = PlayerStatsSaveDirTemp;
        }
        else
        {
            savedir = PlayerStatsSaveDir;
        }
        PlayerStats PStats = _staticplayer.GetComponent<PlayerStats>();
        foreach (string s in File.ReadAllLines(savedir))
        {
            int val = StringToInt(s.Split("=".ToCharArray()[0])[1]);
            if (s.Contains("Health"))
            {
                PStats.HP.SetHP(val);
            }
            else if (s.Contains("Mana"))
            {
                PStats.MP.SetMP(val);
            }
            else if (s.Contains("Exp"))
            {
                PStats.SetExp(val);
            }
            else if (s.Contains("Level"))
            {
                PStats.SetLevel(val);
            }
            else if (s.Contains("SkillPoints"))
            {
                PStats.SetSkillPoints(val);
            }
        }
    }

    public void SaveInventory()
    {
        List<GameObject> PlayerInv = _staticplayer.GetComponent<PlayerInventory>().inventory;
        StreamWriter writer = new StreamWriter(PlayerInvSaveDir, false);
        writer.WriteLine("Gold=" + _staticplayer.GetComponent<PlayerInventory>().Gold);
        foreach (GameObject g in PlayerInv)
        {
            writer.WriteLine(g.GetComponent<GenericItem>().GetSaveData());
        }
        writer.Close();
        PlayerEquipment PlayerEQ = _staticplayer.GetComponent<PlayerEquipment>();
        writer = new StreamWriter(PlayerEqSaveDir, false);
        if (PlayerEQ.WeaponObject != null)
        {
            writer.WriteLine(PlayerEQ.WeaponObject.GetComponent<GenericItem>().GetComponent<GenericItem>().GetSaveData());
        }
        if (PlayerEQ.ArmourObject != null)
        {
            writer.WriteLine(PlayerEQ.ArmourObject.GetComponent<GenericItem>().GetComponent<GenericItem>().GetSaveData());
        }
        if (PlayerEQ.SkillCore != null)
        {
            writer.WriteLine(PlayerEQ.SkillCore.GetComponent<GenericItem>().GetComponent<GenericItem>().GetSaveData());
        }
        writer.Close();
    }

    public static void SaveInventoryTemp()
    {
        List<GameObject> PlayerInv = _staticplayer.GetComponent<PlayerInventory>().inventory;
        StreamWriter writer = new StreamWriter(PlayerTempInvSaveDir, false);
        writer.WriteLine("Gold=" + _staticplayer.GetComponent<PlayerInventory>().Gold);
        foreach (GameObject g in PlayerInv)
        {
            writer.WriteLine(g.GetComponent<GenericItem>().GetSaveData());
        }
        writer.Close();
        PlayerEquipment PlayerEQ = _staticplayer.GetComponent<PlayerEquipment>();
        writer = new StreamWriter(PlayerTempEqSaveDir, false);
        if (PlayerEQ.WeaponObject != null)
        {
            writer.WriteLine(PlayerEQ.WeaponObject.GetComponent<GenericItem>().GetComponent<GenericItem>().GetSaveData());
        }
        if (PlayerEQ.ArmourObject != null)
        {
            writer.WriteLine(PlayerEQ.ArmourObject.GetComponent<GenericItem>().GetComponent<GenericItem>().GetSaveData());
        }
        if (PlayerEQ.SkillCore != null)
        {
            writer.WriteLine(PlayerEQ.SkillCore.GetComponent<GenericItem>().GetComponent<GenericItem>().GetSaveData());
        }
        writer.Close();
    }

    public static void LoadInv(bool Temp)
    {
        string invsavedir;
        string eqsavedir;
        if (Temp)
        {
            invsavedir = PlayerTempInvSaveDir;
            eqsavedir = PlayerTempEqSaveDir;
        }
        else
        {
            invsavedir = PlayerInvSaveDir;
            eqsavedir = PlayerEqSaveDir;
        }
        foreach (string s in File.ReadAllLines(invsavedir))
        {
            if (s.Contains("Gold"))
            {
                string[] line = s.Split("=".ToCharArray()[0]);
                staticplayer.GetComponent<PlayerInventory>().SetGold(StringToInt(line[1]));
            }
            else
            {
                string[] line = s.Split(",".ToCharArray()[0]);
                GameObject item = LoadItem(line);
                staticplayer.GetComponent<PlayerInventory>().additem(Instantiate(item), false);
                Destroy(item);
            }
        }
        foreach (string s in File.ReadAllLines(eqsavedir))
        {
            string[] line = s.Split(",".ToCharArray()[0]);
            GameObject item = LoadItem(line);
            GameObject temp = null;
            staticplayer.GetComponent<PlayerEquipment>().Equip(Instantiate(item), out temp);
            Destroy(item);
        }
    }

    public static int StringToInt(string s)
    {
        return (int)System.Int64.Parse(s);
    }

    public static GameObject LoadItem(string[] details)
    {
        GameObject tempobject = null;
        switch (details[0])
        {
            case "Skill Core":
                tempobject = Instantiate(Resources.Load(FileDir.BaseSkillCore) as GameObject);
                for (int a = 6; a < details.Length; a++)
                {
                    string[] SkillDetails = details[a].Split("/".ToCharArray()[0]);
                    tempobject.GetComponent<SkillCore>().SkillIDs.Add(StringToInt(SkillDetails[0]));
                    tempobject.GetComponent<SkillCore>().SkillLevels.Add(StringToInt(SkillDetails[1]));
                }
                break;
            case "Projectile":
            case "Weapon":
                if (details[0] == "Projectile")
                {
                    tempobject = Instantiate(Resources.Load(FileDir.BasicProjectile) as GameObject);
                }
                List<Statuses.StatusType> StatusEffects = new List<Statuses.StatusType>();
                for (int a = 7; a < details.Length; a++)
                {
                    string[] StatusDetails = details[a].Split("/".ToCharArray()[0]);
                    StatusEffects.Add(Statuses.MakeStatus((Statuses.statuses)System.Enum.Parse(typeof(Statuses.statuses), StatusDetails[0]), StringToInt(StatusDetails[1])));
                }
                tempobject.GetComponent<GenericWeapon>().SetStatuses(StatusEffects);
                tempobject.GetComponent<GenericWeapon>().SetDamage(StringToInt(details[5]));
                break;
            case "EnemyProjectile":
                tempobject = Instantiate(Resources.Load(FileDir.EnemyProjectile) as GameObject);
                List<Statuses.StatusType> StatusEffects2 = new List<Statuses.StatusType>();
                for (int a = 7; a < details.Length; a++)
                {
                    string[] StatusDetails = details[a].Split("/".ToCharArray()[0]);
                    StatusEffects2.Add(Statuses.MakeStatus((Statuses.statuses)System.Enum.Parse(typeof(Statuses.statuses), StatusDetails[0]), StringToInt(StatusDetails[1])));
                }
                tempobject.GetComponent<GenericWeapon>().SetStatuses(StatusEffects2);
                tempobject.GetComponent<GenericWeapon>().SetDamage(StringToInt(details[5]));
                break;
            case "HealingItem":
                tempobject = Instantiate(Resources.Load(FileDir.HealingItem) as GameObject);
                tempobject.GetComponent<StackableItem>().SetAmount(StringToInt(details[5]));
                tempobject.GetComponent<HealingItem>().SetHealDetails(bool.Parse(details[6]), StringToInt(details[7]), StringToInt(details[8]));
                break;
            case "Armour":
                tempobject = Instantiate(Resources.Load(FileDir.BaseArmour) as GameObject);
                tempobject.GetComponent<GenericArmour>().SetArmour(StringToInt(details[5]));
                break;
            case "Tome":
                tempobject = Instantiate(Resources.Load(FileDir.BaseTome) as GameObject);
                tempobject.GetComponent<Tome>().SetSkillSet((Skill.SkillSets)(System.Enum.Parse(typeof(Skill.SkillSets), details[6])));
                break;
            default:
                tempobject = Instantiate(Resources.Load(FileDir.Blank) as GameObject);
                tempobject.AddComponent<GenericItem>();
                tempobject.GetComponent<GenericItem>().SetCategory(GenericItem.CategoryEnum.Item);
                break;
        }
        tempobject.GetComponent<GenericItem>().SetDetails(details[1], details[2], (GenericItem.Rarities)System.Enum.Parse(typeof(GenericItem.Rarities), details[3]), StringToInt(details[4]));
        tempobject.name = tempobject.GetComponent<GenericItem>().itemname + "2";
        return tempobject;
    }

    public static void SetStaticPlayer()
    {
        _staticplayer = GameObject.FindGameObjectWithTag("Player");
    }

    //Delete the temporary save files
    public static void DeleteTemp()
    {
        File.Delete(PlayerTempEqSaveDir);
        File.Delete(PlayerTempInvSaveDir);
        File.Delete(PlayerStatsSaveDirTemp);
        File.Delete(SkillsSaveDirTemp);
        File.Delete(SkillSetsSaveDirTemp);
    }

    public static void SetLoaded(bool b)
    {
        Loaded = b;
    }

    public static void SaveSpawnPoint()
    {
        if (RespawnPoint.CurrentSpawnPoint != null)
        {
            StreamWriter writer = new StreamWriter(SpawnsSaveDir);
            writer.WriteLine(RespawnPoint.CurrentSpawnPoint.GetComponent<RespawnPoint>().ID);
            writer.Close();
        }
    }

    public static void LoadSpawnPoint()
    {
        try
        {
            StreamReader reader = new StreamReader(SpawnsSaveDir);
            RespawnPoint.SetSpawn(RespawnPoint.SpawnDict[SceneManager.GetActiveScene().buildIndex][reader.ReadLine()]);
            reader.Close();
        }
        catch
        {
            Debug.Log("Could not load spawn point");
        }
    }

    //World event flags
    public static void SaveFlags()
    {
        StreamWriter writer = new StreamWriter(WorldFlagsDir);
        foreach (string key in WorldFlags.Flags.Keys)
        {
            writer.WriteLine(key + "=" + WorldFlags.Flags[key]);
        }
        writer.Close();
    }

    public static void LoadFlags()
    {
        foreach (string line in File.ReadAllLines(WorldFlagsDir))
        {
            string[] details = line.Split("=".ToCharArray()[0]);
            WorldFlags.Flags[details[0]] = bool.Parse(details[1]);
        }
    }
}
