using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour {

	public InputField FirstNameGo;
	public InputField MiddleNameGo;
	public InputField LastNameGo;
	public GameObject ErrorGo;
	public Text ActiveCategoryText;

	private List<UserCategory> userCategories;
	private string activeCategory;
	private System.Action<User> nextAction;

	private GameObject Scripts;

    private void Start()
    {
		Scripts = GameObject.Find("Scripts");
    }

    private void LateUpdate()
    {
		ActiveCategoryText.text = string.IsNullOrEmpty(activeCategory) ? "Не выбрано" : activeCategory;
	}

    public void Init(List<UserCategory> categories, System.Action<User> action)
    {
		userCategories = categories;
		nextAction = action;
		//fillDropdown();
    }

	public User GetResult()
    {
		if (!checkData())
        {
			throw new Exception();
        }
		User user = new User
		{
			firstName = FirstNameGo.text.Trim(),
			middleName = MiddleNameGo.text.Trim(),
			lastName = LastNameGo.text.Trim(),
			userCategoryId = userCategories.FirstOrDefault(x => x.title == activeCategory)?.id ?? 0,
			category = userCategories.FirstOrDefault(x => x.title == activeCategory) ?? null,
        };
		Debug.Log(user.userCategoryId);
		return user;
    }

	public void NextButton()
    {
		var user = GetResult();
		nextAction(user);
    }

	private bool checkData()
    {
		if (string.IsNullOrEmpty(FirstNameGo.text) || string.IsNullOrEmpty(LastNameGo.text) || (userCategories.FirstOrDefault(x => x.title == activeCategory) == null))
        {
			ErrorGo.SetActive(true);
			return false;
        }
		return true;
    }

	public void ChooseCategory()
    {
		Scripts.GetComponent<MainController>().PopupChoose(userCategories.Select(x => x.title).ToList(), activeCategory, "Выберите Вашу категорию", (res) =>
		{
			activeCategory = res;
		});
    }
}
