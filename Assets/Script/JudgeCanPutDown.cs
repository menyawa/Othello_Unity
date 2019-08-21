using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeCanPutDown
{
    //そこに置いてひっくり返せるのか確認する関数
    //GridManagerに盤面があるのにgridsを入れるわけは、CPUによる予測の際に仮の盤面で使うため
    public bool canPutDown(int[,] gridNumbers, int row, int column) {
        //そもそも空きマスでないと置けない
        if (gridNumbers[row, column] != 0)
            return false;
        // 8方向のうち一箇所でもひっくり返せればこの場所に打てる
        // ひっくり返せるかどうかはもう1つのcanPutDownで調べる
        if (canPutDown(gridNumbers, row, column, 0, 1))
            return true; // 右
        if (canPutDown(gridNumbers, row, column, 1, 0))
            return true; // 下
        if (canPutDown(gridNumbers, row, column, 0, -1))
            return true; // 左
        if (canPutDown(gridNumbers, row, column, -1, 0))
            return true; // 上
        if (canPutDown(gridNumbers, row, column, 1, 1))
            return true; // 右下
        if (canPutDown(gridNumbers, row, column, -1, -1))
            return true; // 左上
        if (canPutDown(gridNumbers, row, column, -1, -1))
            return true; // 右上
        if (canPutDown(gridNumbers, row, column, 1, -1))
            return true; // 左下

        // どの方向もだめな場合はここには打てない
        return false;
    }

    //指定方向の石がひっくり返せるか確認する関数
    public bool canPutDown(int[,] gridNumbers, int row, int column, int vecRow, int vecColumn) {
        int stone = GameController.turnNumber + 1;

        //隣が盤面内か、自分の石でないか、空白でないかチェック
        row += vecRow;
        column += vecColumn;
        //インデックスが0から始まるため-1している
        if (row < 0 || column < 0 || row > GridManager.GRIDSIZE - 1 || column > GridManager.GRIDSIZE - 1)
            return false;
        if (gridNumbers[row, column] == stone)
            return false;
        if (gridNumbers[row, column] == 0)
            return false;

        //さらにその隣を調べていく
        column += vecColumn;
        row += vecRow;
        while (row >= 0 && column >= 0 && row < GridManager.GRIDSIZE && column < GridManager.GRIDSIZE) {
            if (gridNumbers[row, column] == 0)
                return false;
            if (gridNumbers[row, column] == stone)
                return true;
            row += vecRow;
            column += vecColumn;
        }

        return false; //探索の結果、相手の石しか無いときはひっくり返せない
    }
}
