using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartBook : Interactable {

    [SerializeField]
    private GameObject Boss;
    private TextMeshProUGUI Text;
    private bool Interacted;

    private void Start()
    {
        Text = Instantiate(Resources.Load(FileDir.TextMeshPro) as GameObject, GameObject.FindGameObjectWithTag("MainUI").transform).GetComponent<TextMeshProUGUI>();
        Text.text = "!";
        Text.color = Color.yellow;

    }
    protected override void Update()
    {
        base.Update();
        if (Text != null)
        {
            if (GenericMenu2.OpenMenu == null && !Interacted)
            {
                Text.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0f, 50f, 0f);
                Text.gameObject.SetActive(true);
            }
            else
            {
                Text.gameObject.SetActive(false);
            }
        }
    }
    protected override void Interact()
    {
        Boss.SetActive(true);
        Interacted = true;
    }
}
