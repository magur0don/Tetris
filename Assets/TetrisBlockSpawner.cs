using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlockSpawner : MonoBehaviour
{
    public GameObject[] TetrisBlocks = new GameObject[7];

    private GameObject[] GameTetrisBlock = new GameObject[4];
    public Transform[] NextBlocks = new Transform[4];

    void Start()
    {
        TetrisBlockSpawn();
    }
    private void Update()
    {
        if (TetrisGameManager.Instance.GameStatus == TetrisGameManager.GameState.GameEnd)
        {
            return;
        }


        if (TetrisGameManager.Instance.NowFallTetrisBlock == null)
        {
            TetrisBlockSpawn();
        }

        if (TetrisGameManager.Instance.NowFallTetrisBlock.IsFallen)
        {
            TetrisBlockSpawn();
        }
    }

    public void TetrisBlockSpawn()
    {

        if (GameTetrisBlock[0] != null)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    var spawnNo = Random.Range(0, TetrisBlocks.Length);
                    var tetromino = Instantiate(TetrisBlocks[spawnNo]);
                    GameTetrisBlock[i] = tetromino;
                    SetBlocks(i);
                }
                else
                {
                    GameTetrisBlock[i] = GameTetrisBlock[i + 1];
                    SetBlocks(i);
                }
            }
        }

        if (GameTetrisBlock[0] == null)
        {
            for (int i = 0; i < 4; i++)
            {
                var spawnNo = Random.Range(0, TetrisBlocks.Length);
                var tetromino = Instantiate(TetrisBlocks[spawnNo]);
                GameTetrisBlock[i] = tetromino;
                SetBlocks(i);
            }
        }
        TetrisGameManager.Instance.NowFallTetrisBlock = GameTetrisBlock[0].GetComponent<TetrisBlock>();
        TetrisGameManager.Instance.NowFallTetrisBlock.enabled = true;
    }


    void SetBlocks(int i)
    {
        GameTetrisBlock[i].GetComponent<TetrisBlock>().enabled = false;
        GameTetrisBlock[i].transform.position = NextBlocks[i].position;
    }


}
