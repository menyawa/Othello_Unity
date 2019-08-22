using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Grid : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int _row;
    [SerializeField] private int _column;
    public Vector3 _position;

    public int _placedStoneNumber;
    public GameObject _placedStoneObject;
    public Transform _placedStoneTransform;

    public bool _rotateComplete;
    public static Vector3 _rotateLength = new Vector3(0, 0, 180);
    public static float _rotateSecond = 0.5f;

    // Start is called before the first frame update
    void Start() {
        _rotateComplete = true;
        _placedStoneNumber = 0;
        _position = gameObject.transform.position;
        _position.y += 0.1f;
    }

    // Update is called once per frame
    void Update() {
        updateStones();
    }

    /// <summary>
    /// クリックされたマスに石を置く関数
    /// </summary>
    /// <param name="pointerEventData"></param>
	public void OnPointerClick(PointerEventData pointerEventData) {
        //左クリックじゃない場合ダメ
        if (pointerEventData.pointerId != -1)
            return;

        //置けない場合、もう置いた場合はダメ
        if (!GameController.gridManager._judgeCanPutDown.canPutDown(GameController.gridManager.gridStoneNumbers, _row, _column) || GameController.playerIsPlaced)
            return;

        //盤面に打つ
        GameController.gridManager.gridStoneNumbers = GameController.gridManager._nextGrid.nextGrid(GameController.gridManager.gridStoneNumbers, _row, _column);

        GameController.uiManager._point.countPoint(GameController.gridManager.gridStoneNumbers);
        GameController.uiManager._point.printPoint();
        GameController.uiManager._log.plusLog("じぶん", false, _row, _column);
        GameController.playerIsPlaced = true;
    }

    //シーンの盤面に置かれている石を更新する関数
    public void updateStones() {
        //グリッドのデータと同じ場合更新は必要ないので戻る
        if (_placedStoneNumber == GameController.gridManager.gridStoneNumbers[_row, _column])
            return;

        //回転処理が完了していなかったら戻る
        if (!_rotateComplete) return;

        _rotateComplete = false;
        //石が置かれてなかったところに石が置かれた場合、石を生成
        if (_placedStoneNumber == 0) {
            _placedStoneObject = Instantiate(GameController.gridManager._stonePrefab, _position, Quaternion.identity);
            //回す時用にRectTransfromを取得
            _placedStoneTransform = _placedStoneObject.transform;

            //デフォルトが黒が上なので、白の場合半回転させる
            if (GameController.gridManager.gridStoneNumbers[_row, _column] == 2) {
                //_placedStoneObject.transform.Rotate(_rotateLength);
                _placedStoneTransform.DORotate(_rotateLength, _rotateSecond);
            }
        } else {
            //もともと石が置かれていて石の更新があった場合、半回転させる
            //_placedStoneObject.transform.Rotate(_rotateLength);
            _placedStoneTransform.DORotate(_rotateLength, _rotateSecond);
        }

        //置かれている石が何色かを更新
        _placedStoneNumber = GameController.gridManager.gridStoneNumbers[_row, _column];
        _rotateComplete = true;
    }

    /// <summary>
    /// デバッグ用に、Consoleに数字で盤面状況を吐き出す関数
    /// </summary>
    public static void printDebugGrids() {
        for (int row = 0; row < GridManager.GRIDSIZE; row++) {
            string text = "";
            for (int column = 0; column < GridManager.GRIDSIZE; column++) {
                text += GameController.gridManager.gridStoneNumbers[row, column] + " ";
            }
        }
    }
}
