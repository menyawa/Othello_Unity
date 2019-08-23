using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    //ここで初期化することで、後手の場合のCPUのログ表示にも間に合わせる
    private string _log = "";
	
	[SerializeField] private ScrollRect _scrollRect;
	[SerializeField] private Text _text;


    // Update is called once per frame
    void Update ()
	{
	}

	public void plusLog(string playerText, bool passed, int row = -1, int column = -1)
	{
        string text = playerText + " ";
        //列番号などのログか、パスかを三項演算子で判断している
        text += passed ? "パス" : shapingNumber(row, column);
		_log += text + "\n";
	}

    public void printLog() {
        //クリア後はログを出さない
        if (GameController.gridManager._judgeCheckMate.checkmate)
            return;

        _text.text = _log;
        //テキストを更新したあと、一番下まで送る
        _scrollRect.verticalNormalizedPosition = 0;
    }

    //row、columnをログ用の文字列に整形してくれる関数
    private string shapingNumber(int row, int column) {
        string str = "";

        if (column == 0) str = "a";
        else if (column == 1) str = "b";
        else if (column == 2) str = "c";
        else if (column == 3) str = "d";
        else if (column == 4) str = "e";
        else if (column == 5) str = "f";
        else if (column == 6) str = "g";
        else if (column == 7) str = "h";

        str += row + 1; //インデックスが1から始まるため+1

        return str;
    }
}
