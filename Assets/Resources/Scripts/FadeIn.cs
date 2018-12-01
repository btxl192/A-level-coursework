using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

    private float FadePeriod;
    private Material thismaterial;
    private Image thisimage;

    private void Start()
    {
        if (gameObject.GetComponent<FadeOut>() != null)
        {
            Destroy(gameObject.GetComponent<FadeOut>());
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
            //Increase the alpha value of the object's colour
            thismaterial.color = new Color(thismaterial.color.r, thismaterial.color.g, thismaterial.color.b, thismaterial.color.a + (1 / FadePeriod) * Time.deltaTime);
            //Removes this component if the object is fully opaque
            if (thismaterial.color.a >= 1)
            {
                Destroy(this);
            }
        }
        else if (thisimage != null)
        {
            //Increase the alpha value of the object's colour
            thisimage.color = new Color(thisimage.color.r, thisimage.color.g, thisimage.color.b, thisimage.color.a + (1 / FadePeriod) * Time.deltaTime);
            //Removes this component if the object is fully opaque
            if (thisimage.color.a >= 1)
            {
                Destroy(this);
            }
        }
    }

    public void Initialise(float fadeperiod)
    {
        FadePeriod = fadeperiod;
    }
}
