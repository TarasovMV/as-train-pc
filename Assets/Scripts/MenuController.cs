using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public GameObject ContainerGo;
	public GameObject ContainerOverlayGo;

	public GameObject HelloGo;
	public GameObject LoginGo;
	public GameObject TestingGo;
	public GameObject TestingVrGo;
    public GameObject InstructionGo;
	public GameObject PreviewGo;
	public GameObject ConclusionGo;
	public GameObject LoaderGo;
	public GameObject HeaderGo;

	private GameObject currentLoader = null;

	private MainController mainController;
	private GetDataService getDataService;
	private SocketContoller socketController;

	void Start ()
	{
		mainController = GameObject.Find("Scripts").GetComponent<MainController>();
		getDataService = GameObject.Find("Scripts").GetComponent<GetDataService>();
		socketController = GameObject.Find("SocketHub").GetComponent<SocketContoller>();
	}

	void loadCategories()
    {
		initLoader();
		getDataService.GetCategories(
			(categories) => {
				mainController.SetCategories(categories);
				initLogin();
				destroyLoader();
			},
			() => {
				connectionError(() => loadCategories());
				destroyLoader();
			}
		);
	}

	void loadTestingList(int id)
    {
		initLoader();
		getDataService.GetTestingList(id,(testings) => {
				mainController.SetTestings(testings);
				initPreview();
				destroyLoader();
			},
			() => {
				connectionError(() => loadTestingList(id));
				destroyLoader();
			}
		);
	}

	void saveResult()
    {
        try
        {
            getDataService.SaveResult(mainController.GetResult());
        }
		catch (Exception e)
        {
            Debug.LogError(e);
        } 
		initConclusion();
	} 

	public void Init()
    {
        initHello();
    }

	void initLogin()
    {
		clearContainer();
		var go = Instantiate(LoginGo);
		uiPosition(go, true);
		var categories = getUserCategories();
		go.GetComponent<LoginController>().Init(categories, (user) => {
			clearContainer();
			mainController.SetUser(user);
			HeaderGo.GetComponent<HeaderComponent>().SetUserInfo(user);
			loadTestingList(mainController.GetUser().category.testingSetId);
		});
    }

	void initStage(int idx)
    {
		if (idx >= getTestingCount())
        {
			saveResult();
			return;
        }
		var testing = getTestingByIdx(idx);
		HeaderGo.GetComponent<HeaderComponent>().SetStageInfo(idx, testing.title);
		var instruction = instInstruction(idx, testing);
    }

	GameObject initHello()
    {
		clearContainer();
		var go = Instantiate(HelloGo);
		go.GetComponent<startScreenCs>().Init(() => loadCategories());
		uiPosition(go, true);
		return go;
    }

	GameObject initConclusion()
    {
		clearContainer();
		var go = Instantiate(ConclusionGo);
		ConclusionGo.GetComponent<AlertController>().Init(() => goToMainMenu());
		uiPosition(go, true);
		return go;
	}

	GameObject initPreview()
	{
		clearContainer();
		var category = mainController.GetUserCategoryById();
		var go = Instantiate(PreviewGo);
		uiPosition(go, true);
		go.GetComponent<PreviewController>().Init(category, mainController.GetTestingList(), () => {
			clearContainer();
			initStage(0);
		});
		return go;
    }

	GameObject instInstruction(int idx, Testing testing)
    {
		clearContainer();
		var go = Instantiate(InstructionGo);
		uiPosition(go);
		go.GetComponent<InsructionController>().Init(testing.questions.Count, testing.time, testing.type, () =>
		{
			instTesting(idx, testing);
		});
		return go;
	}

	GameObject instTesting(int idx, Testing testing)
	{
		clearContainer();
		int nextIndex = ++idx;
		Debug.Log($"Next stage {nextIndex}");
        GameObject go;
        if (testing.type == 3)
        {
            go = Instantiate(TestingVrGo);
            go.name = "VrExperienceComponent";
            go.GetComponent<VrExperienceComponent>().Init(testing, () => initStage(nextIndex));
        } else
        {
            go = Instantiate(TestingGo);
            go.GetComponent<TestingController>().Init(testing, () => initStage(nextIndex));
        }
        uiPosition(go);
        return go;
    }

	void initLoader()
    {
		if (currentLoader != null)
        {
			return;
        }
		currentLoader = Instantiate(LoaderGo);
		uiPosition(currentLoader);
    }

	void destroyLoader()
    {
		if (currentLoader == null)
        {
			return;
        }
		Destroy(currentLoader);
		currentLoader = null;
    }

	void goToMainMenu()
    {
		socketController.ClearLastValues();
		Debug.Log("main menu loading...");
		SceneManager.LoadScene("MainMenu");
	}

	void connectionError(System.Action retryAction)
    {
		mainController.ConnectionError(
			() => goToMainMenu(),
			retryAction
		);
    }

	void uiPosition(GameObject go, bool isOverlay = false)
    {
		if (isOverlay)
        {
			go.GetComponent<RectTransform>().SetParent(ContainerOverlayGo.transform);
		}
		else
        {
			go.GetComponent<RectTransform>().SetParent(ContainerGo.transform);
		}
        go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }

	int getTestingCount()
    {
		return mainController.GetTestingList().Count;
	}

	Testing getTestingByIdx(int idx)
    {
		if (idx >= getTestingCount())
        {
			return null;
        }
		return mainController.GetTestingList()[idx];
    }

	List<UserCategory> getUserCategories()
	{
		return mainController.GetUserCategories();
	}

	void clearContainer()
    {
		foreach(Transform child in ContainerGo.transform)
        {
			Destroy(child.gameObject);
        }

		foreach (Transform child in ContainerOverlayGo.transform)
		{
			Destroy(child.gameObject);
		}
	}
}
