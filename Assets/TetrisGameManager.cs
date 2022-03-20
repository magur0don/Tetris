using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TetrisGameManager : MonoBehaviour
{
    public TetrisBlock NowFallTetrisBlock = null;
    private static TetrisGameManager tetrisGameManagerInstance;
    public TextMeshProUGUI ScoreText;
    public int Score = 0;

    public TextMeshProUGUI GameEndText;

    public float EndTime = 5f;

    public int ClearScore = 100;

    public enum GameState
    {
        Invalide,
        GameStart,
        GameMain,
        GameEnd
    }

    public GameState GameStatus = GameState.Invalide;

    public void SetScore(int addScore)
    {
        Score += addScore;
        ScoreText.text = $"Score : {Score}";
    }

    public static TetrisGameManager Instance
    {
        get
        {
            return tetrisGameManagerInstance;
        }
    }

    private void Awake()
    {
        if (tetrisGameManagerInstance == null)
        {
            tetrisGameManagerInstance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        switch (GameStatus)
        {
            case GameState.Invalide:
                GameStatus = GameState.GameStart;
                break;

            case GameState.GameStart:

                GameStatus = GameState.GameMain;
                break;

            case GameState.GameMain:
                if (ClearScore <= Score)
                {
                    GameStatus = GameState.GameEnd;
                }
                break;

            case GameState.GameEnd:

                EndTime -= Time.deltaTime;
                if (ClearScore <= Score)
                {
                    GameEndText.text = "GameClear";
                }
                else
                {
                    GameEndText.text = "GameOver";
                }
                if (EndTime < 0)
                {

                    SceneManager.LoadScene("Tetris");

                }

                break;

        }
    }


}
