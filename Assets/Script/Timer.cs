using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text playerTimerText;
    [SerializeField] private Text computerTimerText;
    private float playerTimeForMinutes;
    private float computerTimeForMinutes;
    private float playerTimeForSecond;
    private float computerTimeForSecond;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTimeForMinutes = 10;
        computerTimeForMinutes = 10;
    }

    // Update is called once per frame
    void Update()
    {
        countTime();
    }

    private void countTime() {
        if (GameController.playerIsBlack) {
            if (playerTimeForSecond > 0) {
                playerTimeForSecond -= Time.deltaTime;
            } else {
                if (playerTimeForMinutes > 0) {
                    playerTimeForMinutes--;
                    playerTimeForSecond += 60;
                }
            }
        } else {
            if (computerTimeForSecond > 0) {
                playerTimeForSecond -= Time.deltaTime;
            } else {
                if (computerTimeForMinutes > 0) {
                    playerTimeForMinutes--;
                    playerTimeForSecond += 60;
                }
            }
        }
    }
}
