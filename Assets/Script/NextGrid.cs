using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextGrid
{
    /// <summary>
    /// 指定した盤面の、指定されたマスに打ったときの変化した盤面状態を返却する関数
    /// </summary>
    /// <param name="gridNumbers"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public int[,] nextGrid(int[,] gridNumbers, int row, int column) {
        int stone = GameController.turnNumber + 1;
        gridNumbers[row, column] = stone;

        //ここからひっくり返す処理
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 1, 0))
            gridNumbers = reverse(gridNumbers, row, column, 1, 0);// 右
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 0, 1))
            gridNumbers = reverse(gridNumbers, row, column, 0, 1); // 下
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, -1, 0))
            gridNumbers = reverse(gridNumbers, row, column, -1, 0); // 左
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 0, -1))
            gridNumbers = reverse(gridNumbers, row, column, 0, -1); // 上
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 1, 1))
            gridNumbers = reverse(gridNumbers, row, column, 1, 1); // 右下
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, -1, -1))
            gridNumbers = reverse(gridNumbers, row, column, -1, -1); // 左上
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 1, -1))
            gridNumbers = reverse(gridNumbers, row, column, 1, -1); // 右上
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, -1, 1))
            gridNumbers = reverse(gridNumbers, row, column, -1, 1); // 左下

        return gridNumbers;
    }

    /// <summary>
    /// 指定された方向の石をひっくり返す関数
    /// </summary>
    /// <param name="gridNumbers"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="vecColumn"></param>
    /// <param name="vecRow"></param>
    /// <returns></returns>
    private int[,] reverse(int[,] gridNumbers, int row, int column, int vecColumn, int vecRow) {
        int stone = GameController.turnNumber + 1;
        row += vecRow;
        column += vecColumn;

        while (gridNumbers[row, column] != stone) {
            gridNumbers[row, column] = stone;
            row += vecRow;
            column += vecColumn;
        }

        return gridNumbers;
    }
}
