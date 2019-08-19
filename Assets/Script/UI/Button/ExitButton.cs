using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : ButtonBase
{
    // Start is called before the first frame update
    new void Start() {
        base.Start();
        _button.onClick.AddListener(exitGame);
    }

    // Update is called once per frame
    void Update() {

    }

    public void exitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
		    Application.OpenURL("http://www.yahoo.co.jp/");
        #else
		    Application.Quit();
        #endif
    }
}
