using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

//Objective 1.3.2.5.5
public class SortInvButton : MonoBehaviour {

	private GameObject player;
	private List<GameObject> inv;
    private bool SortByCategory;

    public delegate void UpdateInvEvent();
    public static event UpdateInvEvent UpdateInv;

    protected void Start()
	{
        SortByCategory = false;
		player = GameObject.FindGameObjectWithTag ("Player");
		this.gameObject.GetComponent<Button>().onClick.AddListener (SortInv);
        inv = player.GetComponent<PlayerInventory>().inventory;
    }

	//http://www.geeksforgeeks.org/quick-sort/
    void SortInv()
    {
        Sort(ref inv,0,inv.Count-1);
        //SortByCategory = !SortByCategory;
        UpdateInv();
    }

    //Recursive algorithm to handle sorting elements on either side of the pivot
    void Sort(ref List<GameObject> listtosort, int lowindex, int highindex)
    {
        if (lowindex < highindex)
        {
            int splitindex = Split(ref listtosort, lowindex, highindex);
            Sort(ref listtosort, lowindex, splitindex - 1);
            Sort(ref listtosort, splitindex + 1, highindex);
        }
    }

    //Returns the index of the pivot after items have been moved left or right
    int Split(ref List<GameObject> listtosort, int lowindex, int highindex)
    {
        string pivot;
        if (SortByCategory)
        {
            pivot = GenericItem.Categories[listtosort[highindex].GetComponent<GenericItem>().category].ToUpper().Replace(" ", "");
        }
        else
        {
            pivot = listtosort[highindex].GetComponent<GenericItem>().itemname.ToUpper().Replace(" ", "");
        }
        int lowestnumberindex = lowindex - 1;
        int charcount = 0;
        for (int a = lowindex; a <= highindex - 1; a++)
        {
            charcount = 0;
            bool CharsEqual = true;
            string currentword;
            if (SortByCategory)
            {
                currentword = GenericItem.Categories[listtosort[a].GetComponent<GenericItem>().category].ToUpper().Replace(" ", "");
            }
            else
            {
                currentword = listtosort[a].GetComponent<GenericItem>().itemname.ToUpper().Replace(" ", "");
            }
            do
            {
                if ((int)currentword[charcount] < (int)pivot[charcount])
                {
                    lowestnumberindex += 1;
                    SwapItems(ref listtosort, a, lowestnumberindex);
                    CharsEqual = false;
                }
                else if ((int)currentword[charcount] == (int)pivot[charcount])
                {
                    charcount += 1;
                }
                else if ((int)currentword[charcount] > (int)pivot[charcount])
                {
                    CharsEqual = false;
                }
            } while (charcount < (currentword.Length - 1) && charcount < (pivot.Length - 1) && CharsEqual);
        }
        SwapItems(ref listtosort, lowestnumberindex + 1, highindex);
        return (lowestnumberindex + 1);
    }

    void SwapItems(ref List<GameObject> listt, int firstitemindex, int seconditemindex)
    {
        GameObject temp = listt[firstitemindex];
        listt[firstitemindex] = listt[seconditemindex];
        listt[seconditemindex] = temp;
    }
}
