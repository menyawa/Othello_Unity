using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour
{
    public Button _button;
    public AudioClip _sound;
    public bool _isPressed;

    // Start is called before the first frame update
    protected void Start()
    {
        _button.onClick.AddListener(playSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSound() {
        if(_sound != null) {
            GameController.audioManager.playSound(_sound);
        }
    }
}
