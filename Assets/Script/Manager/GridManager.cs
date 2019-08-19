using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int blackPoint;
    public int whitePoint;

    public const int GRIDSIZE = 8;
    public int[,] grids = new int[GRIDSIZE, GRIDSIZE];

    public JudgeCheckMate _judgeCheckMate;

    // Start is called before the first frame update
    void Start()
    {
        initGridManager();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initGridManager() {
        initGrid();

        blackPoint = 2;
        whitePoint = 2;
    }

    public void initGrid() {
        for (int row = 0; row < GRIDSIZE; row++) {
            for (int column = 0; column < GRIDSIZE; column++)
                grids[row, column] = 0;
        }
        //-1しているのはインデックスが0から始まるため
        grids[GRIDSIZE / 2 - 1, GRIDSIZE / 2 - 1] = grids[GRIDSIZE / 2, GRIDSIZE / 2] = 2;
        grids[GRIDSIZE / 2 - 1, GRIDSIZE / 2] = grids[GRIDSIZE / 2, GRIDSIZE / 2 - 1] = 1;
    }

    /// <summary>
    /// 黒、白のポイントを計算する
    /// </summary>
    public void countPoint() {
        int blackCount = 0;
        int whiteCount = 0;
        for (int row = 0; row < GRIDSIZE; row++) {
            for (int column = 0; column < GRIDSIZE; column++) {
                //点数計算用に石の数をカウントしておく
                if (GameController.gridManager.grids[row, column] == 1) blackCount++;
                if (GameController.gridManager.grids[row, column] == 2) whiteCount++;
            }
        }

        blackPoint = blackCount;
        whitePoint = whiteCount;
    }
}
