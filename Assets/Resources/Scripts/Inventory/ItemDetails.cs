using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails : MonoBehaviour {

	public delegate void PassEqEvent(int i);
	public static event PassEqEvent PassEq;

	public delegate void ChangeTextEvent(string t);
	public static event ChangeTextEvent ChangeText;
	
	private Text WholeText;
	private Text textPrefab;

	protected void Start () 
	{
		textPrefab = (Resources.Load (FileDir.Text) as GameObject).GetComponent<Text>();
		InvSlot.DisplayDetails += ShowDetails;
		EquipButton.UpdateDetails += ShowDetails;
		WholeText = Instantiate(textPrefab, this.transform);        
        WholeText.rectTransform.anchorMax = new Vector2(1f, 1f);
        WholeText.rectTransform.anchorMin = new Vector2(0f, 0f);
        WholeText.rectTransform.anchoredPosition = Vector3.zero;
        WholeText.rectTransform.sizeDelta = Vector3.zero;
        WholeText.rectTransform.offsetMin = new Vector2(5f, 0f);
        WholeText.rectTransform.offsetMax = new Vector2(0f, -5f);
    }

    private void OnDestroy()
    {
        InvSlot.DisplayDetails -= ShowDetails;
        EquipButton.UpdateDetails -= ShowDetails;
    }

    //Display the item's formatted description and change the text of the equip button according to the item's category
    void ShowDetails(GameObject i, int j)
	{
		if (i != null)
		{
			Clear();
            string category = GenericItem.Categories[i.GetComponent<GenericItem>().category];
            WholeText.text = i.GetComponent<GenericItem>().FormattedDesc;
            switch (category)
            {
                case "Projectile":
                case "Armour":
                case "Skill Core":
                    ChangeText("Equip");
                    break;
                case "Healing Item":
                case "Tome":
                    ChangeText("Use");
                    break;
                default:
					ChangeText("");
					break;
            }
            PassEq (j);
		} 
		else
		{
			Clear ();
			ChangeText("");
		}
	}

	void Clear()
	{
		WholeText.text = "";
	}
}
