using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsManager : MonoBehaviour {

    public static int SaveNum { get; set; }
    public static string SaveName
    {
        get
        {
            return "Save" + SaveNum;
        }
    }
    private const string SettingsFileDir = "Settings.txt";

    public enum BooleanSettingsProperties
    {
        EnemyHealthBarsEnabled,
        EnemyHealthNumsEnabled,
        MinimapEnabled,
        DisplayEnemyDamageNum
    }

    public enum NumberSettingsProperties
    {
        MinimapZoom
    }

    private static Dictionary<string, bool> ToBool = new Dictionary<string, bool>()
    {
        { "True", true},
        { "False", false }
    };

    public static Dictionary<string, BooleanSettingsProperties> StringToBooleanSetting = new Dictionary<string, BooleanSettingsProperties>();

    public static Dictionary<string, NumberSettingsProperties> StringToNumberSetting = new Dictionary<string, NumberSettingsProperties>();

    private static Dictionary<BooleanSettingsProperties, bool> _BooleanSettings = new Dictionary<BooleanSettingsProperties, bool>()
    {
        {BooleanSettingsProperties.EnemyHealthBarsEnabled, true},
        {BooleanSettingsProperties.EnemyHealthNumsEnabled, true},
        {BooleanSettingsProperties.MinimapEnabled, true},
        {BooleanSettingsProperties.DisplayEnemyDamageNum, true}
    };

    private static Dictionary<NumberSettingsProperties, float> _NumberSettings = new Dictionary<NumberSettingsProperties, float>()
    {
        {NumberSettingsProperties.MinimapZoom, 10 }
    };

    //Get the value of the given setting 
    public static bool GetBooleanSetting(BooleanSettingsProperties Setting)
    {
        return _BooleanSettings[Setting];
    }

    //Get the value of the given setting
    public static float GetNumberSetting(NumberSettingsProperties Setting)
    {
        return _NumberSettings[Setting];
    }

    private void Awake()
    {
        try
        {
            foreach (BooleanSettingsProperties s in System.Enum.GetValues(typeof(BooleanSettingsProperties)))
            {
                StringToBooleanSetting.Add(s.ToString(), s);
            }
            foreach (NumberSettingsProperties s in System.Enum.GetValues(typeof(NumberSettingsProperties)))
            {
                StringToNumberSetting.Add(s.ToString(), s);
            }
        }
        catch { }
        try
        {
            LoadSettings();
        }
        catch
        {
            WriteFile();
        }
    } 

    //Write the settings to a text file
    private static void WriteFile()
    {
        StreamWriter writer = new StreamWriter(SettingsFileDir);
        foreach (BooleanSettingsProperties s in System.Enum.GetValues(typeof(BooleanSettingsProperties)))
        {
            writer.WriteLine(s.ToString() + "=" + _BooleanSettings[s]);
        }
        foreach (NumberSettingsProperties s in System.Enum.GetValues(typeof(NumberSettingsProperties)))
        {
            writer.WriteLine(s.ToString() + "=" + _NumberSettings[s]);
        }
        writer.Close();
    }

    //Load the settings from the text file
    private void LoadSettings()
    {
        StreamReader reader = new StreamReader(SettingsFileDir);
        foreach (string s in File.ReadAllLines(SettingsFileDir))
        {
            string[] split = s.Split("=".ToCharArray()[0]);
            try
            {
                _BooleanSettings[StringToBooleanSetting[split[0]]] = ToBool[split[1]];
            }
            catch
            {
                _NumberSettings[StringToNumberSetting[split[0]]] = float.Parse(split[1]);
            }
        }
        reader.Close();
    }

    //Change the specified setting to the value given
    public static void ChangeBooleanSetting(BooleanSettingsProperties Setting, bool b)
    {
        _BooleanSettings[Setting] = b;
        WriteFile();
    }

    //Change the specified setting to the value given
    public static void ChangeNumberSetting(NumberSettingsProperties Setting, float f)
    {
        _NumberSettings[Setting] = f;
        WriteFile();
    }

    //Sets which save file the player is currently on
    public static void SetSaveNum(int i)
    {
        SaveNum = i;
    }

    public void CreateNewSave()
    {
        if (!System.IO.Directory.Exists(FileDir.SaveDirectory))
        {
            System.IO.Directory.CreateDirectory(FileDir.SaveDirectory);
        }
        SaveNum = Directory.GetDirectories(FileDir.SaveDirectory).Length + 1;       
    }

}
