using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startScreenCs : MonoBehaviour {

    private int percentageCounter = 0;

    public Text ProgressText;

    public System.Action nextAction;

    private bool isCheckConnection = false;

    public void Init(System.Action action)
    {
        var getDataService = GameObject.Find("Scripts").GetComponent<GetDataService>();
        nextAction = action;
        StartCoroutine(Lifecycle());
        getDataService.CheckConnection(
            null,
            () =>
            {
                getDataService.IsOnline = true;
                isCheckConnection = true;
            },
            () =>
            {
                getDataService.IsOnline = false;
                isCheckConnection = true;
            }
        );
    }

    IEnumerator Lifecycle()
    {   
        while(percentageCounter < 100)
        {
            percentageCounter += 1;
            ProgressText.text = $"{percentageCounter}%";
            yield return new WaitForSecondsRealtime(.05f);
        }

        while(!isCheckConnection)
        {
            yield return new WaitForSeconds(1);
        }

        nextAction?.Invoke();
        Destroy(this.gameObject);
    }
}
