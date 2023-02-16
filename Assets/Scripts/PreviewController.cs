using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PreviewController : MonoBehaviour {

	public Text CategoryText;
	public Text InfoText;
	public RawImage rawImage;

	private GameObject Scripts;
	private System.Action nextAction;

	public void Init(UserCategory category, List<Testing> testings, System.Action action)
    {
		nextAction = action;
		if (category?.file != null)
        {
			Scripts.GetComponent<GetDataService>().GetTexture(category.file.path, (texture) => drawPicture(texture), null);
		}
		CategoryText.text = $"«{category.title.ToUpper()}».";
		fillInfo(testings);
	}

	void Awake () {
		rawImage.color = new Color(1, 1, 1, 0);
		Scripts = GameObject.Find("Scripts");
    }

	void drawPicture(Texture texture)
    {
		rawImage.color = new Color(1, 1, 1, 1);
		rawImage.texture = texture;
    }

	void fillInfo(List<Testing> testings)
    {
		string countText = testings.Count < 2 ? "конкурсный этап" : "конкурсных этапа";
		InfoText.text = $"На электронной платформе, где Вы сейчас находитесь, проведется {testings.Count} {countText}:\n\n";
		testings.ForEach((x) => InfoText.text += $"{testings.FindIndex(t => x == t) + 1}. {x.title}\n\n");
    }

	public void ButtonNext()
    {
		nextAction?.Invoke();
    }

}
