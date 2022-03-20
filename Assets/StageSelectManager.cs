using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    public void SetStage(int ClearScore)
    {
        SceneManager.LoadScene("Tetris");
        TetrisGameManager.ClearScore = ClearScore;
    }

}
