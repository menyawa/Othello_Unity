using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int blackPoint;
    public int whitePoint;
    public GameObject _stonePrefab;

    public const int GRIDSIZE = 8;
    public GridData[,] grids = new GridData[GRIDSIZE, GRIDSIZE];

    public JudgeCanPutDown _judgeCanPutDown;
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
        initGrids();

        blackPoint = 2;
        whitePoint = 2;
    }

    public void initGrids() {
        for (int row = 0; row < GRIDSIZE; row++) {
            for (int column = 0; column < GRIDSIZE; column++) {
                grids[row, column].beforeStone = 0;
                grids[row, column].nowStone = 0;
            }
        }

        //最初の石を置く
        //-1しているのはインデックスが0から始まるため
        grids[GRIDSIZE / 2 - 1, GRIDSIZE / 2 - 1].nowStone = 2;
        grids[GRIDSIZE / 2, GRIDSIZE / 2].nowStone = 2;
        grids[GRIDSIZE / 2 - 1, GRIDSIZE / 2].nowStone = 1;
        grids[GRIDSIZE / 2, GRIDSIZE / 2 - 1].nowStone = 1;
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
                if (GameController.gridManager.grids[row, column].nowStone == 1) blackCount++;
                if (GameController.gridManager.grids[row, column].nowStone == 2) whiteCount++;
            }
        }

        blackPoint = blackCount;
        whitePoint = whiteCount;
    }

    /// <summary>
    /// パスかどうかを判断する関数
    /// </summary>
    /// <param name="grids"></param>
    /// <returns></returns>
    public static bool checkPassed(GridData[,] grids) {
        bool pass = true;
        for (int row = 0; row < GridManager.GRIDSIZE; row++) {
            for (int column = 0; column < GridManager.GRIDSIZE; column++)
                if (GameController.gridManager._judgeCanPutDown.canPutDown(grids, row, column)) pass = false;
        }

        return pass;
    }
}
