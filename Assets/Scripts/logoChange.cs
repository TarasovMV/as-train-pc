using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logoChange : MonoBehaviour {

    public Texture2D logoOn;
    public Texture2D logoOff;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        this.gameObject.GetComponent<RawImage>().texture = logoOff;
    }

    private void OnMouseExit()
    {
        this.gameObject.GetComponent<RawImage>().texture = logoOn;
    }
}
