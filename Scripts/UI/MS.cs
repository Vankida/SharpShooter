using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class MS : MonoBehaviour
{
    public TextMeshProUGUI pingText;

    private void Update()
    {
        pingText.text = "ms " + PhotonNetwork.GetPing();
    }

    
}
