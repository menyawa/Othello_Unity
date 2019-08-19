using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectTurnNumberButton : ButtonBase
{
    [SerializeField] private int turnNumber;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _button.onClick.AddListener(selectTurnNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //先手・後手を選択する関数
    public void selectTurnNumber() {
        GameController.turnNumber = turnNumber;
        SceneManager.LoadScene("Game");
    }
}
