using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnackbarController : MonoBehaviour
{
	public enum SnackbarType
    {
		Info,
		Error
    }

	[Header("Settings")]
	public int lifeTimeSeconds = 4;
    public Color InfoColor;
    public Color ErrorColor;

    [Header("Components")]
	public Text TextSnack;
    public Image Container;

    public void Init(string msg, SnackbarType type = SnackbarType.Info)
    {
        Debug.Log(msg);
        switch(type)
        {
            case SnackbarType.Info:
                Container.color = InfoColor;
                break;
            case SnackbarType.Error:
                Container.color = ErrorColor;
                break;
        }

        TextSnack.text = msg;
        StartCoroutine(lifecycleCoroutine());
    }

	private IEnumerator lifecycleCoroutine()
    {
        while (this.GetComponent<CanvasGroup>().alpha < 1)
        {
            this.GetComponent<CanvasGroup>().alpha += 0.15f;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(lifeTimeSeconds);
		while (this.GetComponent<CanvasGroup>().alpha != 0)
        {
			this.GetComponent<CanvasGroup>().alpha -= 0.15f;
			yield return new WaitForSeconds(0.05f);
        }
        Destroy(this.gameObject);
    }
}
