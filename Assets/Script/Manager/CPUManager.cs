using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CPUManager
{
    public bool cpuThinked;
    public const int NUMBEROFSEARCHING = 1000;

    //CPU側の処理手順を示した関数
    public void cpuProcess() {
        //思考中に何回も実行されることを防ぐ
        if (cpuThinked) return;

        cpuThinked = true;

        //CPUの処理に遅延を持たせるため、暫定的な処理
        //後でコルーチンにするかもしれない
        //遅延処理を行うとなぜかクリア処理が行われないため、一旦遅延無しで行う
        float waitSecond = 3.0f;
        DOVirtual.DelayedCall(waitSecond,
            () => {
                if (GameController.turnNumber == 0) GameController.turnNumber = 1;
                else GameController.turnNumber = 0;

                if (GridManager.checkPassed(GameController.gridManager.gridStoneNumbers)) {
                    GameController.uiManager._log.plusLog("てき", true);
                    GameController.gridManager._judgeCheckMate.passCount++;
                } else {
                    int stoneNumber = GameController.turnNumber + 1;
                    int row, column;
                    nextHand(out row, out column);

                    GameController.gridManager._nextGrid.updateGrid(GameController.gridManager.gridStoneNumbers, row, column);
                    //ログを送信
                    GameController.uiManager._log.plusLog("てき", false, row, column);

                    GameController.gridManager._judgeCheckMate.passCount = 0;
                }

                if (GameController.turnNumber == 0) GameController.turnNumber = 1;
                else GameController.turnNumber = 0;

                //これらはここでやらないと、置かれてないのに点数が増減したり、プレイヤーの持ち時間が減り続けたりする
                cpuThinked = false;
                GameController.uiManager._point.countPoint(GameController.gridManager.gridStoneNumbers);
                GameController.uiManager._point.printPoint();
                GameController.uiManager._log.printLog();
                GameController.playerIsPlaced = false;
            }
        );
    }

    /// <summary>
    /// CPU用に、次の手を考える
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void nextHand(out int row, out int column) {
        //FIXME:マスごとの評価値(もっといい言い方があるかも？)
        int[,] predictionGridNumbers = new int[GridManager.GRIDSIZE, GridManager.GRIDSIZE];
        int[,] scoreOfGrid = new int[GridManager.GRIDSIZE, GridManager.GRIDSIZE];
        int maxScore;
        int rowOfMaxScore, columnOfMaxScore;

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
                GameController.gridManager._nextGrid.updateGrid(predictionGridNumbers, row, column);

                //ここから最大1000手先までを予測、手番によって、評価値を足すor引く
                for (int searchCount = 0; searchCount < NUMBEROFSEARCHING; searchCount++) {
                    //既にCPUが打てることは決定されているので、パスカウントをリセットしても決着が伸びる心配はない
                    GameController.gridManager._judgeCheckMate.passCount = 0;
                    if (GameController.turnNumber == 0) GameController.turnNumber = 1;
                    else GameController.turnNumber = 0;

                    //次は敵の手番のため、その盤面で最大の評価値を求め、引く
                    maxScore = searchMaxScore(predictionGridNumbers, out rowOfMaxScore, out columnOfMaxScore);
                    if (maxScore != -1024) {
                        scoreOfGrid[row, column] -= maxScore;
                        GameController.gridManager._nextGrid.updateGrid(predictionGridNumbers, rowOfMaxScore, columnOfMaxScore);

                        //予測した手が端の場合、追加で評価値を引く
                        scoreOfGrid[row, column] -= fixScore(rowOfMaxScore, columnOfMaxScore);
                    } else {
                        GameController.gridManager._judgeCheckMate.passCount++;
                    }    

                    if (GameController.turnNumber == 0) GameController.turnNumber = 1;
                    else GameController.turnNumber = 0;

                    //その次の自分の手番のため、同じく最大の評価値を求め足す
                    maxScore = searchMaxScore(predictionGridNumbers, out rowOfMaxScore, out columnOfMaxScore);
                    if (maxScore != -1024) {
                        scoreOfGrid[row, column] += maxScore;
                        GameController.gridManager._nextGrid.updateGrid(predictionGridNumbers, rowOfMaxScore, columnOfMaxScore);

                        scoreOfGrid[row, column] += fixScore(rowOfMaxScore, columnOfMaxScore);
                    } else {
                        GameController.gridManager._judgeCheckMate.passCount++;
                    }

                    //2回打てなかった場合、もう探索は意味なしなので抜ける
                    if (GameController.gridManager._judgeCheckMate.judgeCheckmate()) break;
                }
            }
        }

        //探索中のパスカウントの加算をここで取り消す
        GameController.gridManager._judgeCheckMate.passCount = 0;

        //上で求めた評価値の配列から、最高の評価値のマスの行、列を探索する処理
        maxScore = scoreOfGrid[0, 0];
        rowOfMaxScore = 0;
        columnOfMaxScore = 0;
        for (row = 0; row < GridManager.GRIDSIZE; row++) {
            for (column = 0; column < GridManager.GRIDSIZE; column++) {
                if (maxScore < scoreOfGrid[row, column]) {
                    maxScore = scoreOfGrid[row, column];
                    rowOfMaxScore = row;
                    columnOfMaxScore = column;
                }
            }
        }

        //最終的に、一番評価値が高かったマスの行、列番号を返す
        row = rowOfMaxScore;
        column = columnOfMaxScore;
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

        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 0, 1))
            score += checkScoreInDirection(gridNumbers, row, column, 0, 1); // 右
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 1, 0))
            score += checkScoreInDirection(gridNumbers, row, column, 1, 0); // 下
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 0, -1))
            score += checkScoreInDirection(gridNumbers, row, column, 0, -1); // 左
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, -1, 0))
            score += checkScoreInDirection(gridNumbers, row, column, -1, 0); // 上
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 1, 1))
            score += checkScoreInDirection(gridNumbers, row, column, 1, 1); // 右下
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, -1, -1))
            score += checkScoreInDirection(gridNumbers, row, column, -1, -1); // 左上
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, -1, 1))
            score += checkScoreInDirection(gridNumbers, row, column, -1, 1); // 右上
        if (GameController.gridManager._judgeCanPutDown.canPutDown(gridNumbers, row, column, 1, -1))
            score += checkScoreInDirection(gridNumbers, row, column, 1, -1); // 左下

        return score;
    }

    //渡された盤面の最大の評価値、その行番号、列番号を検索する関数(その盤面のみ対象)
    public int searchMaxScore(int[,] gridNumbers, out int rowOfMaxScore, out int columnOfMaxScore) {
        int[,] scoreOfGrid = new int[GridManager.GRIDSIZE, GridManager.GRIDSIZE];
        for (int row = 0; row < GridManager.GRIDSIZE; row++) {
            for (int column = 0; column < GridManager.GRIDSIZE; column++)
                scoreOfGrid[row, column] = calculateScoreOfGrid(gridNumbers, row, column);
        }

        //最高の評価値のマスを探索する処理
        int maxScore = scoreOfGrid[0, 0];
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

        return maxScore;
    }

    //指定された方向のひっくり返せる個数を調べる関数
    public int checkScoreInDirection(int[,] gridNumbers, int row, int column, int vecRow, int vecColumn) {
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
