using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNickName : MonoBehaviourPun
{
    public string nick;

    // Start is called before the first frame update
    void Start()
    {
        /*if (photonView.IsMine)
        {
            playerNickName = PhotonNetwork.LocalPlayer.NickName;
        }*/
        
    }
}
