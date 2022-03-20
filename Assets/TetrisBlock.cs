using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public float FallTime = 0.5f;

    public bool IsFallen = false;

    public GameObject Ghost;

    public enum TetrisState
    {
        Invalide,
        IsHold,
        IsHolded

    }

    public TetrisState TetrisStatus = TetrisState.Invalide;
    private void Start()
    {
        Ghost = Instantiate(this.gameObject);
        Destroy(Ghost.GetComponent<TetrisBlock>());
        Ghost.name = "Ghost";

        foreach (Transform children in Ghost.transform)
        {
            children.GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.Rotate(Vector3.forward * 90);
            if (!ValidMove())
            {
                transform.Rotate(-Vector3.forward * 90);
            }
        }


        // もし右ボタンを押したら
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
            if (!ValidMove())
            {
                transform.position += Vector3.left;
            }

        }
        // 右ボタンじゃなくて左ボタンを押したら
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;

            if (!ValidMove())
            {
                transform.position += Vector3.right;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            for (int i = 0; i < TetrisStage.Instance.HeightSize; i++)
            {
                transform.position += Vector3.down;
                if (!ValidMove())
                {
                    transform.position += Vector3.up;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            switch (TetrisStatus)
            {
                case TetrisState.Invalide:
                    TetrisStatus = TetrisState.IsHold;
                    if (TetrisStage.Instance.HoldPlace.GetComponent<Transform>().childCount > 0)
                    {
                        var holdTetris = TetrisStage.Instance.HoldPlace.GetChild(0).GetComponent<TetrisBlock>();
                        holdTetris.enabled = true;
                        holdTetris.TetrisStatus = TetrisState.IsHolded;
                        holdTetris.transform.parent = null;

                        holdTetris.transform.position = this.transform.position;
                        holdTetris.Ghost.SetActive(true);
                        TetrisGameManager.Instance.NowFallTetrisBlock = holdTetris;
                    }
                    else
                    {
                        TetrisGameManager.Instance.NowFallTetrisBlock = null;
                    }

                    this.transform.parent = TetrisStage.Instance.HoldPlace;
                    this.transform.position = TetrisStage.Instance.HoldPlace.position;
                    Ghost.SetActive(false);
                    this.enabled = false;


                    break;

                case TetrisState.IsHold:
                    TetrisStatus = TetrisState.IsHolded;
                    break;

                case TetrisState.IsHolded:
                    break;
            }
        }


        FallTime -= Time.deltaTime;

        if (FallTime < 0)
        {
            transform.position += Vector3.down;
            if (!ValidMove())
            {
                Destroy(Ghost);
                transform.position += Vector3.up;

                foreach (Transform children in transform)
                {
                    int roundedY = Mathf.RoundToInt(children.transform.position.y);
                    // 0スタートなので19がステージの高さ20になる
                    if (roundedY >= TetrisStage.Instance.HeightSize - 1)
                    {
                        TetrisGameManager.Instance.GameStatus = TetrisGameManager.GameState.GameEnd;
                        return;
                    }
                }
                IsFallen = true;
                AddToGrids();
                CheckLines();
                this.enabled = false;
            }

            FallTime = 0.5f;
        }

        if (Ghost != null)
        {
            Ghost.transform.rotation = this.transform.rotation;
            Ghost.transform.position = this.transform.position;
            for (int i = 0; i < TetrisStage.Instance.HeightSize; i++)
            {
                Ghost.transform.position += Vector3.down;
                if (!ValidMove(Ghost.transform))
                {
                    Ghost.transform.position += Vector3.up;
                }
            }
        }
    }

    private void CheckLines()
    {

        var lineCount = 0;
        for (var line = TetrisStage.Instance.HeightSize - 1; line >= 0; line--)
        {
            if (HasLine(line))
            {
                lineCount++;
                DeleteLine(line);
                RowDown(line);
            }
        }

        if (lineCount > 0)
        {
            var bonus = lineCount * 5;
            var score = Mathf.RoundToInt(lineCount * bonus);
            TetrisGameManager.Instance.SetScore(score);
        }
    }

    private bool HasLine(int line)
    {

        for (var block = 0; block < TetrisStage.Instance.WidthSize; block++)
        {
            if (TetrisStage.Instance.BlockGrids[block, line] == null)
            {
                return false;
            }
        }
        return true;
    }


    private void DeleteLine(int line)
    {
        for (var block = 0; block < TetrisStage.Instance.WidthSize; block++)
        {
            Destroy(TetrisStage.Instance.BlockGrids[block, line].gameObject);
            TetrisStage.Instance.BlockGrids[block, line] = null;
        }
    }

    private void RowDown(int line)
    {
        for (var yBlocks = line; yBlocks < TetrisStage.Instance.HeightSize; yBlocks++)
        {
            for (var block = 0; block < TetrisStage.Instance.WidthSize; block++)
            {
                if (TetrisStage.Instance.BlockGrids[block, yBlocks] != null)
                {
                    TetrisStage.Instance.BlockGrids[block, yBlocks - 1] = TetrisStage.Instance.BlockGrids[block, yBlocks];

                    TetrisStage.Instance.BlockGrids[block, yBlocks] = null;


                    TetrisStage.Instance.BlockGrids[block, yBlocks - 1].transform.position += Vector3.down;
                }
            }
        }
    }


    private void AddToGrids()
    {
        foreach (Transform children in this.transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            TetrisStage.Instance.BlockGrids[roundedX, roundedY] = children;
        }
    }


    private bool ValidMove(Transform transform = null)
    {
        if (transform == null)
        {
            transform = this.transform;
        }
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            // roundedYがステージの高さより上だったら、生まれて間もないテトリミノなので通す
            if (roundedY >= TetrisStage.Instance.HeightSize)
            {
                return true;
            }

            if (roundedX < 0 || roundedX >= TetrisStage.Instance.WidthSize || roundedY < 0)
            {
                return false;
            }

            if (TetrisStage.Instance.BlockGrids[roundedX, roundedY] != null)
            {
                return false;
            }

        }
        return true;
    }

}
