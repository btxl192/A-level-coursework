using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldFlags : MonoBehaviour {

    public static Dictionary<string, bool> Flags = new Dictionary<string, bool>()
    {
        { "SpectreDefeated", false }
    };
}
