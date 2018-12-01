using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericItem : MonoBehaviour {

    public enum Rarities
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

	public enum CategoryEnum
	{
		Item,
		Projectile,
		HealingItem,
		Armour,
		SkillCore,
		Weapon,
        EnemyProjectile,
        Tome
	}

	public static Dictionary<CategoryEnum, string> Categories = new Dictionary<CategoryEnum,string>()
	{
		{CategoryEnum.Item, "Item"},
		{CategoryEnum.Projectile, "Projectile"},
		{CategoryEnum.HealingItem, "Healing Item"},
		{CategoryEnum.Armour, "Armour"},
		{CategoryEnum.SkillCore, "Skill Core"},
		{CategoryEnum.Weapon, "Weapon"},
        {CategoryEnum.EnemyProjectile, "Enemy Projectile"},
        {CategoryEnum.Tome, "Tome"}
    };

    public static Color GetRarityColour(Rarities r)
    {
        switch (r)
        {
            case Rarities.Rare:
                return Color.green;
            case Rarities.Epic:
                return Color.magenta;
            case Rarities.Legendary:
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    public static Dictionary<CategoryEnum, Sprite> ItemTypeIcon = new Dictionary<CategoryEnum, Sprite>();

	[SerializeField]
    protected string _itemname;
	public string itemname
	{
		get {return _itemname; }
	}

    [SerializeField]
    protected string _desc;
	public string desc
	{
		get {return _desc; }
	}

	[SerializeField]
	protected CategoryEnum _category;
	public CategoryEnum category
	{
		get {return _category; }
		protected set {_category = value;}
	}

    [SerializeField]
    protected Rarities _Rarity;
    public Rarities Rarity
    {
        get { return _Rarity; }
        set { _Rarity = value; }
    }

    [SerializeField]
    protected int _Price;
    public int Price
    {
        get
        {
            if (transform.parent!= null)
            {
                if (transform.parent.GetComponent<PlayerBehaviour>() != null)
                {
                    return (int)(_Price * 0.5f);
                }
                else
                {
                    return _Price;
                }
            }
            else
            {
                return _Price;
            }           
        }
    }

    public virtual string FormattedDesc
    {
        get
        {
            string d = itemname;
            if (desc != "")
            {
                d += "\n\n" + desc;
            }            
            d += "\n\nRarity: " + Rarity;
            d += "\nSelling price: " + Price + " gold";
            return d;
        }
    }

    protected virtual void Awake()
    {
        if (ItemTypeIcon.Count == 0)
        {
            ItemTypeIcon.Add(CategoryEnum.Armour, Resources.Load<Sprite>(FileDir.ArmourIcon));
            ItemTypeIcon.Add(CategoryEnum.HealingItem, Resources.Load<Sprite>(FileDir.HealingItemIcon));
            ItemTypeIcon.Add(CategoryEnum.Projectile, Resources.Load<Sprite>(FileDir.ProjectileIcon));
            ItemTypeIcon.Add(CategoryEnum.SkillCore, Resources.Load<Sprite>(FileDir.SkillCoreIcon));
            ItemTypeIcon.Add(CategoryEnum.Item, Resources.Load<Sprite>(FileDir.ItemIcon));
            ItemTypeIcon.Add(CategoryEnum.Tome, Resources.Load<Sprite>(FileDir.TomeIcon));
        }
    }

    public void SetCategory(CategoryEnum Category)
    {
        category = Category;
    }

    public void SetDetails(string ItemName, string ItemDesc)
    {
        _itemname = ItemName;
        _desc = ItemDesc;
    }

    public void SetDetails(string ItemName, string ItemDesc, Rarities rarity, int price)
    {
        _itemname = ItemName;
        _desc = ItemDesc;
        _Rarity = rarity;
        _Price = price;
    }

    public virtual string GetSaveData()
    {
        return category + "," + itemname + "," + desc + "," + Rarity + "," + Price;
    }
}

