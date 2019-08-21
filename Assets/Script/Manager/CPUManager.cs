using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUManager : MonoBehaviour
{
    public const int NUMBEROFSEARCHING = 1000;

    //CPU側の処理手順を示した関数
    public void cpuProcess() {
        if (GameController.turnNumber == 0) GameController.turnNumber = 1;
        else GameController.turnNumber = 0;

        if (GridManager.checkPassed(GameController.gridManager.gridStoneNumbers)) {
            GameController.uiManager._log.plusLog("CPU", true);
            GameController.gridManager._judgeCheckMate.passCount++;
        } else {
            int stoneNumber = GameController.turnNumber + 1;
            int row, column;
            nextHand(out row, out column);

            //ログを送信
            GameController.uiManager._log.plusLog("CPU", false, row, column);
        }

        if (GameController.turnNumber == 0) GameController.turnNumber = 1;
        else GameController.turnNumber = 0;

        GameController.playerIsPlaced = false;
    }

    //次の手を考える関数
    public void nextHand(out int row, out int column) {
        //FIXME:マスごとの評価値(もっといい言い方があるかも？)
        int[,] predictionGridNumbers = new int[GridManager.GRIDSIZE, GridManager.GRIDSIZE];
        int[,] scoreOfGrid = new int[GridManager.GRIDSIZE, GridManager.GRIDSIZE];
        string hand;
        int maxScore;
        int rowOfMaxScore;
        int columnOfMaxScore;

        //それぞれのマスの評価値を計算する処理
        for (row = 0; row < GridManager.GRIDSIZE; row++) {
            for (column = 0; column < GridManager.GRIDSIZE; column++) {
                //予想盤面をここで初期化する
                Array.Copy(GameController.gridManager.gridStoneNumbers, predictionGridNumbers, GridManager.GRIDSIZE * GridManager.GRIDSIZE);

                //そもそも置けない場合、それ以上やっても意味はないので次のマスへ
                if (!GameController.gridManager._judgeCanPutDown.canPutDown(predictionGridNumbers, row, column)) {
                    //どんな評価値が出てもこれを下回らないようにする(置けないマスが選ばれてはだめなので)
                    scoreOfGrid[row, column] = -1024;
                    continue;
                }
                //一旦そのマスの評価値を計算
                scoreOfGrid[row, column] = calculateScoreOfGrid(predictionGridNumbers, row, column);

                //端だった場合、評価値を加算
                scoreOfGrid[row, column] += fixScore(row, column);

                //そこに打ったと仮定して、予想盤面を更新
                predictionGridNumbers = nextGrid(predictionGridNumbers, row, column);
                //ここから最大1000手先までを予測、評価値を足すor引く
                for (int i = 0; i < NUMBEROFSEARCHING; i++) {
                    GameController.gridManager._judgeCheckMate.passCount = 0;
                    if (GameController.turnNumber == 0) GameController.turnNumber = 1;
                    else GameController.turnNumber = 0;

                    //次は敵の手番のため、引く
                    maxScore = searchMaxScore(copyGrid, out rowOfMaxScore, out columnOfMaxScore);
                    if (maxScore != -1024) {
                        scoreOfGrid[row, column] -= maxScore;
                        hand = shapingNumber(rowOfMaxScore, columnOfMaxScore);
                        copyGrid = nextGrid(copyGrid, hand);

                        //端の場合引く
                        scoreOfGrid[row, column] -= fixScore(rowOfMaxScore, columnOfMaxScore);
                    } else
                        GameController.passCount++;

                    if (GameController.turnNumber == 0) GameController.turnNumber = 1;
                    else GameController.turnNumber = 0;

                    maxScore = searchMaxScore(copyGrid, out rowOfMaxScore, out columnOfMaxScore);
                    //その次の自分の手番のため、足す
                    if (maxScore != -1024) {
                        scoreOfGrid[row, column] += maxScore;
                        hand = shapingNumber(rowOfMaxScore, columnOfMaxScore);
                        copyGrid = nextGrid(copyGrid, hand);

                        scoreOfGrid[row, column] += fixScore(rowOfMaxScore, columnOfMaxScore);
                    } else
                        GameController.passCount++;

                    //2回打てなかった場合、もう探索は意味なし
                    if (GameController.passCount == 2) break;
                }

            }
        }

        //最高の評価値のマスの行、列を探索する処理
        //FIXME:これをsearchMaxScoreに統合しようとしたところ、打てないところに打つバグが出たため冗長だが暫定的にここに置いている
        maxScore = scoreOfGrid[0, 0];
        rowOfMaxScore = 0;
        columnOfMaxScore = 0;
        for (int row = 0; row < GridManager.GRIDSIZE; row++) {
            for (int column = 0; column < GridManager.GRIDSIZE; column++) {
                if (maxScore < scoreOfGrid[row, column]) {
                    maxScore = scoreOfGrid[row, column];
                    rowOfMaxScore = row;
                    columnOfMaxScore = column;
                }
            }
        }
    }

    //四隅の場合、評価値を足す(あるいは引く)関数
    public static int fixScore(int row, int column) {
        int score = 0;
        if (row == 0 || row == 7) score += 5;
        if (column == 0 || column == 7) score += 5;
        return score;
    }

    //指定されたマスの評価値を計算する関数(その盤面のみ対象)
    public int calculateScoreOfGrid(int[,] gridNumbers, int row, int column) {
        int score = 0;
        if (!GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column))
            return -1024;

        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 1, 0))
            score += checkScoreInDirection(gridNumbers, row, column, 1, 0); // 右
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 0, 1))
            score += checkScoreInDirection(gridNumbers, row, column, 0, 1); // 下
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, -1, 0))
            score += checkScoreInDirection(gridNumbers, row, column, -1, 0); // 左
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 0, -1))
            score += checkScoreInDirection(gridNumbers, row, column, 0, -1); // 上
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 1, 1))
            score += checkScoreInDirection(gridNumbers, row, column, 1, 1); // 右下
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, -1, -1))
            score += checkScoreInDirection(gridNumbers, row, column, -1, -1); // 左上
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 1, -1))
            score += checkScoreInDirection(gridNumbers, row, column, 1, -1); // 右上
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, -1, 1))
            score += checkScoreInDirection(gridNumbers, row, column, -1, 1); // 左下

        return score;
    }

    //渡された盤面の最大の評価値、その行番号、列番号を検索する関数(その盤面のみ対象)
    public static int searchMaxScore(int[,] gridNumbers, out int rowOfMaxScore, out int columnOfMaxScore) {
        int[,] scoreOfGrids = gridNumbers;
        scoreOfGrids = new int[GridManager.GRIDSIZE, GridManager.GRIDSIZE];
        for (int row = 0; row < GridManager.GRIDSIZE; row++) {
            for (int column = 0; column < GridManager.GRIDSIZE; column++)
                scoreOfGrids[row, column] = calculateScoreOfGrid(gridNumbers, row, column);
        }

        //最高の評価値のマスを探索する処理
        int maxScore = scoreOfGrids[0, 0];
        rowOfMaxScore = 0;
        columnOfMaxScore = 0;
        for (int row = 0; row < GridManager.GRIDSIZE; row++) {
            for (int column = 0; column < GridManager.GRIDSIZE; column++) {
                if (maxScore < scoreOfGrids[row, column]) {
                    maxScore = scoreOfGrids[row, column];
                    rowOfMaxScore = row;
                    columnOfMaxScore = column;
                }
            }
        }

        return maxScore;
    }

    //指定された方向のひっくり返せる個数を調べる関数
    public static int checkScoreInDirection(int[,] gridNumbers, int row, int column, int vecColumn, int vecRow) {
        int stone = GameController.turnNumber + 1;
        int scoreInDirection = 0;
        row += vecRow;
        column += vecColumn;

        while (gridNumbers[row, column] != stone) {
            scoreInDirection++;
            row += vecRow;
            column += vecColumn;
        }

        return scoreInDirection;
    }
}
