using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.2.6.b
public class Boss : GenericEnemy {

    protected bool firstmeet;
    protected bool Activated;

    public delegate void StringEvent(string s);
    public static event StringEvent Defeated;

    protected override void Start()
    {
        firstmeet = true;
        if (MapIcon == null)
        {
            MapIcon = new MinimapIcon(Resources.Load(FileDir.EnemyIcon) as GameObject, -1);
        }
        Minimap.CreateMiscIcon(MapIcon.Icon, out MapIcon);
        isdead = false;
        mainui = GameObject.FindGameObjectWithTag("MainUI").GetComponent<Canvas>();
        HealthBarReference = Resources.Load(FileDir.BossHealthBar) as GameObject;
        if (SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthBarsEnabled))
        {
            InstantiateHealthBar();
        }
        if (TargetTag != "" && TargetTag != null)
        {
            Target = GameObject.FindGameObjectWithTag(TargetTag);
        }
        this.gameObject.tag = "Enemy";
        Enemies.AddEnemy(this.gameObject);
        SetDrops();
        if (this.gameObject.GetComponent<Renderer>() != null)
        {
            thismaterial = this.gameObject.GetComponent<Renderer>().material;
        }
        EnemyID = Enemies.GetEnemies().IndexOf(this.gameObject);
        Graphics.EnableEnemyHealthBars += InstantiateHealthBar;
        Graphics.DisableEnemyHealthBars += DestroyHealthBar;
        if (GameObject.FindWithTag("TopUI") != null)
        {
            DamageNumberMask = Instantiate(Resources.Load(FileDir.EnemyDamageNumberMask) as GameObject, GameObject.FindGameObjectWithTag("TopUI").transform);
            DamageNumberMask.transform.SetAsLastSibling();
        }
    }

    //Controls how the boss gets aggroed. Runs WhileAggroed while the boss is aggroed, and WhileNotAggroed while the boss is not
    protected override void AdditionalBehaviour()
    {
        if (firstmeet && (Vector3.Distance(PlayerSave.staticplayer.transform.position, transform.position) <= 5 || health != maxhealth))
        {
            Aggroed = true;
            Activated = true;
            firstmeet = false;
            EnableHealthBar = true;
            OnFirstAggro();
        }

        if (Activated && Vector3.Distance(PlayerSave.staticplayer.transform.position, transform.position) <= 20)
        {
            WhileAggroed();
        }
        else if (!firstmeet)
        {
            WhileNotAggroed();
        }
    }

    //When the boss is aggroed for the first time
    protected virtual void OnFirstAggro()
    { }

    protected virtual void WhileAggroed()
    {
        Vector3 reltoplayer = (PlayerSave.staticplayer.transform.position - transform.position).normalized;
        reltoplayer = new Vector3(reltoplayer.x, 0f, reltoplayer.z);
        transform.rotation = Quaternion.LookRotation(reltoplayer);
    }

    protected virtual void WhileNotAggroed()
    { }

    protected override void UpdateHealthBar()
    {
        if (_healthbar != null)
        {
            if (EnableHealthBar)
            {

                _healthbar.GetComponent<EnemyHealthBar>().Sethealthnum(health + "/" + maxhealth);
                _healthbar.GetComponent<EnemyHealthBar>().Setlevelnum(EnemyName + " Lv. " + Level.ToString());
                _healthbar.value = health;
                _healthbar.maxValue = maxhealth;
                _healthbar.gameObject.SetActive(true);
            }
            else
            {
                _healthbar.gameObject.SetActive(false);
            }
        }
    }

    protected void CallDefeated()
    {
        if (Defeated != null)
        {
            Defeated(EnemyName);
        }        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        CallDefeated();
    }
}
