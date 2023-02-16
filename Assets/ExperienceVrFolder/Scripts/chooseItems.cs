using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chooseItems : MonoBehaviour, IChooseItem {

    public int number;
    public string name;
    public GameObject plashka;
    public GameObject commonData;
    public GameObject inObj;
    public GameObject inObjUse;

    public Color colorActive;
    public Color colorUnactive;
    public Color colorUse;

    public int type = 0;

    public bool isUse { get; set; } = false;
    public bool isChoose{ get; set; } = false;
    public bool isCursor { get; set; } = false;

    public void SetUse()
    {
        Debug.Log($"use: {name}");
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
        Debug.Log($"choose: {name}");
        if (isChoose)
            return;
        this.GetComponent<SpriteRenderer>().color = colorActive;
        isChoose = true;
    }

    public void SetDefault()
    {
        this.GetComponent<SpriteRenderer>().color = colorUnactive;
        plashka.SetActive(false);
        isCursor = false;
        isChoose = false;
        isUse = false;
    }

    void Update()
    {
        if (isCursor)
            onDrag();
    }

    void onDrag()
    {
        Vector3 startUgol = inObj.GetComponent<Transform>().rotation.eulerAngles;
        inObj.GetComponent<Transform>().rotation = Quaternion.Euler(startUgol.x, startUgol.y + 1, startUgol.z);
    }
}
