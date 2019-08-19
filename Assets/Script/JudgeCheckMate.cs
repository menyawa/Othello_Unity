using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeCheckMate : MonoBehaviour
{
    public int passCount { set; get; }
    public bool checkmate;

    // Start is called before the first frame update
    void Start()
    {
        passCount = 0;
        checkmate = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
