using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    public int blackPoint;
    public int whitePoint;
    [SerializeField] private Text blackPointText;
    [SerializeField] private Text whitePointText;

    // Start is called before the first frame update
    void Start()
    {
        blackPoint = 2;
        whitePoint = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 黒、白のポイントを計算する
    /// </summary>
    public void countPoint(int[,] gridStoneNumbers) {
        int blackCount = 0;
        int whiteCount = 0;
        for (int row = 0; row < GridManager.GRIDSIZE; row++) {
            for (int column = 0; column < GridManager.GRIDSIZE; column++) {
                //点数計算用に石の数をカウントしておく
                if (gridStoneNumbers[row, column] == 1) blackCount++;
                if (gridStoneNumbers[row, column] == 2) whiteCount++;
            }
        }

        blackPoint = blackCount;
        whitePoint = whiteCount;
    }

    public void printPoint() {
        blackPointText.text = "Black" + " " + blackPoint.ToString();
        whitePointText.text = "White" + " " +  whitePoint.ToString();
    }
}
