using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;
using TMPro;

public class GenericEnemy : MonoBehaviour {

    [SerializeField]
    protected string EnemyName;

    [SerializeField]
    protected MinimapIcon MapIcon;

    [SerializeField]
    protected int _basemaxhealth;
    public virtual int maxhealth
    {
        get
        {
            return (int)(_basemaxhealth * Math.Pow(1.2, Level - 1));
        }
    }

    [SerializeField]
    protected int _health;
    public int health
    {
        get
        {
            return _health;
        }
    }

    [SerializeField]
    protected int _damage;
    public int damage
    {
        get { return _damage; }
    }

    [SerializeField]
    protected int _exp;
    public int exp
    {
        get { return _exp; }
    }

    [SerializeField]
    protected int _Level;
    public int Level
    {
        get { return _Level; }
    }

    //1.3.2.2.3.a
    [SerializeField]
    protected int _DropChance;
    public int DropChance
    {
        get { return _DropChance; }
    }

    public delegate void intevent(int e);
    public static event intevent giveexp;
    public static event intevent DamagedByPlayer;

    protected Slider _healthbar;
    protected Canvas mainui;
    protected GameObject Target;
    protected GameObject Player;
    protected List<Statuses.StatusType> StatusEffects = new List<Statuses.StatusType>();
    protected bool isdead;
    protected Material thismaterial;
    protected int EnemyID;
    protected List<EnemyItemDrop> DropTable = new List<EnemyItemDrop>();
    protected bool OnGround;
    [SerializeField]
    protected bool EnableHealthBar;
    protected GameObject HealthBarReference;
    protected GameObject DamageNumberMask;
    [SerializeField]
    protected float AggroRadius;
    protected bool Aggroed;
    [SerializeField]
    protected string TargetTag;
    [SerializeField]
    protected int MinGold;
    [SerializeField]
    protected int MaxGold;
    [SerializeField]
    protected List<GameObject> _GuaranteedDrops = new List<GameObject>();
    public List<GameObject> GuaranteedDrops
    {
        get
        {
            return _GuaranteedDrops;
        }
    }

    //Initialise the drop table
    protected virtual void SetDrops()
    {
        DropTable = new List<EnemyItemDrop>()
        {
            new EnemyItemDrop(Resources.Load(FileDir.RandomWeaponPickup) as GameObject,2),
            new EnemyItemDrop(Resources.Load(FileDir.RandomArmourPickup) as GameObject,1),
        };
        SetItemSkews();
    }

	protected virtual void Start () 
	{
        //Objective 1.3.2.4.1
        if (MapIcon == null)
        {
            MapIcon = new MinimapIcon(Resources.Load(FileDir.EnemyIcon) as GameObject, -1);
        }
        Minimap.CreateMiscIcon(MapIcon.Icon, out MapIcon);
        isdead = false;
		mainui = GameObject.FindGameObjectWithTag ("MainUI").GetComponent<Canvas>();
	    HealthBarReference = Resources.Load (FileDir.EnemyHealthBar) as GameObject;
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
        if (gameObject.GetComponent<ItemDrop>() == null)
        {
            gameObject.AddComponent<ItemDrop>();
        }
	}

    protected virtual void OnDestroy()
    {
        Graphics.EnableEnemyHealthBars -= InstantiateHealthBar;
        Graphics.DisableEnemyHealthBars -= DestroyHealthBar;
        Enemies.RemoveEnemy(this.gameObject);
    }

    //Initialise the item skew values in the drop table
    protected void SetItemSkews()
    {
        int totalskew = 0;
        foreach (EnemyItemDrop i in DropTable)
        {
            for (int a = 0; a < i.Skew; a++)
            {
                totalskew += 1;
            }
            i.SumSkew = totalskew;
        }
    }

