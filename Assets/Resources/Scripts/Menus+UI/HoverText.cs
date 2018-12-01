using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverText : MonoBehaviour {

    [SerializeField]
	private bool MouseHovering;
    [SerializeField]
	private GameObject hovertext;
    private Vector3 MouseOffset;
    private bool Initialised;

	void Awake () 
	{
        Initialise();
    }

    //Initialise properties
    void Initialise()
    {
        if (!Initialised)
        {
            GenericMenu2.OnClose += MouseExit;
            hovertext = Instantiate(Resources.Load(FileDir.HoverText) as GameObject, GameObject.FindGameObjectWithTag("MainUI").transform);
            hovertext.SetActive(false);
            Initialised = true;
        }
    }

    //Every frame, check if the mouse is hovering over it 
    void Update()
    {
        if (MouseHovering)
        {
            hovertext.GetComponent<RectTransform>().position = Input.mousePosition + MouseOffset;
        }
        else
        {
            hovertext.SetActive(false);
        }

    }

    //When the mouse is over the UI element
	public void MouseOver()
	{
        SetSize();
        hovertext.SetActive(true);
        MouseHovering = true;
    }

    //When the mouse exits the UI element
    public void MouseExit(GameObject g)
	{
        MouseHovering = false;
        if (hovertext != null)
        {
            hovertext.SetActive(false);
        }
    }

	public void SetText(string Textt)
	{
        Initialise();
        hovertext.GetComponentInChildren<Text>().text = Textt;
        SetSize();
    }

    //Resizes the popup
    void SetSize()
    {
        Text textHoverText = hovertext.GetComponentInChildren<Text>();
        if (textHoverText.preferredWidth > Screen.width * 0.3f)
        {
            textHoverText.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.2f, 0f);
        }
        else
        {
            textHoverText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(textHoverText.preferredWidth, 0f);
        }
        textHoverText.GetComponent<RectTransform>().sizeDelta = new Vector2(textHoverText.GetComponent<RectTransform>().sizeDelta.x, textHoverText.preferredHeight);
        hovertext.GetComponent<RectTransform>().sizeDelta = new Vector2(textHoverText.GetComponent<RectTransform>().sizeDelta.x, textHoverText.GetComponent<RectTransform>().sizeDelta.y);
        if (Input.mousePosition.x > Screen.width * 0.5f)
        {
            hovertext.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.5f);
            hovertext.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0.5f);
            hovertext.GetComponent<RectTransform>().pivot = new Vector2(1f, 0.5f);
            MouseOffset = new Vector3(-5f, 0f, 0f);
        }
        else
        {
            hovertext.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0.5f);
            hovertext.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.5f);
            hovertext.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
            MouseOffset = new Vector3(10f, 0f, 0f);
        }
        GameObject outline = hovertext.transform.Find("Outline").gameObject;
        outline.GetComponent<RectTransform>().offsetMin = new Vector2(-5f, -5f);
        outline.GetComponent<RectTransform>().offsetMax = new Vector2(5f, 5f);
        if (hovertext.GetComponent<RectTransform>().sizeDelta.x == 0)
        {
            outline.SetActive(false);
        }
        else
        {
            outline.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        Destroy(hovertext);
    }
}
