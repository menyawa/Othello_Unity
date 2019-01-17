using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
	[SerializeField] private int turnNumber;
	[SerializeField] private GameObject button;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//先手・後手を選択する関数
	public void selectTurnNumber()
	{
		GameController.turnNumber = turnNumber;
		Destroy(button);
	}
}
