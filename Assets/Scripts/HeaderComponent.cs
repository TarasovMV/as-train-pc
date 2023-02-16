using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderComponent : MonoBehaviour {

    public Text StageInfoNumberText;
    public Text StageInfoTitleText;
    public GameObject UserInfo;
    public Text UserFioText;
    public Text UserCategoryText;

    public void SetStageInfo(int stageIdx, string stageTitle)
    {
        StageInfoNumberText.text = $"{stageIdx + 1} ЭТАП.";
        StageInfoTitleText.text = stageTitle.ToUpper();
    }

    public void SetUserInfo(User user)
    {
        UserInfo.SetActive(true);
        UserFioText.text = $"{user.lastName} {user.firstName} {user.middleName}";
        UserCategoryText.text = $"{user.category.title}";
    }
}
