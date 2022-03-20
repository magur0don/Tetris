using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisStage : MonoBehaviour
{
    public int WidthSize = 10;
    public int HeightSize = 20;

    private static TetrisStage tetrisStageInstance;

    public Transform[,] BlockGrids;

    public Transform HoldPlace;

    // static�ÓI�ȃV���O���g���p�^�[���ŊO����A�N�Z�X�ł���悤��
    public static TetrisStage Instance
    {
        get
        {
            return tetrisStageInstance;
        }
    }

    private void Awake()
    {
        if (tetrisStageInstance == null)
        {
            tetrisStageInstance = this;
        }
        else
        {
            Destroy(this);
        }
        BlockGrids = new Transform[tetrisStageInstance.WidthSize,tetrisStageInstance.HeightSize];
    }
}
