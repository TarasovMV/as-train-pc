using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberContoller : MonoBehaviour {

	public GameObject NumberPrefab;
	public GameObject NumberContainer;

	List<Number> numbers = new List<Number>();
	List<TestingQuestion> questions = new List<TestingQuestion>();

	System.Action<int> toggleQuestion;

    private void FixedUpdate()
    {
		SetFillNumbers();
	}

    public void Init (List<TestingQuestion> questions, System.Action<int> action)
	{
		toggleQuestion = action;
		this.questions = questions;
		for (int i = 0; i < questions.Count; i++)
        {
			var numberGo = Instantiate(NumberPrefab);
			numberGo.transform.SetParent(NumberContainer.transform);
			numberGo.GetComponent<nomerCs>().Init(i, (idx) => activeNumber(idx));
			numbers.Add(new Number(i, numberGo));
		}
		activeNumber(0);
	}

	public void activeNumber(int index, bool isNext = false)
    {
		if (!numbers[index].isAvailable && !isNext)
		{
            return;
        }
        toggleQuestion(index);
        foreach (var number in numbers)
		{
			number.numberGo.GetComponent<nomerCs>().Unactivate();
		}
		numbers[index].numberGo.GetComponent<nomerCs>().Activate();
		numbers[index].isAvailable = true;
	}

	private void SetFillNumbers()
    {
		for(int i = 0; i < questions.Count; i++)
        {
			var question = questions[i];
			var number = numbers[i].numberGo.GetComponent<nomerCs>();
			if (string.IsNullOrEmpty(question.result.freeResult) && !(question.result.chooseResult?.Count > 0))
            {
				number.Unfill();
            }
			else
            {
				number.Fill();
			}
        }
    }
}

class Number
{
	public GameObject numberGo;
	public bool isActive;
	public bool isAvailable;

	public Number(int index, GameObject obj)
    {
		numberGo = obj;
		isActive = false;
		if (index == 0)
        {
			isAvailable = true;
			return;
		}
		isAvailable = false;
	}
}