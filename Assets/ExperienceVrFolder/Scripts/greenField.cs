using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greenField : MonoBehaviour, IGreenField {

    public int chooseNum { get; set; } = -1;
    public bool isCursor { get; set; } = false;
    public GameObject commonData;
    GameObject newSelectObj;
    GameObject newText;
    bool isClick;
    public GameObject activeTable;
    public int[] pointToScore;

    [Header("Стандартная позиция")]
    public float scaleSize = 1;
    public Vector3 pos;
    public Vector3 ugol;
    public Vector3 scaleVector = Vector3.zero;

    public void SetDefault()
    {
        isCursor = false;
        Color col = this.gameObject.GetComponent<MeshRenderer>().material.color;
        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(col.r, col.g, col.b, 0);
    }

    public void SetCursor()
    {
        isCursor = true;
        Color col = this.gameObject.GetComponent<MeshRenderer>().material.color;
        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(col.r, col.g, col.b, 0.4583f);
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
        newSelectObj.transform.parent = this.gameObject.transform;
        newSelectObj.transform.localRotation = Quaternion.Euler(ugol);
        newSelectObj.transform.localScale = scaleVector != Vector3.zero ? scaleVector : new Vector3(scaleSize, scaleSize, scaleSize);
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
