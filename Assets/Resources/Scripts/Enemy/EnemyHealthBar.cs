using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI healthnum;
    [SerializeField]
    private TextMeshProUGUI levelnum;

    private void Awake()
    {
        Graphics.EnableEnemyHealthNums += EnableNums;
        Graphics.DisableEnemyHealthNums += DisableNums;
    }

    void OnDestroy()
    {
        Graphics.EnableEnemyHealthNums -= EnableNums;
        Graphics.DisableEnemyHealthNums -= DisableNums;
    }

    public void Sethealthnum(string healthnumvalue)
    {
        healthnum.text = healthnumvalue;
    }

    public void Setlevelnum(string levelnumvalue)
    {
        levelnum.text = levelnumvalue;
    }

    public void DisableNums()
    {
        healthnum.gameObject.SetActive(false);
    }

    public void EnableNums()
    {
        healthnum.gameObject.SetActive(true);
    }
}
