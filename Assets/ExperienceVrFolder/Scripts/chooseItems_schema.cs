using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chooseItems_schema : MonoBehaviour, IChooseItem {

    public int number;
    public string name;
    public GameObject plashka;
    public GameObject commonData;
    public int type = 0;
    public Material[] schemeMat;

    public Color colorActive;
    public Color colorUnactive;
    public Color colorUse;

    public bool isUse { get; set; } = false;
    public bool isChoose { get; set; } = false;
    public bool isCursor { get; set; } = false;

    public void SetUse()
    {
        if (isUse)
            return;
        this.GetComponent<SpriteRenderer>().color = colorUse;
        isUse = true;
    }

    public void SetCursor()
    {
        if (isCursor)
            return;
        plashka.SetActive(true);
        this.gameObject.GetComponentInChildren<Text>().text = name;
        isCursor = true;
    }

    public void SetChoose()
    {
        if (isChoose)
            return;
        this.GetComponent<SpriteRenderer>().color = colorActive;
        isChoose = true;
    }

    public void SetDefault()
    {
        if (!isCursor && !isChoose && !isUse)
            return;
        this.GetComponent<SpriteRenderer>().color = colorUnactive;
        plashka.SetActive(false);
        isCursor = false;
        isChoose = false;
        isUse = false;
    }
}
