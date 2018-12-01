using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Graphics : MonoBehaviour {

    public delegate void BlankEvent();
    public static event BlankEvent EnableEnemyHealthBars;
    public static event BlankEvent DisableEnemyHealthBars;
    public static event BlankEvent EnableEnemyHealthNums;
    public static event BlankEvent DisableEnemyHealthNums;
    private const string healthbars = "Options/enemyhealthbarsenabled/EnableEnemyHealthBars";
    private const string healthnums = "Options/enemyhealthnum/enabled";
    private const string damagenums = "Options/enemydamagenums/enabled";

    private void Start()
    {
        transform.Find(healthbars).GetComponentInChildren<UnityEngine.UI.Text>().text = SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthBarsEnabled).ToString();
        transform.Find(healthnums).GetComponentInChildren<UnityEngine.UI.Text>().text = SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthNumsEnabled).ToString();
        transform.Find(damagenums).GetComponentInChildren<UnityEngine.UI.Text>().text = SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.DisplayEnemyDamageNum).ToString();
    }

    //Enable or disable enemy health bars
    public void SwitchEnemyHealthBars()
    {
        SettingsManager.ChangeBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthBarsEnabled, !SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthBarsEnabled));
        if (EnableEnemyHealthBars != null & DisableEnemyHealthBars != null)
        {
            if (SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthBarsEnabled))
            {
                EnableEnemyHealthBars();
            }
            else
            {
                DisableEnemyHealthBars();
            }
        }
        transform.Find(healthbars).GetComponentInChildren<UnityEngine.UI.Text>().text = SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthBarsEnabled).ToString();
    }

    //Enable or disable enemy health numbers
    public void SwitchEnemyHealthNums()
    {
        SettingsManager.ChangeBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthNumsEnabled, !SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthNumsEnabled));
        if (EnableEnemyHealthNums != null & DisableEnemyHealthNums != null)
        {
            if (SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthNumsEnabled))
            {
                EnableEnemyHealthNums();
            }
            else
            {
                DisableEnemyHealthNums();
            }
        }
        transform.Find(healthnums).GetComponentInChildren<UnityEngine.UI.Text>().text = SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthNumsEnabled).ToString();
    }
    //Enable or disable the enemy damage numbers
    public void SwitchEnemyDamageNums()
    {
        SettingsManager.ChangeBooleanSetting(SettingsManager.BooleanSettingsProperties.DisplayEnemyDamageNum, !SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.DisplayEnemyDamageNum));
        transform.Find(damagenums).GetComponentInChildren<UnityEngine.UI.Text>().text = SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.DisplayEnemyDamageNum).ToString();
    }
}
