using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour, IPointerClickHandler
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
    /// クリックされたマスに石を置く関数
    /// </summary>
    /// <param name="pointerEventData"></param>
	public void OnPointerClick(PointerEventData pointerEventData) {
        //左クリックじゃない場合ダメ
        if (pointerEventData.pointerId != -1)
            return;

        //Debug.Log("row:" + _row + " " + "column:" + _column);
        //Debug.Log(GameController.gridManager.gridStoneNumbers[_row, _column]);
        Debug.Log(GameController.gridManager._judgeCanPutDown.canPutDown(GameController.gridManager.gridStoneNumbers, _row, _column));
        Debug.Log(GameController.playerIsPlaced);

        Debug.Log("Before");
        printDebugGrids();

        //置けない場合、もう置いた場合はダメ
        if (!GameController.gridManager._judgeCanPutDown.canPutDown(GameController.gridManager.gridStoneNumbers, _row, _column) || GameController.playerIsPlaced)
            return;

        //盤面に打つ
        GameController.gridManager.gridStoneNumbers = GameController.gridManager._nextGrid.nextGrid(GameController.gridManager.gridStoneNumbers, _row, _column);

        GameController.uiManager._point.countPoint(GameController.gridManager.gridStoneNumbers);
        GameController.uiManager._point.printPoint();
        GameController.uiManager._log.plusLog("じぶん", false, _row, _column);
        GameController.playerIsPlaced = true;

        Debug.Log("After");
        printDebugGrids();
    }

    //シーンの盤面に置かれている石を更新する関数
    public void updateStones() {
        //グリッドのデータと同じ場合更新は必要ないので戻る
        if (_placedStoneNumber == GameController.gridManager.gridStoneNumbers[_row, _column])
            return;

        //石が置かれてなかったところに石が置かれた場合、石を生成
        if (_placedStoneNumber == 0) {
            _placedStoneObject = Instantiate(GameController.gridManager._stonePrefab, _position, Quaternion.identity);

            //デフォルトが黒が上なので、白の場合半回転させる
            if (GameController.gridManager.gridStoneNumbers[_row, _column] == 2) {
                _placedStoneObject.transform.Rotate(0, 0, 180);
            }
        } else {
            //もともと石が置かれていて石の更新があった場合、半回転させる
            _placedStoneObject.transform.Rotate(0, 0, 180);
        }

        //置かれている石が何色かを更新
        _placedStoneNumber = GameController.gridManager.gridStoneNumbers[_row, _column];
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
            Debug.Log(text);
        }
    }
}
