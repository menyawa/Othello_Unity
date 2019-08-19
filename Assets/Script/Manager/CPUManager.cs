using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUManager : MonoBehaviour
{
    public const int NUMBEROFSEARCHING = 1000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //CPU側の処理手順を示した関数
    private void cpuProcess() {
        if (GameController.turnNumber == 0) GameController.turnNumber = 1;
        else GameController.turnNumber = 0;

        if (Process.passed()) {
            GameController.logManager.plusLog("CPU:パス");
            GameController.judgeCheckMate.passCount++;
            checkmate = GameController.judgeCheckMate.judgeCheckmate();
        } else {
            string hand = Process.nextHand(hand);
            GameController.gridManager.grids = Process.nextGrid(hand);

            //ログを送信
            GameController.logManager.plusLog("CPU:" + hand);
        }

        if (GameController.turnNumber == 0) GameController.turnNumber = 1;
        else GameController.turnNumber = 0;

        GameController.isPlaced = false;
    }
}
