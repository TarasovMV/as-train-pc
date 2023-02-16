using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class toggleMain : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int id;
    public GameObject textLabel;

    public Color ActiveTextColor;
    public Color DefaultTextColor;

    public bool isCircle;

    private System.Action toggleAction;

    void Start ()
    {
	}

	void FixedUpdate ()
    {
        if (isCircle)
        {
            return;
        }

        if (this.gameObject.GetComponent<Toggle>().isOn)
        {
            textLabel.GetComponent<Text>().color = ActiveTextColor;
        }
        else
        {
            textLabel.GetComponent<Text>().color = DefaultTextColor;
        }
    }

    public void Init(string text, int id, System.Action toggleAction)
    {
        this.id = id;
        this.toggleAction = toggleAction;
        writeLabel(text);
    }

    void writeLabel(string text)
    {
        textLabel.GetComponent<Text>().text = text;
    }

    public void switchCheckmark()
    {
        toggleAction?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
