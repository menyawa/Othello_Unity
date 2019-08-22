using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Clear : MonoBehaviour
{
    public RectTransform[] clearMenuUIsRectTrans;

    [SerializeField] private RectTransform winTextRectTrans;
    [SerializeField] private RectTransform loseTextRectTrans;
    [SerializeField] private RectTransform drawTextRectTrans;

    [SerializeField] private Text pointText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clearProgress()
    {
        RectTransform winnerTextRectTrans = judgeWinner();
        int winnerPoint = calculateWinnerPoint();
        pointText.text = "Point:" + winnerPoint.ToString();

        //上から順番にクリア時のUIを出していく
        //勝敗のテキストはどれになるかがわからないため、ここで最初に代入する
        clearMenuUIsRectTrans[1] = winnerTextRectTrans;
        Sequence clearMenuSeq = DOTween.Sequence();
        float waitSecond = 1.0f;
        for (int index = 0; index < clearMenuUIsRectTrans.Length; index++) {
            clearMenuSeq.Append(clearMenuUIsRectTrans[index].DOScale(UIManager.DEFAULTSCALE, waitSecond).SetEase(Ease.InOutElastic));
        }
    }

    public RectTransform judgeWinner() {
        RectTransform winnerTextRectTrans = null;

        //念の為、ポイントを最新にしておく
        GameController.uiManager._point.countPoint(GameController.gridManager.gridStoneNumbers);

        if (GameController.uiManager._point.blackPoint > GameController.uiManager._point.whitePoint) {
            winnerTextRectTrans = GameController.playerIsBlack ? winTextRectTrans : loseTextRectTrans;
        } else if (GameController.uiManager._point.whitePoint > GameController.uiManager._point.blackPoint) {
            winnerTextRectTrans = GameController.playerIsBlack ? loseTextRectTrans : winTextRectTrans;
        } else {
            winnerTextRectTrans = drawTextRectTrans;
        }

        return winnerTextRectTrans;
    }

    /// <summary>
    /// 勝った方のポイントを計算する関数
    /// </summary>
    /// <returns></returns>
    public int calculateWinnerPoint() {
        int blackWinnerPoint = GameController.uiManager._point.blackPoint - GameController.uiManager._point.whitePoint;
        int whiteWinnerPoint = GameController.uiManager._point.whitePoint - GameController.uiManager._point.blackPoint;

        //引き分けの場合、どちらも0になるため専用の条件分岐は考えなくても良い
        int point = GameController.uiManager._point.blackPoint > GameController.uiManager._point.whitePoint ? blackWinnerPoint : whiteWinnerPoint;

        return point;
    }
}
