using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerLog : MonoBehaviour
{
	private static string log;
	//ログの差分
	private string oldLog;
	
	//ScrollViewのTextコンポーネント
	private ScrollRect scrollRect;
	private Text textLog;
    
	// Use this for initialization
	void Start ()
	{
		scrollRect = this.gameObject.GetComponent<ScrollRect>();
		textLog = scrollRect.content.GetComponentInChildren<Text>();
		//タイトル画面のログも拾ってきてしまうため初期化
		log = "";
		oldLog = "";
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (scrollRect != null && log != oldLog)
		{
			textLog.text += log;
			//Textが追加されたら5フレーム後にScrollViewの一番下へ
			StartCoroutine(DelayMethod(5, () =>
			{
				scrollRect.verticalNormalizedPosition = 0;
			}));
			oldLog = log;
		}
	}

	public static void plusLog(string text)
	{
		log += text + "\n";
	}
	
	// 指定したフレーム数後にActionが実行される
	private IEnumerator DelayMethod(int delayFrameCount, Action action)
	{
		for (var i = 0; i < delayFrameCount; i++)
		{
			yield return null;
		}
		action();
	}
}
