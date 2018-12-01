using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PageCount : MonoBehaviour {

	private static int _PageNum;
	public static int PageNum
	{
		get {return _PageNum;}
	}
	private int MaxPage;
	public delegate void UpdateInvEvent();
	public static event UpdateInvEvent UpdateInv;

	protected void Start () 
	{
		_PageNum = 1;
		MaxPage = 5;
		NextPageButton.NextPage += NextPage;
		PrevPageButton.PrevPage += PrevPage;
		UpdatePageCount();
	}

    //Go to the next page. Loop back around if on last page
	void NextPage()
	{
		if (PageNum < MaxPage)
		{
			_PageNum += 1;
		}
		else
		{
			_PageNum = 1;
		}
		UpdatePageCount();
		UpdateInv ();
	}

    //Go to the previous page. Loop back around if on first page
	void PrevPage()
	{
		if (PageNum != 1)
		{
			_PageNum -= 1;
		} 
		else
		{
			_PageNum = MaxPage;
		}
		UpdatePageCount();
		UpdateInv ();
	}

    //Update the page count text
	void UpdatePageCount()
	{
		this.gameObject.GetComponent<TextMeshProUGUI>().text = PageNum + "/" + MaxPage;
	}

    private void OnDestroy()
    {
        NextPageButton.NextPage -= NextPage;
        PrevPageButton.PrevPage -= PrevPage;
    }
}
