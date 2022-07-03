using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

    public TextMeshProUGUI usernameInput;
    public TextMeshProUGUI buttonText;



    public void OnClickConnect()
    {
        if(usernameInput.text.Length > 1 && usernameInput.text.Length < 10)
        {
            PhotonNetwork.NickName = usernameInput.text;
            buttonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }
}
