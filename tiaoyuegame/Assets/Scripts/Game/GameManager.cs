using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ManagerVars vars;
    public static GameManager Instance;

    public bool IsGameStarted { get; set; }

    public bool IsGameOver { get; set; }

    public bool IsPause { get; set; }

    public bool PlayerIsMove { get; set; }


    private int gameScore;
    private int gameDiamond;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        Instance = this;
        EventCenter.AddListener(EventDefine.AddScore, AddGameScore);
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond, AddGameDiamond);


        if (GameData.IsAgainGame)
        {
            IsGameStarted = true;
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore, AddGameScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddGameDiamond);


    }

    private void PlayerMove()
    {
        PlayerIsMove = true;
    }    


    private void AddGameScore()
    {
        if (IsGameStarted == false||IsGameOver ||IsPause)
        {
            return;
        }
        gameScore++;
        EventCenter.Broadcast(EventDefine.UpdateScoreText, gameScore);
    }

    public int GetGameScore()
    {
        return gameScore; 
    }
    private void AddGameDiamond()
    {
        gameDiamond++;
        EventCenter.Broadcast(EventDefine.UpdateDiamondText, gameDiamond);
    }


    public int GetGameDiamond()
    {
        return gameDiamond;
    }
}
