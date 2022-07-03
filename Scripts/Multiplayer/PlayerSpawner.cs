using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner instance;

    [HideInInspector]public GameObject localPlayer;


    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        int randomNumber = Random.Range(0, spawnPoints.Length - 1);
        //Transform spawnPoint = spawnPoints[randomNumber];
        Transform spawnPoint = spawnPoints[0];


        GameObject playerToSpawn;
        int randomCharacter = Random.Range(0, 2);
        playerToSpawn = playerPrefabs[randomCharacter];

        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.transform.position, Quaternion.identity);


        
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");
    }
}