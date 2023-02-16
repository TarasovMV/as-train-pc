using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greenField_nasos : MonoBehaviour, IGreenField {

    public int chooseNum { get; set; } = -1;
    public bool isCursor { get; set; } = false;
    public GameObject commonData;
    public int[] pointToScore;

    bool isClick;
    GameObject newSelectObj;
    GameObject newText;

    [Header("Стандартная позиция")]
    public float scaleSize = 1;
    public Vector3 pos;
    public Vector3 ugol;


    [HideInInspector]
    public GameObject activeTable;

    public void SetDefault()
    {
        Color col = this.gameObject.GetComponent<MeshRenderer>().material.color;
        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(col.r, col.g, col.b, 0);
        isCursor = false;
    }

    public void SetCursor()
    {
        Color col = this.gameObject.GetComponent<MeshRenderer>().material.color;
        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(col.r, col.g, col.b, 0.4583f);
        isCursor = true;
    }

    public void SetObject(int idx)
    {
        if (idx == chooseNum)
        {
            return;
        }

        if (idx == -1)
        {
            DestroyObject();
            return;
        }

        DestroyObject();

        List<chooseItems> movableItems = new List<chooseItems>();
        var movableItemsContainer = GameObject.Find("MovableItems");
        foreach (Transform item in movableItemsContainer.transform)
            movableItems.Add(item.gameObject.GetComponent<chooseItems>());
        var gObject = movableItems[idx]?.inObjUse ?? null;

        if (gObject == null)
            return;

        newSelectObj = Instantiate(gObject) as GameObject;
        newSelectObj.SetActive(true);
        newSelectObj.transform.parent = this.gameObject.transform.parent;
        newSelectObj.transform.localRotation = Quaternion.Euler(ugol);
        newSelectObj.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
        newSelectObj.transform.localPosition = pos;

        chooseNum = idx;
    }

    public void DestroyObject()
    {
        chooseNum = -1;
        if (newSelectObj)
            Destroy(newSelectObj);
    }

    void Start()
    {
        chooseNum = -1;
    }
}