    protected void Update()
    {
        if (!isdead)
        {
            if (Player == null)
            {
                Player = PlayerSave.staticplayer;
            }
            UpdateHealthBar();
            if (DamageNumberMask != null && SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.DisplayEnemyDamageNum))
            {
                DamageNumberMask.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(20f, 20f, 0);
            }
            //Objective 1.3.2.4.2
            Minimap.UpdateMiscIcon(MapIcon, transform.position);
            if (!Pause.Paused)
            {
                AdditionalBehaviour();
                //Objective 1.3.2.2.1.b
                if (_health <= 0)
                {
                    if (!isdead)
                    {
                        isdead = true;
                        dead();
                        try
                        {
                            gameObject.GetComponent<Renderer>().material = Resources.Load(FileDir.EnemyTransparent) as Material;
                            gameObject.AddComponent<FadeOut>().Initialise(0.5f, true);
                        }
                        catch
                        {
                            Destroy(gameObject);
                        }
                    }

                }
            }
        }
    }

    //Any additional behaviour derived classes may exhibit
	protected virtual void AdditionalBehaviour()
	{
        //Objective 1.3.2.2.4
        if (OnGround && Target != null && (Vector3.Distance(Target.transform.position, transform.position) <= AggroRadius || AggroRadius == -1))
        {
            Aggroed = true;
            this.gameObject.GetComponent<NavMeshAgent>().SetDestination(Target.transform.position);
        }
        else
        {
            this.gameObject.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        }
    }

    //Reduce health and display the damage value
    //Objective 1.3.2.2.1.a
	public void TakeDamage(int d)
	{
		_health -= d;
        DisplayDamageNumber(d);
    }

    //Update the values on the healthbar
    protected virtual void UpdateHealthBar()
	{
        if (_healthbar != null)
        {
            if (!Pause.Paused && (GenericMenu2.OpenMenu == null) && WholeUICanvas.ShowEnemyHealthBars && (GetDistanceFromMouse() < 0.25f || health < maxhealth || Player.GetComponent<PlayerBehaviour>().lockedon == this.gameObject) && EnableHealthBar)
            {
                _healthbar.gameObject.SetActive(true);
                _healthbar.transform.position = Camera.main.WorldToScreenPoint(this.transform.position) + new Vector3(0f, 20f, 0f);
                _healthbar.value = _health;
                _healthbar.maxValue = maxhealth;
                if (SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.EnemyHealthNumsEnabled))
                {
                    _healthbar.gameObject.GetComponent<EnemyHealthBar>().Sethealthnum(health + "/" + maxhealth);
                }
                _healthbar.gameObject.GetComponent<EnemyHealthBar>().Setlevelnum("Lv. " + Level + " " + EnemyName);
            }
            else
            {
                _healthbar.gameObject.SetActive(false);
            }
        }
	}

    protected void DestroyHealthBar()
    {
        if (_healthbar != null)
        {
            Destroy(_healthbar);
        }
    }

    protected void InstantiateHealthBar()
    {
        if (_healthbar == null)
        {
            _healthbar = Instantiate(HealthBarReference.GetComponent<Slider>(), mainui.transform);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Projectile") && !other.GetComponent<GenericWeapon>().WhiteListed(this.gameObject))
		{
            Instantiate(Resources.Load(FileDir.EnemyHitParticles) as GameObject, transform.position, transform.rotation, transform);
			GenericWeapon weapon = other.gameObject.GetComponent<GenericWeapon> ();
            int damagetaken = weapon.damage;
            if (!Aggroed)
            {
                damagetaken = (int)(damagetaken * (1 + ((SneakAttack)SkillManager.Skills[16]).DamageIncrease));                
            }
            TakeDamage(damagetaken);
            if (weapon.isPlayerWeapon)
            {
                Aggroed = true;
                CallDamagedByPlayer(damagetaken);
            }
		}
	}

    protected void CallDamagedByPlayer(int damagetaken)
    {
        if (DamagedByPlayer != null)
        {
            DamagedByPlayer(damagetaken);
        }
    }

    //What the enemy does after it has died
	protected void dead()
	{
        try
        {
            Minimap.RemoveMiscIcon(MapIcon);
        }
        catch { Debug.Log("No minimap"); }
		Destroy (GetComponent<Rigidbody>()); //removes the rigidbody of the object. This means physics no longer applies to the game object
        try
        {
            this.GetComponent<BoxCollider>().enabled = false;
        }
        catch
        {
            GetComponent<CapsuleCollider>().enabled = false;
        }
		_healthbar.transform.SetParent (this.transform);
        //Objective 1.3.2.2.5
        if (giveexp != null)
        {
            giveexp(_exp);
        }				
        foreach (GameObject g in GuaranteedDrops)
        {
            if (g != null)
            {
                GetComponent<ItemDrop>().DropItem(Instantiate(g), 60f, 0f);
            }
        }
        while (_DropChance > 100)
        {
            DropRandomItem();
            _DropChance -= 100;
        }
        int dropnum = UnityEngine.Random.Range(1, 100);
        if (dropnum >= (100 - DropChance))
		{
            DropRandomItem();
		}
        Destroy(DamageNumberMask);
        //Objective 1.3.2.2.3
        int goldamt = UnityEngine.Random.Range(MinGold, MaxGold);
        Player.GetComponent<PlayerInventory>().AddGold(goldamt);
    }

    protected virtual void OnCollisionEnter(Collision other)
	{
        //Objective 1.3.2.2.2
        if (other.gameObject.CompareTag("Player")) 
		{
            try
            {
                other.gameObject.GetComponent<PlayerBehaviour>().takedamage(damage);
                if (StatusEffects.Count > 0)
                {
                    foreach (Statuses.StatusType s in StatusEffects)
                    {
                        other.gameObject.GetComponent<Statuses>().ApplyStatus(s, this.gameObject);
                    }
                }
            }
            catch { Debug.Log("PlayerBehaviour or status may not be set up properly"); }
		}
        if (other.gameObject.CompareTag("Ground"))
        {
            if (gameObject.GetComponent<NavMeshAgent>() != null)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
            }
            OnGround = true;
        }
	}

    //Return a random number based on the total skew of the items in the drop table. This is the index of the item to be dropped
    protected int GetDropIndex()
    {
        int skewnum = UnityEngine.Random.Range(1, DropTable[DropTable.Count-1].SumSkew + 1);
        for (int a = 0; a < DropTable.Count; a++)
        {
            if (skewnum <= DropTable[a].SumSkew)
            {
                return a;
            }
        }
        return -1;
    }

    //Drop a random item. Initialise properties of that item if necessary
    //Objective 1.3.2.2.3.b
    protected void DropRandomItem()
    {
        //Objective 1.3.2.2.3.b.i
        int dropindex = GetDropIndex();
        if (DropTable[dropindex].Item.GetComponent<StackableItem>() != null)
        {
            DropTable[dropindex].Item.GetComponent<StackableItem>().ChangeAmount(UnityEngine.Random.Range(1, 4));
        }
        if (DropTable[dropindex].Item.GetComponent<RandomPickup>() != null)
        {
            DropTable[dropindex].Item.GetComponent<RandomPickup>().SetMultiplier(0.2f * Level);
        }           
        this.gameObject.GetComponent<ItemDrop>().DropItem(DropTable[dropindex].Item, 60f, 0f);
    }

    public void SetLevel(int i)
    {
        _Level = i;
        _health = maxhealth;
    }

    //Kills the enemy
    public void Kill()
    {
        _health = 0;
    }

    private float GetDistanceFromMouse()
    {
        Ray mousetoground = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;
        if (Physics.Raycast(mousetoground, out rayhit, 100f))
        {
            return Vector3.Distance(transform.position, rayhit.point);
        }
        else
        {
            return 99999999999999999;
        }
    }

    private void DisplayDamageNumber(int damage)
    {
        if (SettingsManager.GetBooleanSetting(SettingsManager.BooleanSettingsProperties.DisplayEnemyDamageNum))
        {
            GameObject text = Instantiate(Resources.Load(FileDir.EnemyDamageNum) as GameObject, DamageNumberMask.transform);
            text.GetComponent<FadingTMPro>().Setup(1f);
            text.GetComponent<TextMeshProUGUI>().text = damage.ToString();
            if (DamageNumberMask.transform.childCount >= 3)
            {
                Destroy(DamageNumberMask.transform.GetChild(0).gameObject);
            }
        }
    }

    //Data the enemy needs to save
    public virtual string GetSaveData()
    {
        //type, name, level, health, maxhealth, position, targettag, damage, exp, dropchance, enablehealthbar, aggroradius, mingold, maxgold, guaranteeddrops
        string data = string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}/{13}", GetType(), EnemyName, Level, health, maxhealth, transform.position, TargetTag, damage, exp, DropChance, EnableHealthBar, AggroRadius, MinGold, MaxGold, GuaranteedDrops);
        foreach (GameObject g in GuaranteedDrops)
        {
            if (g != null)
            {
                data += "/" + g.GetComponent<GenericItem>().GetSaveData();
            }           
        }
        return data;
    }

    //Sets the properties of the enemy according to the details given
    public virtual void SetDetails(string[] details)
    {
        EnemyName = details[1];
        _Level = PlayerSave.StringToInt(details[2]);
        _health = PlayerSave.StringToInt(details[3]);
        _basemaxhealth = PlayerSave.StringToInt(details[4]);
        string[] formattedpos = details[5].Replace("(", "").Replace(")", "").Trim().Split(",".ToCharArray()[0]);
        Vector3 pos = new Vector3(float.Parse(formattedpos[0]), float.Parse(formattedpos[1]), float.Parse(formattedpos[2]));
        transform.position = pos;
        TargetTag = details[6];
        _damage = PlayerSave.StringToInt(details[7]);
        _exp = PlayerSave.StringToInt(details[8]);
        _DropChance = PlayerSave.StringToInt(details[9]);
        EnableHealthBar = bool.Parse(details[10]);
        AggroRadius = PlayerSave.StringToInt(details[11]);
        MinGold = PlayerSave.StringToInt(details[12]);
        MaxGold = PlayerSave.StringToInt(details[13]);
        if (details.Length > 14)
        {
            for (int a = 14; a < details.Length - 1; a++)
            {
                _GuaranteedDrops.Add(PlayerSave.LoadItem(details[a].Split(",".ToCharArray()[0])));
            }
        }
        if (TargetTag != "" && TargetTag != null)
        {
            Target = GameObject.FindGameObjectWithTag(TargetTag);
        }
    }
}
