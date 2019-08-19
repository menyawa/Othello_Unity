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
    public void cpuProcess() {
        if (GameController.turnNumber == 0) GameController.turnNumber = 1;
        else GameController.turnNumber = 0;

        if (Process.passed()) {
            GameController.uiManager._log.plusLog("CPU:パス");
            GameController.gridManager._judgeCheckMate.passCount++;
        } else {
            string hand = Process.nextHand();
            GameController.gridManager.grids = Process.nextGrid(hand);

            //ログを送信
            GameController.uiManager._log.plusLog("CPU:" + hand);
        }

        if (GameController.turnNumber == 0) GameController.turnNumber = 1;
        else GameController.turnNumber = 0;

        GameController.playerIsPlaced = false;
    }
}
