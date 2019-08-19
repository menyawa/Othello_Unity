using System;
using UnityEngine;

public class Stone : MonoBehaviour
{
	private GameObject placeStone = null; //既に置かれている石
	[SerializeField] private GameObject stone; //コマのデータ
	[SerializeField] private int row;
	[SerializeField] private int column;
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		//置いたという判定がない間盤面を更新し続けることで、盤面を最新に保てる
		//Instantiateし続けることになるけど、まあシカタナイネ
		//どうよこれ、天才的
		if(!GameController.isPlaced)
			updateStone();
	}
	
	//石を置く関数
	public void ClickGrid()
	{
		//置けない場合、もう置いた場合は無効
		if (!Process.canPutDown(GameController.grids, row, column)　|| GameController.isPlaced)
			return;
		
		string hand = Process.shapingNumber(row, column);
		GameController.grids = Process.nextGrid(GameController.grids, hand);
		
		//ログを送信
		ManagerLog.plusLog("Player:" + hand);
		GameController.isPlaced = true;
	}
	
	//石を更新する関数
	public void updateStone()
	{
		Vector3 gridPosition = this.gameObject.GetComponent<Transform>().position;
		Quaternion stoneQuaternion = stone.GetComponent<Transform>().rotation;
		if(placeStone != null)
		Destroy(placeStone);
		if (GameController.grids[row, column] == 1)
			placeStone = Instantiate(stone, new Vector3(gridPosition.x, gridPosition.y + (float)0.1, gridPosition.z), stoneQuaternion);
		else if(GameController.grids[row, column] == 2)
			placeStone =  Instantiate(stone, new Vector3(gridPosition.x, gridPosition.y + (float)0.1, gridPosition.z), Quaternion.Euler(stoneQuaternion.x, stoneQuaternion.y, stoneQuaternion.z + 180));
		
		//マスの子として登録することで、散逸を防ぐ
		if(placeStone != null)
		placeStone.transform.parent = this.gameObject.transform;
	}
}
