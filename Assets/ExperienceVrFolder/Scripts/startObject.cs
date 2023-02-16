using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startObject : MonoBehaviour {

    public GameObject tablePref;
    public GameObject table;
    public GameObject main = null;
    public Text text;
    public string nameObj = "name";

    public void OnEnter()
    {
        Debug.Log("OnEnter");
        table.SetActive(true);
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        table.SetActive(false);
        
    }

    public void OnDrag()
    {
        table.GetComponent<Transform>().LookAt(main.transform);
    }

    public void OnButtonClick()
    {
        Debug.Log("OnClick");
    }


    void Awake()
    {
        main = GameObject.Find("CenterEyeAnchor");
        table = Instantiate(tablePref);
        table.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
        table.transform.parent = this.gameObject.transform;
        table.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);    
        
        text = table.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = nameObj;
        }
        table.SetActive(false);
    }
}
