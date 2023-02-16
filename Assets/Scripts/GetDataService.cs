using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GetDataService : MonoBehaviour {

    public bool IsOnline;

    public async void SetActivePano(int panoIdx)
    {
        if (!IsOnline)
        {
            return;
        }
        var SocketHub = GameObject.Find("SocketHub");
        var device = PlayerPrefs.GetString("VrDevice");
        await SocketHub.GetComponent<SocketContoller>().SetActivePano(device, panoIdx);
    }

    public async void SetActiveVr(TestingVrHelmet hemlet)
    {
        if (!IsOnline)
        {
            return;
        }
        var SocketHub = GameObject.Find("SocketHub");
        var device = PlayerPrefs.GetString("VrDevice");
        Debug.Log("set from gd");
        await SocketHub.GetComponent<SocketContoller>().StartVrHeadset(device, hemlet);
    }

    public async void CloseVr()
    {
        if (!IsOnline)
        {
            return;
        }
        var SocketHub = GameObject.Find("SocketHub");
        var device = PlayerPrefs.GetString("VrDevice");
        await SocketHub.GetComponent<SocketContoller>().CloseVr(device);
    }

    public void CheckConnection(string url, System.Action nextAction, System.Action errorAction)
    {
        StartCoroutine(
            this.GetComponent<WebService>()
            .CheckConnection(
                url,
                nextAction,
                errorAction
            )
        );
    }

    public void GetTexture(string url, System.Action<Texture> nextAction, System.Action errorAction)
    {
        StartCoroutine(
            this.GetComponent<WebService>()
            .GetTexture(
                url,
                nextAction,
                errorAction
            )
        );
    }

    public void SyncResults(System.Action nextAction, System.Action errorAction)
    {
        var local = getLocalUids();
        getRestUids((res) => {
            var uids = uidsCompare(res, local);
            syncResult(nextAction, errorAction, uids);
        }, errorAction);
    }

    private List<string> uidsCompare(List<string> rest, List<string> local)
    {
        var uids = new List<string>();
        foreach (var uid in local)
        {
            if (!rest.Exists(res => res == uid))
            {
                uids.Add(uid);
            }
        }
        return uids;
    }

    private void syncResult(System.Action nextAction, System.Action errorAction, List<string> uids, int idx = 0)
    {
        errorAction();
        if (idx >= uids.Count)
        {
            nextAction?.Invoke();
            return;
        }
        var json = getLocalResultsByUid(uids[idx]);
        saveResultApi(json, (res) => syncResult(nextAction, errorAction, uids, ++idx), errorAction);
    }

    private List<string> getLocalUids()
    {
        var files = Directory.GetFiles("Results");
        var uids = new List<string>();
        foreach (var file in files)
        {
            uids.Add(file.Split('/')[1].Split('.')[0]);
        }
        return uids;
    }

    private void getRestUids(System.Action<List<string>> nextAction = null, System.Action errorAction = null)
    {
        StartCoroutine(
            this.GetComponent<WebService>()
            .GetRequestCoroutine<List<string>>(
                "testing/result/uids",
                nextAction,
                errorAction
            )
        );
    }

    private string getLocalResultsByUid(string uid)
    {
        string json = System.IO.File.ReadAllText($"Results/{uid}.txt");
        return json;
    }

    public void SaveResult(CompetitionResult result)
    {
        var jsonResult = JsonUtility.ToJson(result);
        saveResultLocal(jsonResult, result.uid);
        if (IsOnline)
        {
            saveResultApi(jsonResult);
        }
    }

    private void saveResultApi(string json, System.Action<CompetitionResult> nextAction = null, System.Action errorAction = null)
    {
        StartCoroutine(
            this.GetComponent<WebService>()
            .PostRequestCoroutine<CompetitionResult>(
                "testing/result",
                json,
                nextAction,
                errorAction
            )
        );
    }

    private void saveResultLocal(string json, string uid)
    {
        using (FileStream fstream = new FileStream($"Results/{uid}.txt", FileMode.OpenOrCreate))
        {
            byte[] array = System.Text.Encoding.Default.GetBytes(json);
            fstream.Write(array, 0, array.Length);
        }
    }

    public void GetCategories(System.Action<List<UserCategory>> nextAction, System.Action errorAction)
    {
        if (IsOnline)
        {
            getCategoriesApi(nextAction, errorAction);
            //getCategoriesLocal(nextAction, errorAction);
        }
        else
        {
            getCategoriesLocal(nextAction, errorAction);
        }
    }

    private void getCategoriesLocal(System.Action<List<UserCategory>> nextAction, System.Action errorAction = null)
    {
        string json = System.IO.File.ReadAllText("LocalPack.txt");
        var pack = JsonUtility.FromJson<LocalPack>(json);
        nextAction(pack.userCategories);
    }

    private void getCategoriesApi(System.Action<List<UserCategory>> nextAction, System.Action errorAction)
    {
        StartCoroutine(
            this.GetComponent<WebService>()
            .GetRequestCoroutine(
                "user/category/all/pack",
                nextAction,
                errorAction
            )
        );
    }

    public void GetTestingList(int setId, System.Action<List<Testing>> nextAction, System.Action errorAction)
    {
        if (IsOnline)
        {
            getTestingListApi(setId, nextAction, errorAction);
            //getTestingListLocal(setId, nextAction, errorAction);
        }
        else
        {
            getTestingListLocal(setId, nextAction, errorAction);
        }
    }

    private void getTestingListApi(int setId, System.Action<List<Testing>> nextAction, System.Action errorAction)
    {
        StartCoroutine(
            this.GetComponent<WebService>()
            .GetRequestCoroutine(
                $"testing/testing/pack/{setId}",
                nextAction,
                errorAction
            )
        );
    }

    private void getTestingListLocal(int setId, System.Action<List<Testing>> nextAction, System.Action errorAction = null)
    {
        string json = System.IO.File.ReadAllText("LocalPack.txt");
        var pack = JsonUtility.FromJson<LocalPack>(json);
        var testings = new List<Testing>();
        pack.sets.FirstOrDefault(set => set.id == setId).stages.ForEach(stage =>
        {
            //testings.Add(CopyExtension<Testing>.Copy(pack.testings.FirstOrDefault(test => test.id == stage.testingId)));
            testings.Add(pack.testings.FirstOrDefault(test => test.id == stage.testingId).Copy());
        });
        nextAction(testings);
    }
}
