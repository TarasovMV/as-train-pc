using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingController : MonoBehaviour {

    Testing testing;

    private TestingQuestionModel currentQuestion;
    private GameObject MainScripts;
    private System.Action nextAction;

    public GameObject PanoImage;
    public GameObject QuestionPrefab;
    public GameObject NumbersPanel;
    public GameObject TimerGo;
    public GameObject NextButton;
    public GameObject SendButton;

    void Start ()
    {
        MainScripts = GameObject.Find("Scripts");
	}

    private void OnDestroy()
    {
        StopCoroutine(timeCounterCoroutine());
    }

    public void Init(Testing test, System.Action action)
    {
        SendButton.SetActive(false);
        testing = test;
        nextAction = action;
        testingTypeCheck(testing.type);
        StartCoroutine(timeCounterCoroutine());
        TimerGo.GetComponent<TimerContoller>().Init(test.time, () => timeout());
    }

    void InitNumbers()
    {
        NumbersPanel.GetComponent<NumberContoller>().Init(testing.questions, (idx) => activateQuestion(idx));
    }

    void activateQuestion(int index)
    {
        getCurrentResult();
        if (currentQuestion != null)
        {
            Destroy(currentQuestion.gameObject);
        }
        buttonsCheck(index);
        var questionGo = Instantiate(QuestionPrefab);
        questionGo.transform.SetParent(transform);
        questionGo.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        questionGo.GetComponent<QuestionController>().Init(testing.questions[index], index + 1, testing.type == 2);
        currentQuestion = new TestingQuestionModel(index, questionGo);
    }

    public void NextQuestion()
    {
        int index = ++currentQuestion.index;
        NumbersPanel.GetComponent<NumberContoller>().activeNumber(index, true);
    }

    public void SendResult()
    {
        getCurrentResult();
        MainScripts.GetComponent<MainController>().TestingCheck(testing, nextAction);
    }

    void getCurrentResult()
    {
        if (!(currentQuestion != null && currentQuestion.gameObject != null))
        {
            return;
        }
        currentQuestion.gameObject.GetComponent<QuestionController>().getResult();
    }

    void buttonsCheck(int index)
    {
        NextButton.SetActive(true);
        if (index >= testing.questions.Count - 1)
        {
            NextButton.SetActive(false);
            SendButton.SetActive(true);
        }
    }

    void timeout()
    {
        getCurrentResult();
        MainScripts.GetComponent<MainController>().TestingTimeout(testing, nextAction);
    }

    void testingTypeCheck(int type)
    {
        switch (type)
        {
            case 1:
                InitNumbers();
                break;
            case 2:
                //PanoImage.SetActive(true);
                InitNumbers();
                break;
            case 3:
                // vr
                break;
        }
    }

    IEnumerator timeCounterCoroutine()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(1);
            testing.resultTime++;
        }
    }
}

public class TestingQuestionModel
{
    public int index;
    public GameObject gameObject;

    public TestingQuestionModel(int index, GameObject gameObject)
    {
        this.index = index;
        this.gameObject = gameObject;
    }
}
