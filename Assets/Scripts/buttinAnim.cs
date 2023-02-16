using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttinAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponent<Animation>().Play("buttonBig");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<Animation>().Play("buttonSmall");
    }
}
