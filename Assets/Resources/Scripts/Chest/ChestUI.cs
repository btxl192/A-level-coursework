using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour {

    [SerializeField]
    private int MaxInRow = 5;
    [SerializeField]
    private List<GameObject> Rows = new List<GameObject>();

    public void AddRow()
    {
        Rows.Add(Instantiate(Resources.Load(FileDir.ChestRow) as GameObject, transform));
    }

    public void AddButton(GenericItem item, int index, GameObject currentchest)
    {
        SetPosition();
        Sprite itemtypeicon = GenericItem.ItemTypeIcon[item.category];
        if (Rows.Count < 1)
        {
            AddRow();
        }
        GameObject temp;
        if (Rows[Rows.Count-1].transform.childCount >= MaxInRow)
        {
            AddRow();
        }
        temp = Instantiate(Resources.Load(FileDir.ChestButton) as GameObject, Rows[Rows.Count-1].transform);
        temp.GetComponentInChildren<Button>().image.sprite = itemtypeicon;
        temp.transform.Find("Outline").GetComponent<Image>().color = GenericItem.GetRarityColour(item.Rarity);
        temp.GetComponent<HoverText>().SetText(item.FormattedDesc);
        temp.GetComponent<ChestButton>().SetIndex(index, currentchest);
    }

    public void Clear()
    {
        foreach (GameObject g in Rows)
        {
            Destroy(g);
        }
        Rows.Clear();

    }

    void SetPosition()
    {
        this.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        this.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
    }
}
