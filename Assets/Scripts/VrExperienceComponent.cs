using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VrExperienceComponent : MonoBehaviour {

    Testing testing;

    private GameObject MainScripts;
    private System.Action nextAction;

    public InputField CodeInput;

    void Awake()
    {
        MainScripts = GameObject.Find("Scripts");
    }

    private void OnDestroy()
    {
        MainScripts.GetComponent<GetDataService>().CloseVr();
        StopCoroutine(timeCounterCoroutine());
    }

    public void Init(Testing test, System.Action action)
    {
        testing = test;
        nextAction = action;
        StartCoroutine(timeCounterCoroutine());
        connectHelmet(test);
    }

    public void SetCode(string code)
    {
        CodeInput.text = code;
    }

    public void OpenVrScene()
    {
        GameObject.Find("VrScenes").GetComponent<StateController>().Activate();
    }

    public void ApplyCode()
    {
        // code = concat(9 - (true answer)) * 5
        var code = CodeInput.text;
        var originalCode = code;
        if (code?.Length == 0 || code?.Length < testing.questions.Count)
        {
            return;
        }
        
        code = (int.Parse(code) / 5).ToString();
        if (code.Length < testing.questions.Count)
        {
            return;
        }
        for (int i = 0; i < testing.questions.Count; i++)
        {
            testing.questions[i].result = new TestingQuestionResult()
            {
                vrResult = 9 - int.Parse(code[i].ToString()),
            };
        }
        MainScripts.GetComponent<MainController>().TestingCheckVr(testing, nextAction, originalCode);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OpenVrScene();
        }
    }

    private void connectHelmet(Testing test)
    {
        var hemlet = getHelmet(test);
        Debug.Log(JsonUtility.ToJson(hemlet));
        MainScripts.GetComponent<GetDataService>().SetActiveVr(hemlet);
    }

    private TestingVrHelmet getHelmet(Testing test) {
        return new TestingVrHelmet()
        {
            questions = test.questions.Select(x => new TestingVrHelmetQuestion() { id = x.vrExperience, title = x.title }).ToList(),
            restTime = test.time - test.resultTime,
            allTime = test.time,
        };
    }

    IEnumerator timeCounterCoroutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            testing.resultTime++;
        }
    }
}
