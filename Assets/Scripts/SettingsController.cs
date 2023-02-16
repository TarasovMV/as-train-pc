using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    public GameObject AlertContainer;
    public GameObject LoaderGo;
    public GameObject SnackbarGo;
    public GameObject PopupChooseGo;

    public InputField InputUrl;

    public Text ActiveVrText;

    public List<string> VrDevices = new List<string>();

    private void Start()
    {
        setActiveUrl();
        GameObject.Find("SocketHub")?.GetComponent<SocketContoller>()?.GetDevices();
    }

    private void LateUpdate()
    {
        setActiveVrText();
    }

    public void ButtonConnectToRest()
    {
        var url = InputUrl.text;
        this.gameObject.GetComponent<GetDataService>().CheckConnection(
            url,
            () => {
                showSnackBar("Connect to rest success!");
                PlayerPrefs.SetString("RestUrl", url);
                GameObject.Find("SocketHub")?.GetComponent<SocketContoller>()?.CreateConnection();
            },
            () => {
                showSnackBar("Connect error!", SnackbarController.SnackbarType.Error);
                PlayerPrefs.SetString("RestUrl", url);
            }
        );
    }

    public void ChooseDevice()
    {
        var go = Instantiate(PopupChooseGo);
        uiPosition(go);
        go.GetComponent<PopupChooseComponent>().Init(VrDevices, PlayerPrefs.GetString("VrDevice"), "Выберите сопряженное VR устройство", (device) => setActiveDevice(device));
    }

    public void SyncResult()
    {
        this.GetComponent<GetDataService>().SyncResults(
            () => {
                showSnackBar("Sync success!");
            },
            () => {
                showSnackBar("Sync error!", SnackbarController.SnackbarType.Error);
            }
        );
    }

    private void setActiveDevice(string device)
    {
        Debug.Log($"set active device: {device}");
        PlayerPrefs.SetString("VrDevice", device);
    }

    private void showSnackBar(string msg, SnackbarController.SnackbarType type = SnackbarController.SnackbarType.Info)
    {
        var snack = Instantiate(SnackbarGo);
        uiPosition(snack);
        snack.GetComponent<SnackbarController>().Init(msg, type);
    }

    private void initLoader()
    {
        var go = Instantiate(LoaderGo);
        uiPosition(go);
    }

    private void clearAlertContainer()
    {
        foreach(Transform child in AlertContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void setActiveVrText()
    {
        var result = PlayerPrefs.GetString("VrDevice");
        if (!VrDevices.Contains(result))
        {
            result = null;
        }
        ActiveVrText.text = string.IsNullOrEmpty(result) ? "Не выбрано" : result;
    }

    private void setActiveUrl()
    {
        InputUrl.text = PlayerPrefs.GetString("RestUrl");
    }

    void uiPosition(GameObject go)
    {
        go.GetComponent<RectTransform>().SetParent(AlertContainer.transform);
        go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
