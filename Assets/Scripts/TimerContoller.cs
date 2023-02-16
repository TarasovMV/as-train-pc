using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerContoller : MonoBehaviour {

	// config
	private readonly int alarmPercent = 25;
	private readonly Color alarmColor = new Color(0.9044118f, 0.2859537f, 0.2859537f);

	// components
	public GameObject timerText;
	public GameObject timerLoader;
	public GameObject timerGo;

	// values
	private int restTimeInSecs = 0;
	private int allTimeInSecs = 0;
	private int alarmTimeInSecs = 0;

	private System.Action timeoutAction;

	public void Init(int time, System.Action timeoutAction)
    {
		if (!(time > 0)) return;
		restTimeInSecs = allTimeInSecs = time;
		alarmTimeInSecs = restTimeInSecs * alarmPercent / 100;
		this.timeoutAction = timeoutAction;
		StartCoroutine(TimerCoroutine());
    }

    public void SetTime(int allTime, int restTime)
    {
        allTimeInSecs = allTime;
        displayTimerText(restTime);
        displayTimerLoader(restTime);
    }

	IEnumerator TimerCoroutine()
    {
		while(restTimeInSecs > 0)
        {
			yield return new WaitForSecondsRealtime(1);
			restTimeInSecs--;

			displayTimerText(restTimeInSecs);
			displayTimerLoader(restTimeInSecs);
			displayAlarm(restTimeInSecs, alarmTimeInSecs);
		}
		timeoutAction?.Invoke();
    }

	void displayTimerText(int time)
    {
		int minute = time / 60;
		int second = time - 60 * minute;
		string timerStr = minute.ToString() + (second < 10 ? ":0" : ":") + second.ToString();
		timerText.GetComponent<Text>().text = timerStr;
    }

	void displayTimerLoader(int time)
    {
		float loaderValue = allTimeInSecs <= 0 ? 1 : ((float)time / (float)allTimeInSecs);
		timerLoader.GetComponent<Image>().fillAmount = loaderValue;
    }

	void displayAlarm(int time, int alarmTime)
    {
		if (time > alarmTime)
        {
			return;
		}
		timerGo.GetComponent<Animation>().Play("timerAlarm");
		timerLoader.GetComponent<Image>().color = alarmColor;
	}
}
