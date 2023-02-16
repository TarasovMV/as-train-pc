using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlertController : MonoBehaviour {

	public Text textGo;
	public bool IsAuto;
	public int TimerSec;
	private System.Action acceptFunction;
	private System.Action cancelFunction;

    private bool isTimer = false;

    void Start()
    {
        if (IsAuto)
        {
            StartCoroutine(AutoLifecycleCoroutine());
        }
    }

	public void Init(System.Action accept, System.Action cancel = null, string questionText = null)
    {
		acceptFunction = accept;
		cancelFunction = cancel;
		setQuestion(questionText);
    }

	private void setQuestion(string text)
    {
		if (textGo == null || string.IsNullOrEmpty(text))
        {
			return;
        }
		textGo.text = text;
    }
	
	public void ButtonAccept()
    {
        Debug.Log("btn accept");
        acceptFunction?.Invoke();
        Destroy(this.gameObject);
    }

	public void ButtonCancel()
    {
		Debug.Log("btn cancel");
        cancelFunction?.Invoke();
        Destroy(this.gameObject);
	}

	IEnumerator AutoLifecycleCoroutine()
    {
		yield return new WaitForSecondsRealtime(TimerSec);
        SceneManager.LoadScene("MainMenu");
    }
}
