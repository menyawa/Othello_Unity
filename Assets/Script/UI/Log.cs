using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    private string log;
	
	[SerializeField] private ScrollRect _scrollRect;
	[SerializeField] private Text _text;
    
	// Use this for initialization
	void Start ()
	{
		//タイトル画面のログも拾ってきてしまうため初期化
		log = "";
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void plusLog(string text)
	{
		log += text + "\n";
	}

    public void overWriteLogText() {
        _text.text = log;
        //テキストを更新したあと、一番下まで送る
        _scrollRect.verticalNormalizedPosition = 0;
    }
}
