using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {

	private float FadePeriod;
	private Material thismaterial;
    private bool ToDestroy;
    private Image thisimage;

    private void Start()
    {
        if (gameObject.GetComponent<FadeIn>() != null)
        {
            Destroy(gameObject.GetComponent<FadeIn>());
        }
        try
        {
            thismaterial = this.gameObject.GetComponent<Renderer>().material;
        }
        catch
        {
            try
            {
                thisimage = this.gameObject.GetComponent<Image>();
            }
            catch
            {
                Debug.Log("FADING SCRIPT ONLY WORKS ON IMAGES OR MATERIALS");
            }
        }
    }

    void Update () 
	{
        if (thismaterial != null)
        {
            //Lowers the alpha of the object's colour
            thismaterial.color = new Color(thismaterial.color.r, thismaterial.color.g, thismaterial.color.b, thismaterial.color.a - (1 / FadePeriod) * Time.deltaTime);
            //When the object is no longer visible, destroy this component. Destroy the object if ToDestroy is true
            if (thismaterial.color.a <= 0)
            {
                if (ToDestroy)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Destroy(this);
                }
            }
        }
        else if (thisimage != null)
        {
            //Lowers the alpha of the object's colour
            thisimage.color = new Color(thisimage.color.r, thisimage.color.g, thisimage.color.b, thisimage.color.a - (1 / FadePeriod) * Time.deltaTime);
            //When the object is no longer visible, destroy this component. Destroy the object if ToDestroy is true
            if (thisimage.color.a <= 0)
            {
                if (ToDestroy)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Destroy(this);
                }
            }
        }
	}

    public void Initialise(float fadeperiod, bool todestroy)
    {
        FadePeriod = fadeperiod;
        ToDestroy = todestroy;
    }
}
