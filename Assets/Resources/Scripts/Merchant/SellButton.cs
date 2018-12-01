using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour {

    public delegate void MerchantMode(MerchantUI.Mode m);
    public static event MerchantMode SwitchSell;

    void Awake ()
    {
        this.GetComponent<Button>().onClick.AddListener(SwitchToSell);
        MerchantUI.DeselectButtons += Deselected;
    }

    //Switch the merchant's UI to the Sell mode
    void SwitchToSell()
    {
        SwitchSell(MerchantUI.Mode.Sell);
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
    }
}
