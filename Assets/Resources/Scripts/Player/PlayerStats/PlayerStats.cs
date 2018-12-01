using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public struct FlatBonus
    {
        public string Identifier;
        public int Amount;

        public FlatBonus(string identifier, int amount)
        {
            Identifier = identifier;
            Amount = amount;
        }
    }

    public struct MultBonus
    {
        public string Identifier;
        public float Amount;

        public MultBonus(string identifier, float amount)
        {
            Identifier = identifier;
            Amount = amount;
        }
    }

    public PlayerHealth HP
    {
        get
        {
            return this.gameObject.GetComponent<PlayerHealth>();
        }
    }
    public PlayerMana MP
    {
        get
        {
            return this.gameObject.GetComponent<PlayerMana>();
        }
    }
    public PlayerCombat Combat
    {
        get
        {
            return this.gameObject.GetComponent<PlayerCombat>();
        }
    }
    public PlayerArmour ArmourVals
    {
        get
        {
            return this.gameObject.GetComponent<PlayerArmour>();
        }
    }

    public int exp {get; private set; }
    //Objective 1.3.2.7.5.b
    public int maxexp
    {
        get
        {
            return (int)((float)100 * Mathf.Pow(1.25f, level - 1));
        }
    }
    public int level {get; private set; }
    public int skillpoints {get; private set; }

    public delegate void BlankEvent();
    public static event BlankEvent CheckStats;
    public static event BlankEvent UpdateExp;

    private Queue<int> ExpQueue = new Queue<int>();
    public delegate void addexpevent(int i);
    public static event addexpevent addexp;

    [SerializeField]
    private TMPro.TextMeshProUGUI sptext;
        
    void Start()
    {
        GenericEnemy.giveexp += receiveexp;
        ExpBar.increaselevel += increaselevel;
        Combat.UpdateDamage(this.gameObject.GetComponent<PlayerEquipment>().WeaponObject);
        CheckStats();
        if (UpdateExp != null)
        {
            UpdateExp();
        }
        UpdateSPtext();
    }

    private void OnDestroy()
    {
        GenericEnemy.giveexp -= receiveexp;
        ExpBar.increaselevel -= increaselevel;
    }

    //Dequeues an item from the experience points queue 
    private void Update()
    {
        if (ExpQueue.Count > 0)
        {
            addexp(ExpQueue.Dequeue());
        }
    }

    //Add exp to the player
    public void receiveexp(int e)
    {
        exp += e;
        IngameLog.Log("Gained " + e + " exp", Color.yellow);
        ExpQueue.Enqueue(e);      
    }

    //Increase the player's level and updates the player's stats
    //Objective 1.3.2.7.5.a
    public void increaselevel()
    {
        exp -= maxexp;
        if (exp < 0)
        {
            exp = 0;
        }
        level += 1;
        ChangeSkillPoints(3);
        Combat.UpdateDamage(null);
        CheckStats();
    }

    public void ChangeSkillPoints(int amount)
    {
        skillpoints += amount;
        UpdateSPtext();
    }

    public void SetSkillPoints(int amount)
    {
        skillpoints = amount;
        UpdateSPtext();
    }

    //Updates the text that tells the player how many unused skill points they have
    private void UpdateSPtext()
    {
        if (skillpoints > 0)
        {
            sptext.text = string.Format("You have {0} unused skill points!", skillpoints);
        }
        else
        {
            sptext.text = "";
        }
    }

    public void SetLevel(int i)
    {
        level = i;
        UpdateExp();
    }

    public void SetExp(int i)
    {
        exp = i;
        UpdateExp();
    }
}
