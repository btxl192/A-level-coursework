using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objective 1.3.2.8.3
public class KeyBindings : MonoBehaviour 
{

    private static Dictionary<string, string> _KeyBindings = new Dictionary<string, string>();
    public static Dictionary<string, string> KeyBinds
    {
        get { return _KeyBindings; }
    }

    private static List<RevKeyBind> RevKeyBindings = new List<RevKeyBind>();
    public static List<RevKeyBind> RevKeyBinds = new List<RevKeyBind>();

    public struct RevKeyBind
    {
        public string Key;
        public string Action;

        public RevKeyBind(string key, string action)
        {
            Key = key;
            Action = action;
        }
    }

    void Awake()
	{
        try
        {
            _KeyBindings.Add("Interact", "E");
            _KeyBindings.Add("Shoot", "Mouse0");
            _KeyBindings.Add("LockOn", "Mouse1");
            _KeyBindings.Add("Inventory", "I");
            _KeyBindings.Add("Pause", "esc");
            _KeyBindings.Add("CmdLine", "`");
            foreach (string s in KeyBinds.Keys)
            {
                RevKeyBindings.Add(new RevKeyBind(KeyBinds[s], s));
            }
        }
        catch { }
    }

    //Checks if the key assigned to the action specified has been pressed
    public static bool KeyPressed(string Action)
    {
        try
        {
            return Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), KeyBinds[Action]));
        }
        catch
        {
            return false;
        }
    }

    //Add an action to the key bindings
    public static void AddAction(string action)
    {
        try
        {
            _KeyBindings.Add(action, "");
            RevKeyBindings.Clear();
            foreach (string s in KeyBinds.Keys)
            {
                RevKeyBindings.Add(new RevKeyBind(KeyBinds[s], s));
            }
        }
        catch
        {
            print(action + " could not be addded to the key bindings");
        }
    }

    //Check if the specified key is bound to an action
    public static bool IsKeyBound(string key)
    {
        foreach (string s in KeyBinds.Values)
        {
            if (s == key.ToUpper() || s == key)
            {
                return true;
            }
        }
        return false;
    }

}
