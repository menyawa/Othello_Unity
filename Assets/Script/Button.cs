using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
	[SerializeField] private GameObject turnNumberPanel;
	[SerializeField] private int turnNumber;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startButton()
	{
		turnNumberPanel.SetActive(true);
		Destroy(this.gameObject);
	}
	
	//先手・後手を選択する関数
	public void selectTurnNumber()
	{
		GameController.turnNumber = turnNumber;
		//DontDestroyOnLoad(GameObject.Find("GameController"));
		SceneManager.LoadScene("Game");
	}
}
