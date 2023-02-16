using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainObject : MonoBehaviour {

    public void OnEnter()
    {
        Debug.Log("OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
    }

    public void OnDrag()
    {

    }

    public void OnButtonClick()
    {
        Debug.Log("OnClick");
    }




    void Start()
    {

    }
}
