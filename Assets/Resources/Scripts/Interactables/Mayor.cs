using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mayor : InteractableWorldSwitch {

    private bool Updated;
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
            if (GenericMenu2.OpenMenu == null)
            {
                Text.gameObject.SetActive(true);
            }
            else
            {
                Text.gameObject.SetActive(false);
            }
        }
        if (!WorldFlags.Flags["SpectreDefeated"] && !Interacted)
        {
            Text.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0f, 50f, 0f);
        }
        else
        {
            Destroy(Text);
        }
        if (!Updated)
        {
            Updated = true;
            if (WorldFlags.Flags["SpectreDefeated"])
            {
                _Dialog = new List<string>()
                {
                    { "THANKS FOR PLAY TESTING!!!"}
                };
                ToSwitch.SetActive(true);
            }
            else
            {
                _Dialog = new List<string>()
                {
                    { "Bad cubes are raiding our magic library! Please help stop them!"},
                    { "*You now have access to the library*"}
                };
            }
        }
    }

    protected override void Interact()
    {
        base.Interact();
        Interacted = true;
    }
}
