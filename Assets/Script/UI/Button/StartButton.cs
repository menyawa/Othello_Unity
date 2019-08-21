using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : ButtonBase
{
    [SerializeField] private GameObject _titleMenu;
    [SerializeField] private GameObject _turnNumberPanel;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        _button.onClick.AddListener(openSelectTurnNumberPanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openSelectTurnNumberPanel() {
        if(_titleMenu != null)
            _titleMenu.SetActive(false);
        _turnNumberPanel.SetActive(true);
    }
}
