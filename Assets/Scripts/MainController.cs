using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainController : MonoBehaviour {

	public GameObject Canvas;

	[Header("Testing")]
	public GameObject TestingPrefab;

	[Header("Alerts")]
	public GameObject AlertContainer;
	public GameObject AlertTimeGo;
	public GameObject AlertStartGo;
	public GameObject AlertCheckGo;
	public GameObject AlertCheckVrGo;
    public GameObject AlertErrorGo;
    public GameObject PopupChooserGo;

    private User user;
    private List<Testing> testings;
    private List<UserCategory> userCategories;

	void Start () {
        Canvas.GetComponent<MenuController>().Init();
    }

    public CompetitionResult GetResult()
    {
        var uid = System.Guid.NewGuid();
        return new CompetitionResult(uid, user, testings);
    }

    public void SetUser(User user)
    {
        this.user = user;
    }

    public User GetUser()
    {
        return user;
    }

    public UserCategory GetUserCategoryById()
    {
        var cat = userCategories.FirstOrDefault(x => x.id == user.userCategoryId);
        Debug.Log(cat.file.path);
        return userCategories.FirstOrDefault(x => x.id == user.userCategoryId);
    }

    public void SetCategories(List<UserCategory> categories)
    {
        userCategories = categories;
    }

    public void SetTestings(List<Testing> testings)
    {
        this.testings = testings;
    }

    public List<Testing> GetTestingList()
    {
        return testings;
    }

    public List<UserCategory> GetUserCategories()
    {
        return userCategories;
    }

    public void ClearAlerts()
    {
        foreach(Transform alert in AlertContainer.transform)
        {
            Destroy(alert.gameObject);
        }
    }

    public void TestingStart(System.Action nextAction)
    {
        var alert = Instantiate(AlertStartGo);
        alertPoser(alert);
        alert.GetComponent<AlertController>().Init(() => nextAction());
    } 

	public void TestingCheck(Testing testing, System.Action nextAction, bool isAllAnswer = false)
    {
		var alert = Instantiate(AlertCheckGo);
        alertPoser(alert);
        alert.GetComponent<AlertController>().Init(() => nextAction());
    }

    public void TestingCheckVr(Testing testing, System.Action nextAction, string code)
    {
        var alert = Instantiate(AlertCheckVrGo);
        alertPoser(alert);
        alert.GetComponent<AlertController>().Init(() => nextAction(), null, $"Проверьте правильность введенного кода: {code}.");
    }

    public void TestingTimeout(Testing testing, System.Action nextAction)
	{
        ClearAlerts();
        var alert = Instantiate(AlertTimeGo);
        alertPoser(alert);
        alert.GetComponent<AlertController>().Init(() => nextAction());
	}

    public void PopupChoose(List<string> vars, string activeVar, string title, System.Action<string> nextAction)
    {
        var popup = Instantiate(PopupChooserGo);
        alertPoser(popup);
        popup.GetComponent<PopupChooseComponent>().Init(vars, activeVar, title, nextAction);
    }

    public void ConnectionError(System.Action nextAction, System.Action cancelAction)
    {
        var alert = Instantiate(AlertErrorGo);
        alertPoser(alert);
        alert.GetComponent<AlertController>().Init(nextAction, cancelAction);
    }

    private void alertPoser(GameObject alert)
    {
        alert.transform.SetParent(AlertContainer.transform);
        alert.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
    }
}

public class RenderingExtension : MonoBehaviour
{
	static public RenderingExtension instance;

	void Awake()
	{
		instance = this;
	}

	static public void AsyncRendering(System.Action action)
	{
		instance.StartCoroutine(instance.enumerator(() => action()));
	}

	private IEnumerator enumerator(System.Action action)
	{
		yield return new WaitForEndOfFrame();
		action();
	}
}
