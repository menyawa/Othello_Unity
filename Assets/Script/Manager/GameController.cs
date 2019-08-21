using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public static int turnNumber = 0;
	public static bool playerIsBlack;
    public static bool playerIsPlaced = false;

    public static GridManager gridManager;
    public static UIManager uiManager;
    public static CPUManager cpuManager;
    public static Clear clear;
	
	// Use this for initialization
	void Awake () {
        initGameController();
        gridManager.initGridManager();

        //プレイヤーが後手の場合、まずコンピューターに打たせる
        //FIXME:ゲーム画面で選ばせた場合、Startのタイミングで打たせることができない
        //なので、今の所先手しか選べない(0で初期化しているのはそれが理由)
        playerIsBlack = true;
        if (turnNumber == 1)
		{
			playerIsBlack = false;
			cpuManager.cpuProcess();
		}	
	}

    void Start() {
       

    }

    // Update is called once per frame
    void Update () {
        //プレイヤーの処理
        //打つ、盤面の更新はstoneでやってくれるのでここで記述せずとも良い
        if (!gridManager._judgeCheckMate.checkmate) {
            if (!playerIsPlaced) {
                gridManager._judgeCheckMate.passCount = 0;
                if (GridManager.checkPassed(gridManager.gridStoneNumbers)) {
                    uiManager._log.plusLog("Player", true);
                    playerIsPlaced = true;
                    gridManager._judgeCheckMate.passCount++;
                }
            }

            gridManager._judgeCheckMate.checkmate = gridManager._judgeCheckMate.judgeCheckmate();


            //勝敗の処理を更新しているため、ここでも終わっていないか判断する必要がある
            //if (playerIsPlaced && !gridManager._judgeCheckMate.checkmate) {
            //    cpuManager.cpuProcess();
            //    playerIsPlaced = false;
            //}

            //CPUは一瞬で打ち終わるため、ログの描写は一回でOK
            uiManager._log.overWriteLogText();

            gridManager._judgeCheckMate.checkmate = gridManager._judgeCheckMate.judgeCheckmate();

            if (gridManager._judgeCheckMate.checkmate) {
                clear.clearProgress();
            }
        }
	}

    private void initGameController() {
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        cpuManager = new CPUManager();
        clear = gameObject.GetComponent<Clear>();

        DOTween.Init();
    }
}
