using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public static int turnNumber = 0;
	public static bool playerIsBlack;
	public static bool isPlaced = false;
	public static bool checkmate = false;

    public static GridManager gridManager;
    public static UIManager uiManager;
    public static CPUManager cpuManager;
    public static Clear clear;
	
	// Use this for initialization
	void Awake () {
        initManagers();
        
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
	
	// Update is called once per frame
	void Update () {
		//プレイヤーの処理
		//打つ、盤面の更新はstoneでやってくれるのでここで記述せずとも良い
		if (!isPlaced && !checkmate)
		{
			gridManager._judgeCheckMate.passCount = 0;
			if (Process.passed())
			{
				uiManager._log.plusLog("Player:パス");
				isPlaced = true;
                gridManager._judgeCheckMate.passCount++;
			}
		}
		
		//CPU側の処理
		if (isPlaced && !checkmate)
			cpuProcess();
		
		if(checkmate)
			checkmateGame();
	}

    private void initManagers() {
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        cpuManager = GameObject.Find("CPUManager").GetComponent<CPUManager>();
        clear = gameObject.GetComponent<Clear>();
    }
}
