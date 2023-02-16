using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class SocketContoller : MonoBehaviour
{
    private static HubConnection connection;

    private List<System.Action> actionList = new List<System.Action>();

    private string wsUrl => $"{PlayerPrefs.GetString("RestUrl")}/signalr";

    private int lastPano = -1;
    private TestingVrHelmet lastVr = null;

    private void Awake()
    {
        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);
    }

    private void LateUpdate()
    {
        threadHandler();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        CreateConnection();
    }

    public async Task CreateConnection()
    {
        if (connection != null)
        {
            await connection.StopAsync();
            await connection.DisposeAsync();
        }
        Debug.Log(wsUrl);
        connection = new HubConnectionBuilder()
            .WithUrl(wsUrl)
            .Build();
        connection.Closed += async (error) =>
        {
            await Task.Delay(5 * 1000);
            await connection.StartAsync();
        };
        await Connect();
    }

    private async Task Connect()
    {
        connection.On<string>("Send", message =>
        {
            Debug.Log($"msg: {message}");
        });
        connection.On<string>("VrDeviceConnect", message =>
        {
            Debug.Log($"connect: {message}");
            actionList.Add(() => OnVrConnect(message));
        });
        connection.On<string>("VrDevicesHub", message =>
        {
            Debug.Log($"available devices: {message}");
            actionList.Add(() => SetDevicesList(message));
        });
        connection.On<string>("SetExperienceVrState", message =>
        {
            actionList.Add(() => SetVrState(message));
        });
        connection.On("StartVrView", () =>
        {
            actionList.Add(() => StartVrView());
        });
        connection.On<string>("SendResult", message =>
        {
            actionList.Add(() => SendResult(message));
        });

        try
        {
            await connection.StartAsync();
            Debug.Log("Connection started");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

        await Send("PC");
        await GetDevices();
    }

    private void SetVrState(string msg)
    {
        var state = JsonUtility.FromJson<ExperienceVrState>(msg);
        GameObject.Find("VrScenes")?.GetComponent<StateController>()?.SetState(state);
    }

    private void StartVrView()
    {
        GameObject.Find("VrScenes")?.GetComponent<StateController>()?.Activate();
    }

    private void SendResult(string code)
    {
        GameObject.Find("VrExperienceComponent")?.GetComponent<VrExperienceComponent>()?.SetCode(code);
        GameObject.Find("VrScenes")?.GetComponent<StateController>()?.Disactivate();
    }

    private void SetDevicesList(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return;
        }
        var obj = JsonUtility.FromJson<JsonContainer<List<string>>>(json);
        var scriptsGo = GameObject.Find("Scripts");
        if (scriptsGo == null)
        {
            return;
        }
        if (!scriptsGo.GetComponent<SettingsController>())
        {
            return;
        }
        scriptsGo.GetComponent<SettingsController>().VrDevices = obj.data;
    }

    public async Task GetDevices()
    {
        try
        {
            await connection.InvokeAsync("GetVrDevices");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private async Task Send(string msg)
    {
        try
        {
            await connection.InvokeAsync("Send", msg);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public async Task SetActivePano(string deviceId, int panoIdx)
    {
        this.lastPano = panoIdx;
        this.lastVr = null;

        try
        {
            Debug.Log($"SetActivePano: {deviceId} {panoIdx}");
            await connection.InvokeAsync("SetActivePano", deviceId, panoIdx);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public async Task StartVrHeadset(string deviceId, TestingVrHelmet model)
    {
        lastVr = model;
        lastPano = -1;

        try
        {
            Debug.Log($"{deviceId} activate headset");
            var msg = JsonUtility.ToJson(model);
            await connection.InvokeAsync("StartVrHeadset", deviceId, msg);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public async Task CloseVr(string deviceId)
    {
        try
        {
            Debug.Log($"{deviceId} close vr");
            await connection.InvokeAsync("CloseVr", deviceId, "close");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public async Task ClearPano(string deviceId)
    {
        try
        {
            await connection.InvokeAsync("VrPanoClear", deviceId);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void ClearLastValues()
    {
        lastPano = -1;
        lastVr = null;
    }

    public async void OnVrConnect(string deviceId)
    {
        Debug.Log("OnVrConnect");

        if (PlayerPrefs.GetString("VrDevice") != deviceId)
        {
            Debug.Log("VrDeviceDEstroy " + PlayerPrefs.GetString("VrDevice"));
            return;
        }

        await Task.Delay(3 * 1000);

        if (lastPano != -1)
        {
            await SetActivePano(deviceId, this.lastPano);
            return;
        }

        if (lastVr != null)
        {
            await StartVrHeadset(deviceId, lastVr);
            return;
        }
    }

    private void threadHandler()
    {
        if (!(actionList?.Count > 0))
        {
            return;
        }
        var savedActionList = new List<System.Action>(actionList);
        foreach (var action in savedActionList)
        {
            action();
        }
        actionList.Clear();
    }
}
