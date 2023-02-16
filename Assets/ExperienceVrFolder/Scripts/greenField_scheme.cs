using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greenField_scheme : MonoBehaviour, IGreenField {

    public int chooseNum { get; set; } = -1;
    public bool isCursor { get; set; } = false;
    public GameObject commonData;
    public int[] pointToScore;
    public Material empty;
    public int side;

    private float kf = 1.57f;
    
    [HideInInspector]
    public GameObject activeTable;
    [HideInInspector]
    public bool isUse = false;
    bool isClick;

    public void SetDefault()
    {
        Color col = this.gameObject.GetComponent<MeshRenderer>().materials[1].color;
        this.gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(col.r, col.g, col.b, 0);
        isCursor = false;
    }

    public void SetCursor()
    {
        Color col = this.gameObject.GetComponent<MeshRenderer>().materials[1].color;
        this.gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(col.r, col.g, col.b, 0.4583f);
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

        Debug.Log("prepare");

        //GameObject gObject = GameObject.Find("System").GetComponent<commonData>().selectObj ?? null;
        List<GameObject> movableItems = new List<GameObject>();
        var movableItemsContainer = GameObject.Find("MovableItems");
        foreach (Transform item in movableItemsContainer.transform)
            movableItems.Add(item.gameObject);
        var gObject = movableItems[idx] ?? null;
        Debug.Log(gObject.name);

        if (gObject == null)
            return;

        Debug.Log("set");

        switch (side)
        {
            case 0:
                this.GetComponent<MeshRenderer>().material = gObject.GetComponent<chooseItems_schema>()?.schemeMat[0];
                break;
            case 2:
                this.GetComponent<MeshRenderer>().material = gObject.GetComponent<chooseItems_schema>()?.schemeMat[1];
                break;
            case 1:
                this.GetComponent<MeshRenderer>().material = gObject.GetComponent<chooseItems_schema>()?.schemeMat[2];
                break;
            case (-1):
                this.GetComponent<MeshRenderer>().material = gObject.GetComponent<chooseItems_schema>()?.schemeMat[3];
                break;
        }
        rotate();

        chooseNum = idx;
    }

    public void DestroyObject()
    {
        chooseNum = -1;
        this.gameObject.GetComponent<MeshRenderer>().material = empty;
    }

    void rotate()
    {
        this.GetComponent<MeshRenderer>().material.SetFloat("_Angle", side * kf);
    }

    void Start()
    {
        chooseNum = -1;
    }
}
