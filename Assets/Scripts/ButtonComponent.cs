using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonComponent : MonoBehaviour {

	public Text LabelComponent;
	public string Label;

	void Start () {
		LabelComponent.text = Label.ToUpper();
	}
}
