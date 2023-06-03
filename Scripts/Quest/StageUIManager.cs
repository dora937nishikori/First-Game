using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//StageUIを管理(ステージ数、進行ボタン、戻るボタン)
public class StageUIManager : MonoBehaviour
{
    private void Start()
    {
        stageClearImage.SetActive(false);
    }
    public Text stageText;
    public GameObject nextButton;
    public GameObject toTownButton;
    public GameObject stageClearImage;
    public GameObject toTitleButton;
    

    public void UpdateUI(int currentStage)
    {
        stageText.text = currentStage.ToString();
        stageText.text = string.Format("ステージ：{0}",currentStage);
        toTitleButton.SetActive(false);
    }

    //敵に遭遇時ボタンを隠す
    public void HideButtons()
    {
        nextButton.SetActive(false);
        toTownButton.SetActive(false);
        toTitleButton.SetActive(false);

    }

    public void ShowButtons()
    {
        nextButton.SetActive(true);
        toTownButton.SetActive(true);
    }

    //クリア時処理
    public void ShowClearText()
    {
        stageClearImage.SetActive(true);
        nextButton.SetActive(false);
        toTitleButton.SetActive(true);

    }
}
