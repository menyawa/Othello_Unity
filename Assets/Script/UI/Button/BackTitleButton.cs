using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackTitleButton : ButtonBase
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _button.onClick.AddListener(backTitle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void backTitle() {
        SceneManager.LoadScene("Title");
    }
}
