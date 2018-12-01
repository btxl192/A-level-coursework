using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CheatInput : MonoBehaviour {

    private string UserInput;
    public static readonly Dictionary<string, string> CommandDescriptions = new Dictionary<string, string>()
    {
        {"/addskillpoints", "Adds health by the amount specified. /addskillpoints <amount>" },
        {"/addcurrency", "Adds currency by the amount specified. /addcurrency <amount>" },
        {"/changehealth", "Changes health by the amount specified. /changehealth <amount>" },
        {"/addexp", "Adds exp by the amount specified. /addexp <amount>" },
    };
	
	void Update ()
    {
        //Parse the user's input
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UserInput = this.GetComponent<InputField>().text;
            string[] UserInputParts = UserInput.Split(" ".ToCharArray()[0]);
            if (UserInputParts[0] == "/addskillpoints")
            {
                AddSkillPoints(UserInputParts[1]);
            }
            else if (UserInputParts[0] == "/addcurrency")
            {
                AddCurrency(UserInputParts[1]);
            }
            else if (UserInputParts[0] == "/changehealth")
            {
                ChangeHealth(UserInputParts[1]);
            }
            else if (UserInputParts[0] == "/addexp")
            {
                AddExp(UserInputParts[1]);
            }
            else
            {
                IngameLog.Log("No such command as '" + UserInputParts[0] + "'", Color.red);
            }
        }
	}

    //Adds skillpoints for the player
    void AddSkillPoints(string points)
    {
        try
        {
            PlayerSave.staticplayer.GetComponent<PlayerStats>().ChangeSkillPoints(int.Parse(points));
            IngameLog.Log("Added " + points + " skill points", Color.white);
        }
        catch
        {
            IngameLog.Log("Please input a proper amount of skill points!", Color.red);
        }
    }

    //Adds gold for the player
    void AddCurrency(string amount)
    {
        try
        {
            PlayerSave.staticplayer.GetComponent<PlayerInventory>().AddGold(int.Parse(amount));
        }
        catch
        {
            TempPopup.Show("Please input a proper amount of gold!", Color.red);
        }
    }

    //Change the player's health
    void ChangeHealth(string amount)
    {
        try
        {
            PlayerSave.staticplayer.GetComponent<PlayerStats>().HP.ChangeHealth(int.Parse(amount));
            IngameLog.Log("Changed health by " + amount + " points", Color.white);
        }
        catch
        {
            IngameLog.Log("Please input an integer!", Color.red);
        }
    }

    //Adds experience points
    void AddExp(string amount)
    {
        try
        {
            PlayerSave.staticplayer.GetComponent<PlayerStats>().receiveexp(int.Parse(amount));
        }
        catch
        {
            IngameLog.Log("Please input an integer!", Color.red);
        }
    }
}
