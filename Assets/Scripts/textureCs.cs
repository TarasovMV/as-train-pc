using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureCs : MonoBehaviour {

    public GameObject sphereCs;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseEnter()
    {
        Debug.Log("enter");
        sphereCs.GetComponent<pano360Cs>().allowScroll = true;

    }
    public void OnMouseExit()
    {
        Debug.Log("exit");
        sphereCs.GetComponent<pano360Cs>().allowScroll = false;
        sphereCs.GetComponent<pano360Cs>().resetPano();
    }
}
