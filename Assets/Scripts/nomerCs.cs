using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nomerCs : MonoBehaviour {

    public GameObject numText;
    public GameObject SelectLine;

    private System.Action clickAction;

    public Color FillColorBack;
    public Color DefaultColorBack;

    public Color FillColorText;
    public Color DefaultColorText;

    public void Init(int index, System.Action<int> action)
    {
        numText.GetComponent<Text>().text = (index + 1).ToString();
        clickAction = () => action(index);
    }

    public void Activate()
    {
        SelectLine.SetActive(true);
    }

    public void Unactivate()
    {
        SelectLine.SetActive(false);
    }

    public void Fill()
    {
        this.gameObject.GetComponent<Image>().color = FillColorBack;
        numText.GetComponent<Text>().color = FillColorText;
    }

    public void Unfill()
    {
        this.gameObject.GetComponent<Image>().color = DefaultColorBack;
        numText.GetComponent<Text>().color = DefaultColorText;
    }

    public void OnClick()
    {
        clickAction();
    }
}
