using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour {

	public GameObject AnswerPrefab;

    public GameObject ListAnswersPanel;
    public GameObject ListAnswersContainer;
    public GameObject FreeAnswerPanel;
	public Text QuestionText;
	public Text QuestionNumber;

    private readonly string questionOffset = "                       ";
    private bool isPano = false;
    private GameObject PanoGo;
    private List<GameObject> AnswersGo = new List<GameObject>();

    TestingQuestion questionData = null;

    private void Start()
    {
        PanoGo = GameObject.Find("Pano360");
    }

    public void Init(TestingQuestion data, int questionNumber, bool isPano)
    {
        questionData = data;
        QuestionNumber.text += questionNumber.ToString();
        QuestionText.text = questionData.title;
        activePanelByType(questionData.type);
        fillAnswer();
        fillHistory();
        this.isPano = isPano;
        if (isPano)
        {
            activateSocketPano(data.pano - 1);
        }
    }

    private void activateSocketPano(int panoIdx)
    {
        var Scripts = GameObject.Find("Scripts");
        Scripts.GetComponent<GetDataService>().SetActivePano(panoIdx);
    }

    private void activePanelByType(int type)
    {
        if (type == 1 || type == 2)
        {
            ListAnswersPanel.SetActive(true);
        }
        else
        {
            FreeAnswerPanel.SetActive(true);
        }
    }

    private void fillAnswer()
    {
        if(!ListAnswersPanel.activeSelf)
        {
            return;
        }
        int idx = 0;
        foreach (var answer in questionData.answers)
        {
            int index = idx;
            var answerGo = Instantiate(AnswerPrefab);
            answerGo.transform.SetParent(ListAnswersContainer.transform);
            answerGo.GetComponent<toggleMain>().Init(answer.title, answer.id, () => toggleAnswer(index));
            AnswersGo.Add(answerGo);
            idx++;
        }
    }

    private void fillHistory()
    {
        if(questionData.type == 3)
        {
            FreeAnswerPanel.GetComponent<InputField>().text = questionData.result?.freeResult ?? "";
        }
        else
        {
            AnswersGo.ForEach((toggle) =>
            {
                if ((questionData.result?.chooseResult?.Exists(el => el == toggle.GetComponent<toggleMain>().id) ?? false))
                {
                    toggle.GetComponent<Toggle>().isOn = true;
                }
            });
        }
    }

    private void toggleAnswer(int index)
    {
        if (questionData.type != 1 || AnswersGo[index].GetComponent<Toggle>().isOn == false)
        {
            return;
        }
        AnswersGo.ForEach(el =>
        {
            if (el != AnswersGo[index])
            {
                el.GetComponent<Toggle>().isOn = false;
            }
        });
    }

    public void panoClick()
    {
        if (!isPano || PanoGo == null)
        {
            return;
        }
        activateSocketPano(questionData.pano - 1);
        PanoGo.GetComponent<pano360Cs>().Init(questionData.pano - 1);
    }

    public TestingQuestionResult getResult()
    {
        var result = new TestingQuestionResult();
        if (questionData.type == 3)
        {
            result.freeResult = getResultFree();
        }
        else
        {
            result.chooseResult = getResultChoose();
        }
        questionData.result = result;
        return result;
    }

    private string getResultFree()
    {
        return string.IsNullOrEmpty(FreeAnswerPanel.GetComponent<InputField>().text) ? null : FreeAnswerPanel.GetComponent<InputField>().text;
    }

    private List<int> getResultChoose()
    {
        var result = new List<int>();
        foreach (var answerGo in AnswersGo)
        {
            if(answerGo.GetComponent<Toggle>().isOn)
            {
                result.Add(answerGo.GetComponent<toggleMain>().id);
            }
        }
        return result.Count == 0 ? null : result;
    }
}