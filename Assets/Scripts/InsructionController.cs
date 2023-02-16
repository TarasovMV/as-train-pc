using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsructionController : MonoBehaviour {

	public GameObject DefaultInstruction;
	public GameObject PanoInstruction;
	public GameObject VrInstruction;

    public List<Text> Timers;
	public List<Text> Questions;

	private MainController mainController;

	private System.Action action;

    private void Start()
    {
		mainController = GameObject.Find("Scripts").GetComponent<MainController>();
    }

    public void Init(int questionCount, int time, int type, System.Action action)
    {
		this.action = action;

		Timers.ForEach(timer => timer.text = getTextTime(time));
		Questions.ForEach(question => question.text = getTextQuestion(questionCount));

		switch (type)
        {
			case 1:
				DefaultInstruction.SetActive(true);
				break;
			case 2:
				PanoInstruction.SetActive(true);
				break;
            case 3:
                VrInstruction.SetActive(true);
                break;
		}
	}

	public void ButtonNext()
    {
		mainController.TestingStart(action);
    }

	string getTextTime(int timeInSecs)
    {
		string minText = "минут";
		string secText = "секунд";

		int minutes = timeInSecs / 60;
		int seconds = timeInSecs % 60;

		minText += charAdderTime(minutes);
		secText += charAdderTime(seconds);

		if (seconds == 0)
        {
			return minutes.ToString() + " " + minText;
		}
		else
        {
			return minutes.ToString() + " " + minText + (seconds > 9 ? " " : " 0") + seconds.ToString() + " " + secText;
		}
	}

	string getTextQuestion(int count)
    {
		return $"{count} вопрос{charAdderQuestion(count)}";
    }

	string charAdderTime(int num)
	{
		if (num % 10 == 1)
		{
			return "а";
		}
		else if (num % 10 > 1 && num % 10 < 5)
		{
			return "ы";
		}
		return "";
	}

	string charAdderQuestion(int num)
	{
		if (num % 10 >= 2 && num % 10 <= 4)
		{
			return "а";
		}
		else if (num % 10 == 0 || (num % 10 >= 5 && num % 10 <= 9))
		{
			return "ов";
		}
		return "";
	}
}
