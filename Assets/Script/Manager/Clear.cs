using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Clear : MonoBehaviour
{
    [SerializeField] private GameObject clearMenu;
    [SerializeField] private GameObject playerWinObject;
    [SerializeField] private GameObject computerWinObject;
    [SerializeField] private GameObject DrawObject;

    [SerializeField] private GameObject point;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clearProgress(int blackCount, int whiteCount)
    {
        clearMenu.SetActive(true);
        Text pointText = point.GetComponent<Text>();
        pointText.text = "Point:";

        if (blackCount > whiteCount)
        { 
            if(GameController.playerIsBlack)
                playerWinObject.SetActive(true);
            else
                computerWinObject.SetActive(true);

            pointText.text += (blackCount - whiteCount).ToString();
        }
        else if(blackCount < whiteCount)
        {
            if(!GameController.playerIsBlack)
                playerWinObject.SetActive(true);
            else
                computerWinObject.SetActive(true);
            
            pointText.text += (whiteCount - blackCount).ToString();
        }
        else
        {
            pointText.text = "0";
            DrawObject.SetActive(true);
        }
            
    }

    
}
