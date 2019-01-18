using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public static int turnNumber = 0;
	public static bool isPlaced = false;
	public static bool checkmate = false;
	public static int passCount;
	public const int GRIDSIZE = 8;
	public const int NUMBEROFSEARCHING = 1000;
	public static int[,] grids = new int[GRIDSIZE, GRIDSIZE];
	
	// Use this for initialization
	void Start () {
		//初期化
		for (int i = 0; i < GRIDSIZE; i++)
		{
			for (int j = 0; j < GRIDSIZE; j++)
				grids[i, j] = 0;
		}
		//-1しているのはインデックスが0から始まるため
		grids[GRIDSIZE / 2 - 1, GRIDSIZE / 2 - 1] = grids[GRIDSIZE / 2, GRIDSIZE / 2] = 2;
		grids[GRIDSIZE / 2 - 1, GRIDSIZE / 2] = grids[GRIDSIZE / 2, GRIDSIZE / 2 - 1] = 1;
			
		//プレイヤーが後手の場合、まずコンピューターに打たせる
		//FIXME:ゲーム画面で選ばせた場合、Startのタイミングで打たせることができない
		//なので、今の所先手しか選べない(0で初期化しているのはそれが理由)
		if (turnNumber == 1)
			cpuProcess();
	}
	
	// Update is called once per frame
	void Update () {
		//プレイヤーの処理
		//打つ、盤面の更新はstoneでやってくれるのでここで記述せずとも良い
		if (!isPlaced && !checkmate)
		{
			passCount = 0;
			if (Process.passed())
			{
				isPlaced = true;
				passCount++;
			}
		}
		
		//CPU側の処理
		if (isPlaced && !checkmate)
			cpuProcess();
		
		if(checkmate)
			checkmateGame();
	}
	
	//CPU側の処理手順を示した関数
	private void cpuProcess()
	{
		if (turnNumber == 0) turnNumber = 1;
		else turnNumber = 0;
			
		if (Process.passed())
		{
			passCount++;
			checkmate = Process.judgeCheckmate(grids, turnNumber);
		}
		else
		{
			string hand = Process.nextHand(grids);
			grids = Process.nextGrid(grids, hand);
		}
		
		if (turnNumber == 0) turnNumber = 1;
		else turnNumber = 0;

		isPlaced = false;
	}

	void checkmateGame()
	{
		int blackCount = 0;
		int WhiteCount = 0;
		for (int row = 0; row < GRIDSIZE; row++) {
			for (int column = 0; column < GRIDSIZE; column++)
			{
				//点数計算用に石の数をカウントしておく
				if (grids[row, column] == 1) blackCount++;
				if (grids[row, column] == 2) WhiteCount++;
			}
		}
			
		Debug.Log("チェックメイト");
		if (blackCount > WhiteCount)
		{
			Debug.Log("勝ち：黒");
			Debug.Log("ポイント：" + (blackCount - WhiteCount));
		}
		else if(blackCount < WhiteCount)
		{
			Debug.Log("勝ち：白");
			Debug.Log("ポイント：" + (WhiteCount - blackCount));
		}
		else
			Debug.Log("引き分け");
	}
}
