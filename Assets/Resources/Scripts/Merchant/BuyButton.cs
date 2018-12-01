using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour {

    public delegate void MerchantMode(MerchantUI.Mode m);
    public static event MerchantMode SwitchBuy;

	void Awake () 
	{
        this.GetComponent<Button>().onClick.AddListener(SwitchToBuy);
        MerchantUI.DeselectButtons += Deselected;
        MerchantUI.SetBuyYellow += Selected;
	}

    //Switch the merchant's UI to the Buy mode
    void SwitchToBuy()
    {
        SwitchBuy(MerchantUI.Mode.Buy);
        Selected();
    }

    //If the button is clicked, change the colour to yellow
    void Selected()
    {
        this.GetComponent<Image>().color = Color.yellow;
    }

    //Change the colour to white when another button is clicked
    void Deselected()
    {
        this.GetComponent<Image>().color = Color.white;
    }

    private void OnDestroy()
    {
        MerchantUI.DeselectButtons -= Deselected;
        MerchantUI.SetBuyYellow -= Selected;
    }
}
