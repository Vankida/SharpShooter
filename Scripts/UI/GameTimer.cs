using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class GameTimer : MonoBehaviourPun
{

    public TextMeshProUGUI gameTimerText;
    public GameObject gameTimerTextObj;
    public float timeValue = 90f;
    public GameObject endGamePanel;

    PhotonView myPV;

    public GameObject playerStack;

    private void Start() {
        myPV = GetComponent<PhotonView>();

        if (myPV.IsMine)
        {
            gameTimerTextObj.SetActive(true);
        }
    }

    private void Update()
    {

        if (myPV.IsMine)
        {
            DisplayGameTime();
        }
        
    }

    void DisplayGameTime()
    {
        if (timeValue <= 1)
        {
            EndGame();
        }
        else
        {
            timeValue -= Time.deltaTime;
        }
        float minutes = Mathf.FloorToInt(timeValue / 60);
        float seconds = Mathf.FloorToInt(timeValue % 60);

        gameTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndGame()
    {
        endGamePanel.SetActive(true);

        playerStack.SetActive(false);
    }
}
