using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    private static string log;
	//ログの差分
	private string oldLog;
	
	[SerializeField] private ScrollRect _scrollRect;
	[SerializeField] private Text _text;
    
	// Use this for initialization
	void Start ()
	{
		//タイトル画面のログも拾ってきてしまうため初期化
		log = "";
		oldLog = "";
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (log != oldLog)
		{
			_text.text += log;
            //テキストを更新したあと、一番下まで送る
            _scrollRect.verticalNormalizedPosition = 0;

            //oldLogを更新
            oldLog = log;
		}
	}

	public void plusLog(string text)
	{
		log += text + "\n";
	}
}
