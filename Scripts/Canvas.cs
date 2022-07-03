using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Canvas : MonoBehaviourPun
{

    PhotonView myPV;

    public GameObject canvas;


    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();

        if (myPV.IsMine)
        {
            canvas.SetActive(true);
        }
    }
}
