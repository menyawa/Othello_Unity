using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : ButtonBase
{
    [SerializeField] private GameObject turnNumberPanel;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        _button.onClick.AddListener(continueButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void continueButton() {
        turnNumberPanel.SetActive(true);
    }
}
