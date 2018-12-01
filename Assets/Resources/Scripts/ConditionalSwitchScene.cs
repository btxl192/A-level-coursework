using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalSwitchScene : SwitchScene {

    [SerializeField]
    private bool CanSwitch;
    [SerializeField]
    private string Identifier;
    private Color OriginalColor;
    private string OriginalText;

    private void Start()
    {
        OriginalColor = GetComponentInChildren<ParticleSystem>().main.startColor.color;
        OriginalText = PopupText;
        Level1Boss.Defeated += EnableSwitch;
        if (!CanSwitch)
        {
            SetParticles(false);
        }       
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Level1Boss.Defeated -= EnableSwitch;
    }

    //Enables the portal
    public void EnableSwitch(string identifier)
    {
        if (identifier == Identifier)
        {
            CanSwitch = true;
            SetParticles(true);
        }        
    }

    //Disables the portal
    public void DisableSwitch(string identifier)
    {
        if (identifier == Identifier)
        {
            CanSwitch = false;
            SetParticles(false);
        }       
    }

    protected override void Interact()
    {
        if (CanSwitch)
        {
            base.Interact();
        }       
    }

    //Change the colour of the particles and the text of the interactable's popup
    public void SetParticles(bool b)
    {
        foreach (Transform p in transform)
        {
            if (p.GetComponent<ParticleSystem>() != null)
            {
                var main = p.GetComponent<ParticleSystem>().main;
                if (!b)
                {
                    PopupText = "Something is interfering with the portal";
                    main.startColor = Color.red;
                }
                else
                {
                    PopupText = OriginalText;
                    main.startColor = OriginalColor;
                    TempPopup.Show("A portal has been enabled!", Color.green);
                }
                
            }
        }
    }

}
