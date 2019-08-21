using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeCheckMate
{
    public int passCount { set; get; }
    public bool checkmate;

    public JudgeCheckMate() {
        passCount = 0;
        checkmate = false;
    }

    //勝敗を判断する関数
    public bool judgeCheckmate() {
        //2回パスされていない場合、まだ打てるマスがあるということ
        if (passCount != 2) {
            return false;
        } 

        return true;
    }
}
