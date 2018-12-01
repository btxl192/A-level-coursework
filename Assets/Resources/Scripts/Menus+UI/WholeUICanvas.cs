using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WholeUICanvas : MonoBehaviour
{
    [SerializeField]
    private bool _ShowEnemyHealthBars;
    public static bool ShowEnemyHealthBars { get; private set; }

    void Awake()
    {
        ShowEnemyHealthBars = _ShowEnemyHealthBars;
    }

}
