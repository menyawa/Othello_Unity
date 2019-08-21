using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int blackPoint;
    public int whitePoint;
    public GameObject _stonePrefab;

    public const int GRIDSIZE = 8;
    public int[,] gridStoneNumbers = new int[GRIDSIZE, GRIDSIZE];

    public NextGrid _nextGrid;
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
        _nextGrid = new NextGrid();
        _judgeCanPutDown = new JudgeCanPutDown();
        _judgeCheckMate = new JudgeCheckMate();

        blackPoint = 2;
        whitePoint = 2;
    }

    public void initGrids() {
        for (int row = 0; row < GRIDSIZE; row++) {
            for (int column = 0; column < GRIDSIZE; column++) {
                gridStoneNumbers[row, column] = 0;
            }
        }

        //最初の石を置く
        //-1しているのはインデックスが0から始まるため
        gridStoneNumbers[GRIDSIZE / 2 - 1, GRIDSIZE / 2 - 1] = 2;
        gridStoneNumbers[GRIDSIZE / 2, GRIDSIZE / 2] = 2;
        gridStoneNumbers[GRIDSIZE / 2 - 1, GRIDSIZE / 2] = 1;
        gridStoneNumbers[GRIDSIZE / 2, GRIDSIZE / 2 - 1] = 1;
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
                if (GameController.gridManager.gridStoneNumbers[row, column] == 1) blackCount++;
                if (GameController.gridManager.gridStoneNumbers[row, column] == 2) whiteCount++;
            }
        }

        blackPoint = blackCount;
        whitePoint = whiteCount;
    }

    /// <summary>
    /// パスかどうかを判断する関数
    /// </summary>
    /// <param name="gridNumbers"></param>
    /// <returns></returns>
    public static bool checkPassed(int[,] gridNumbers) {
        bool pass = true;
        for (int row = 0; row < GRIDSIZE; row++) {
            for (int column = 0; column < GRIDSIZE; column++)
                if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column)) pass = false;
        }

        return pass;
    }
}
