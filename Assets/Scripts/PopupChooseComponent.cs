using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PopupChooseComponent : MonoBehaviour {

	public Text Title;
	public GameObject ToggleGo;
	public Transform ToggleContainer;

	private List<GameObject> ToggleList = new List<GameObject>();
    private System.Action<string> nextAction;

    void Start () {
		
	}

    public void Init(List<string> devices, string activeDevice, string title, System.Action<string> action)
    {
        nextAction = action;
        Title.text = title;
        fillToggles(devices, activeDevice);
    }

	public void ToggleChoose(int idx)
    {
        if (!ToggleList[idx].GetComponent<Toggle>().isOn)
        {
            return;
        }
        ToggleList.ForEach((x) =>
		{
			if (x != ToggleList[idx])
				x.GetComponent<Toggle>().isOn = false;
		});
    }

	public void ButtonCancel()
    {
		Destroy(this.gameObject);
	}

	public void ButtonOk()
    {
        var result = getCurrentToggle();
        if (result != null)
        {
            nextAction?.Invoke(result);
        }
        
		Destroy(this.gameObject);
    }

    private void fillToggles(List<string> toggles, string activeDevice)
    {
        if (!ToggleContainer.gameObject.activeSelf)
        {
            return;
        }
        int idx = 0;
        foreach (var toggle in toggles)
        {
            int index = idx;
            var toggleGo = Instantiate(ToggleGo);
            toggleGo.transform.SetParent(ToggleContainer.transform);
            toggleGo.GetComponent<toggleMain>().Init(toggle, 0, () => ToggleChoose(index));
            ToggleList.Add(toggleGo);
            idx++;
            if (activeDevice == toggle)
            {
                toggleGo.GetComponent<Toggle>().isOn = true;
            }
        }
    }

    private string getCurrentToggle()
    {
        foreach(var toggle in ToggleList)
        {
            if (toggle.GetComponent<Toggle>().isOn)
            {
                return toggle.GetComponent<toggleMain>().textLabel.GetComponent<Text>().text;
            }
        }
        return null;
    }
}
