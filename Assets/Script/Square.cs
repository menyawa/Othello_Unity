﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Square : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int _row;
    [SerializeField] private int _column;
    public Vector3 _position;

    public int _placedStoneNumber;
    public GameObject _placedStoneObject;

    void Awake() {

    }

    // Start is called before the first frame update
    void Start() {
        _placedStoneNumber = 0;
        _position = gameObject.transform.position;
        _position.y += 0.1f;
    }

    // Update is called once per frame
    void Update() {
        updateStones();
    }

    /// <summary>
    /// 石を置く関数
    /// </summary>
    /// <param name="pointerEventData"></param>
	public void OnPointerClick(PointerEventData pointerEventData) {
        //左クリックじゃない場合ダメ
        if (pointerEventData.pointerId != -1)
            return;

        //置けない場合、もう置いた場合はダメ
        if (!GameController.gridManager._judgeCanPutDown.canPutDown(GameController.gridManager.grids, _row, _column) || GameController.playerIsPlaced)
            return;

        //盤面に打つ
        int stoneNumber = GameController.turnNumber + 1;
        GameController.gridManager.grids[_row, _column].nowStone = stoneNumber;

        //ログを送信
        string hand = Process.shapingNumber(_row, _column);
        GameController.uiManager._log.plusLog("Player:" + hand);

        updateStones();
        GameController.playerIsPlaced = true;
    }

    //盤面の置かれている石を更新する関数
    public void updateStones() {
        //グリッドのデータと同じ場合更新は必要ないので戻る
        if (_placedStoneNumber == GameController.gridManager.grids[_row, _column].nowStone)
            return;

        //石が置かれてなかったところに石が置かれた場合、石を生成
        if (_placedStoneNumber == 0) {
            _placedStoneObject = Instantiate(GameController.gridManager._stonePrefab, _position, Quaternion.identity);

            //デフォルトが黒が上なので、白の場合半回転させる
            if (GameController.gridManager.grids[_row, _column].nowStone == 2) {
                _placedStoneObject.transform.Rotate(0, 0, 180);
            }
        } else {
            //もともと石が置かれていて石の更新があった場合、半回転させる
            _placedStoneObject.transform.Rotate(0, 180, 0);
        }

        //置かれている石が何色かを更新
        _placedStoneNumber = GameController.gridManager.grids[_row, _column].nowStone;
    }
}
