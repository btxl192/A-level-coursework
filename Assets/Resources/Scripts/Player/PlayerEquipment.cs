using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerEquipment : MonoBehaviour {

    [SerializeField]
    private GameObject _WeaponObject;
    public GameObject WeaponObject
    {
        get
        {
            return _WeaponObject;
        }
        set
        {
            _WeaponObject = value;
        }
    }

	public GenericWeapon Weapon
	{
		get
        {
            try
            {
                return WeaponObject.GetComponent<GenericWeapon>();
            }
            catch
            {
                return null;
            }
        }
	}

	[SerializeField]
	public GameObject SkillCore
	{
		get;
		private set;
	}
	[SerializeField]
	public GameObject ArmourObject
	{
		get;
		private set;
	}
	public GenericArmour Armour
	{
		get
        {
            try
            {
                return ArmourObject.GetComponent<GenericArmour>();
            }
            catch
            {
                return null;
            }
        }
	}

	public delegate void ChangedEq(GameObject eq);
	public static event ChangedEq ChangedWeapon;
    public static event ChangedEq ChangedArmour;
    public static event ChangedEq ChangedSkillCore;

    void Awake () 
	{
		if (WeaponObject == null)
		{
			WeaponObject = Resources.Load(FileDir.BasicProjectile) as GameObject;
		}
	}

    //Unequip a piece of equipment based on its type. Events depending on the item type are broadcasted saying that there has been a change
    public void UnEquip(System.Type T, out GameObject PrevEquipment)
    {
        if (T == typeof(GenericWeapon))
        {
            try
            {
                PrevEquipment = WeaponObject;
            }
            catch { PrevEquipment = null; }
            WeaponObject = null;
            ChangedWeapon(null);
        }
        else if (T == typeof(GenericArmour))
        {
            try
            {
                PrevEquipment = ArmourObject;
            }
            catch { PrevEquipment = null; }
            ArmourObject = null;
            ChangedArmour(null);
        }
        else if (T == typeof(SkillCore))
        {
            try
            {
                PrevEquipment = SkillCore;
            }
            catch { PrevEquipment = null; }
            SkillCore = null;
            ChangedSkillCore(null);
        }
        else
        {
            PrevEquipment = null;
        }
    }

    //Equip or swap out a piece of equipment based on its type. Events depending on the item type are broadcasted saying that there has been a change
	public void Equip(GameObject Equipment, out GameObject PrevEquipment)
	{
		if(Equipment.GetComponent<GenericWeapon>() != null)
		{
            try
            {
                PrevEquipment = WeaponObject;
            }
            catch { PrevEquipment = null; }
			WeaponObject = Equipment;
            if (ChangedWeapon != null)
            {
                ChangedWeapon(Equipment);
            }			
		}
		else if(Equipment.GetComponent<GenericArmour>() != null)
		{
            try
            {
                PrevEquipment = ArmourObject;
            }
            catch { PrevEquipment = null; }
			ArmourObject = Equipment;
            if (ChangedArmour != null)
            {
                ChangedArmour(Equipment);
            }           
		}
		else if(Equipment.GetComponent<SkillCore>() != null)
		{
            try
            {
                PrevEquipment = SkillCore;
            }
            catch { PrevEquipment = null; }
			SkillCore = Equipment;
            if (ChangedSkillCore != null)
            {
                ChangedSkillCore(Equipment);
            }            
		}
		else
		{
			PrevEquipment = null;
		}
	}
}
