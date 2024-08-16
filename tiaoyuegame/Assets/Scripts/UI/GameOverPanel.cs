using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public Text txt_Score, txt_BestScore,txt_AddDiamondCount;

    public Button btn_Restart, btn_Rank,btn_Home;


    private void Awake()
    {
        btn_Restart.onClick.AddListener(OnRestartButtonclick);
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Home.onClick.AddListener(OnHomeButtonClick);
        EventCenter.AddListener(EventDefine.ShowGameOverPanel, Show);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGameOverPanel, Show);

    }

    private void Show()
    {
        txt_Score.text = GameManager.Instance.GetGameScore().ToString();
        txt_AddDiamondCount.text =  "+"+GameManager.Instance.GetGameDiamond().ToString();
        gameObject.SetActive(true);
    }

    private void  OnRestartButtonclick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = true;

    }
    private void OnRankButtonClick()
    {

    }
    private void OnHomeButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = false;
    }
}
