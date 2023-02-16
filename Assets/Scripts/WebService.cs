using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebService : MonoBehaviour
{
	private string restApiUrl => $"{PlayerPrefs.GetString("RestUrl")}/api";
	private string fileUrl => $"{PlayerPrefs.GetString("RestUrl")}";


	public IEnumerator GetTexture(string url, System.Action<Texture> thenCallback, System.Action errorCallback)
    {
		Debug.Log($"{fileUrl}{url}");
		UnityWebRequest www = UnityWebRequestTexture.GetTexture($"{fileUrl}{url}");
		yield return www.SendWebRequest();
		

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
			errorCallback?.Invoke();
		}
		else
		{
			Texture result = DownloadHandlerTexture.GetContent(www);
			thenCallback?.Invoke(result);
		}
	}

	public IEnumerator CheckConnection(string url, System.Action thenCallback, System.Action errorCallback)
    {
		if (url == null)
        {
			url = PlayerPrefs.GetString("RestUrl");
        }
		string checkUrl = "api/start";
		UnityWebRequest www = UnityWebRequest.Get($"{url}/{checkUrl}");
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
			errorCallback?.Invoke();
		}
		else
		{
			Debug.Log(www.downloadHandler.text);
			thenCallback?.Invoke();
		}
	}

	public IEnumerator GetRequestCoroutine<T>(string url, System.Action<T> thenCallback, System.Action errorCallback)
	{
		Debug.Log($"{restApiUrl}/{url}");
		UnityWebRequest www = UnityWebRequest.Get($"{restApiUrl}/{url}");
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
			errorCallback?.Invoke();
		}
		else
		{
			Debug.Log(www.downloadHandler.text);
			thenCallback?.Invoke(JsonUtility.FromJson<JsonContainer<T>>(www.downloadHandler.text).data);
		}
	}

	public IEnumerator PostRequestCoroutine<T>(string url, string body, System.Action<T> thenCallback, System.Action errorCallback)
	{
		var request = new UnityWebRequest($"{restApiUrl}/{url}", "POST");
		byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
		request.uploadHandler = new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		yield return request.SendWebRequest();

		if (request.isNetworkError || request.isHttpError)
		{
			Debug.LogError(request.error);
			errorCallback?.Invoke();
		}
		else
		{
			Debug.Log(request.downloadHandler.text);
			thenCallback?.Invoke(JsonUtility.FromJson<JsonContainer<T>>(request.downloadHandler.text).data);
		}
	}

	public IEnumerator GetFile(string fileName)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(restApiUrl + "/start/report"))
		{
			yield return www.SendWebRequest();
			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				System.IO.File.WriteAllBytes(fileName, www.downloadHandler.data);
			}
		}
	}
}


