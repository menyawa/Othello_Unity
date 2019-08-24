using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static int turnNumber = 0;
    public static bool playerIsBlack;
    public static bool playerIsPlaced = false;

    public static GridManager gridManager;
    public static UIManager uiManager;
    public static CPUManager cpuManager;
    public static AudioManager audioManager;
    public static Clear clear;

    // Use this for initialization
    void Awake() {
        initGameController();

        if (SceneManager.GetActiveScene().name == "Title")
            return;

        gridManager.initGridManager();

        //プレイヤーが後手の場合、まずコンピューターに打たせる
        playerIsBlack = true;
        playerIsPlaced = false;
        if (turnNumber == 1) {
            playerIsBlack = false;
            cpuManager.cpuProcess();
        }
    }

    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (SceneManager.GetActiveScene().name == "Title")
            return;

        if (gridManager._judgeCheckMate.checkmate) {
            return;
        }

        if (!playerIsPlaced) {
            if (GridManager.checkPassed(gridManager.gridStoneNumbers)) {
                uiManager._log.plusLog("じぶん", true);
                playerIsPlaced = true;
                audioManager.playSound(gridManager._passSE);
                gridManager._judgeCheckMate.passCount++;
            }
        }

        gridManager._judgeCheckMate.checkmate = gridManager._judgeCheckMate.judgeCheckmate();

        //勝敗の処理を更新しているため、ここでも終わっていないか判断する必要がある
        if (playerIsPlaced && !gridManager._judgeCheckMate.checkmate) {
            cpuManager.cpuProcess();
        }

        gridManager._judgeCheckMate.checkmate = gridManager._judgeCheckMate.judgeCheckmate();

        if (gridManager._judgeCheckMate.checkmate) {
            clear.clearProgress();
        }
    }

    private void initGameController() {
        DOTween.Init();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        if (SceneManager.GetActiveScene().name == "Title")
            return;

        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        cpuManager = new CPUManager();
        clear = gameObject.GetComponent<Clear>();

        
    }
}
