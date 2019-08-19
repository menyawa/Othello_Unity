using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int _row;
    [SerializeField] private int _column;
    public Vector3 _position;

    public int beforeStone;
    public int nowStone;
    public GameObject _placedStone;
    
    void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        GameController.gridManager.grids[_row, _column] = this;
        _position = gameObject.transform.position;
        _position.y += 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 石を置く関数
    /// </summary>
    /// <param name="pointerEventData"></param>
	public void OnPointerClick(PointerEventData pointerEventData) {
        //左クリックじゃない場合
        if (pointerEventData.pointerId != -1)
            return;

        //置けない場合、もう置いた場合は無効
        if (!GameController.gridManager._judgeCanPutDown.canPutDown(_row, _column) || GameController.playerIsPlaced)
            return;
        
        //盤面に打つ処理を書く

        //ログを送信
        string hand = Process.shapingNumber(_row, _column);
        GameController.uiManager._log.plusLog("Player:" + hand);
        
        updateGrid();
        GameController.playerIsPlaced = true;
    }

    //盤面を更新する関数
    public static void updateGrid() {
        for(int row = 0; row < GridManager.GRIDSIZE; row++) {
            for(int column = 0; column < GridManager.GRIDSIZE; column++) {
                //石が置かれてなかったところに石が置かれた場合、石を生成
                if(GameController.gridManager.grids[row, column].nowStone != 0 && GameController.gridManager.grids[row, column].beforeStone == 0) {
                    GameController.gridManager.grids[row, column]._placedStone = Instantiate(GameController.gridManager._stonePrefab, GameController.gridManager.grids[row, column]._position, Quaternion.identity);

                    //デフォルトが黒が上なので、白の場合半回転させる
                    if(GameController.gridManager.grids[row, column].nowStone == 2) {
                        GameController.gridManager.grids[row, column]._placedStone.transform.Rotate(0, 0, 180);
                    }
                } else {
                    //石の更新があった場合、半回転させる
                    if (GameController.gridManager.grids[row, column].nowStone != GameController.gridManager.grids[row, column].beforeStone) {
                        GameController.gridManager.grids[row, column]._placedStone.transform.Rotate(0, 180, 0);
                    }
                }
            }
        }
    }

    
}
