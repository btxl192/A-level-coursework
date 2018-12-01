using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

	private float _atttimer;
    [SerializeField]
    private bool dead;

    [SerializeField]
    public GameObject test;

	public GameObject lockedon { get; set; }

    private bool invul;

    private PlayerStats PStats;
	private PlayerEquipment Eq;

	GameObject _Interacting;
	public GameObject Interacting
	{
		get {return _Interacting;}
		set
        {
			try
            {
                _Interacting = value;
            }
			catch
            {
                Debug.Log("ERROR in PlayerBehaviour: couldn't set interacting value");
            }
        }
	}

	void Start () 
	{
        PStats = this.gameObject.GetComponent<PlayerStats>();
		Eq = this.gameObject.GetComponent<PlayerEquipment>();
        test = Instantiate(Resources.Load(FileDir.BasicProjectile) as GameObject);
        if (dead == false && PStats.HP.health == 0)
        {
            PStats.HP.SetHP(PStats.HP.maxhealth);
            PStats.MP.SetMP(PStats.MP.maxmana);
        }
    }

    void Update()
	{
        if (!Pause.Paused)
		{
            //Objective 1.3.2.7.7
			if (PStats.HP.health <= 0 && PlayerSave.Loaded) //Sets the player as dead if their health reaches 0
			{
				this.gameObject.GetComponent<PlayerMovement>().enabled = false;
                RespawnPoint.ShowRespawnCanvas();
                dead = true;
			}
			else
			{
				if (_atttimer <= PStats.Combat.attspeed) //counter used to keep track of the last time the player attacked. only increments if the last time the player attacked is less than the attack speed to ensure no overflows happen
				{
					_atttimer += Time.deltaTime;
				}
                //Objective 1.3.2.7.1
				if (KeyBindings.KeyPressed("Shoot") && _atttimer >= PStats.Combat.attspeed && !MerchantUI.CurrentlyOpen && GenericMenu2.OpenMenu == null) //checks if the player can attack yet and if the fire button is pressed
				{
					_atttimer = 0;
                    try
                    {
                        //Loading the weapon
                        GameObject wpn = Instantiate(Eq.WeaponObject);
                        wpn.transform.localScale = (Resources.Load(FileDir.BasicProjectile) as GameObject).transform.localScale;
                        if (SkillManager.Skills[0].Level > 0 && Random.Range(0f, 1f) > ((Toxic)SkillManager.Skills[0]).PoisonChance)
                        {
                            wpn.GetComponent<GenericWeapon>().AddStatus(Statuses.MakeStatus(Statuses.statuses.Poison, SkillManager.Skills[0].Level));
                        }
                        //Shooting the weapon
                        Eq.Weapon.GetComponent<GenericProjectile>().Shoot(wpn, this.gameObject, lockedon, PStats.Combat.damage, true);
                        Destroy(wpn);
                    }
                    catch
                    {
                        TempPopup.Show("No weapon equipped!", Color.red);
                        throw;
                    }
				}
			} 
		}

	}

    //Starts the playerhit coroutine
	public void takedamage(int d)
	{
		StartCoroutine (playerhit (d));
	}

    //Decrease the player's health and makes them invulnerable for one second
    //Objective 1.3.2.7.10
	IEnumerator playerhit(int d)
	{
		if (!invul)
		{
            Color temp = this.GetComponent<Renderer>().material.color;
            PStats.HP.ChangeHealth(-PStats.ArmourVals.GetResultantDamage(d));
            invul = true;
            this.GetComponent<Renderer>().material.color = new Color(0f, 255f, 0f);
            yield return new WaitForSeconds(1f);
            invul = false;
            this.GetComponent<Renderer>().material.color = temp;
        }
	}
}
