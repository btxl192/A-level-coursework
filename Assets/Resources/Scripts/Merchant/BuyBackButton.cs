using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyBackButton : MonoBehaviour {

    public delegate void MerchantMode(MerchantUI.Mode m);
    public static event MerchantMode SwitchBuyBack;

    void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(SwitchToBuyBack);
        MerchantUI.DeselectButtons += Deselected;
    }

    //Switch the merchant's UI to the BuyBack mode
    void SwitchToBuyBack()
    {
        SwitchBuyBack(MerchantUI.Mode.BuyBack);
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

    void OnDestroy()
    {
        MerchantUI.DeselectButtons -= Deselected;
    }
}
