using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameObject playerTimerObject;
    [SerializeField] private GameObject computerTimerObject;
    private Text playerTimerText;
    private Text computerTimerText;
    private float playerTimeForMinutes;
    private float computerTimeForMinutes;
    private float playerTimeForSecond;
    private float computerTimeForSecond;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTimerText = playerTimerObject.GetComponent<Text>();
        computerTimerText = computerTimerObject.GetComponent<Text>();
        playerTimeForMinutes = 10;
        computerTimeForMinutes = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.playerIsBlack)
        {
            if (playerTimeForSecond > 0)
                playerTimeForSecond -= Time.deltaTime;
            else
            {
                
            }
        }
    }
}
