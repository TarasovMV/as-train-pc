using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main360Cs : MonoBehaviour {

    public GameObject header;
    public GameObject panoObject;
    public GameObject mainCamera;
    public GameObject canvas;


	// Use this for initialization
	void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panoEnd();
        }
    }

    public void panoStart()
    {
        header.SetActive(false);
        panoObject.SetActive(true);
        //mainCamera.SetActive(false);
        //canvas.SetActive(false);
    }

    public void panoEnd()
    {
        header.SetActive(true);
        panoObject.SetActive(false);
        //mainCamera.SetActive(true);
        //canvas.SetActive(true);
    }

}
