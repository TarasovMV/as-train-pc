using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanoInfoComponent : MonoBehaviour {

    private GameObject Scripts;
    private int number;

	void Start () {
        Scripts = GameObject.Find("Pano360");
        int.TryParse(gameObject.name, out number);
    }

    void OnMouseEnter()
    {
        //Scripts.GetComponent<pano360Cs>().SetUiInfo($"РАБОТНИК {number}");
    }

    void OnMouseExit()
    {
        //Scripts.GetComponent<pano360Cs>().DisableUiInfo();
    }
}
