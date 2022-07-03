using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Death : MonoBehaviourPun
{
    [SerializeField]
    private GameObject body;
    HP hp;

    [SerializeField]
    private MeshRenderer bodyMesh;
    [SerializeField]
    private MeshRenderer reloadMeshL;
    [SerializeField]
    private MeshRenderer reloadMeshR;
    [SerializeField]
    private GameObject floats;
    [SerializeField]
    private MeshRenderer gunMeshL;
    [SerializeField]
    private MeshRenderer gunMeshR;

    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private RaycastWeapon raycastWeapon;
    [SerializeField]
    private UltimateHeal ultimateHeal;
    public CapsuleCollider col;

    private GameObject playerSpawner;
    Spawner spawner;


    PhotonView myPV;

    Player[] allPlayers;
    int myNumberInRoom = 0;

    private void Awake()
    {
        hp = GetComponentInChildren<HP>();

        playerSpawner = GameObject.Find("SpawnPoints2");
        spawner = playerSpawner.GetComponent<Spawner>();

        myPV = GetComponent<PhotonView>();

        allPlayers = PhotonNetwork.PlayerList;
        foreach (Player p in allPlayers)
        {
            if (p != PhotonNetwork.LocalPlayer)
            {
                myNumberInRoom++;
            }

            if (p == PhotonNetwork.LocalPlayer)
            {
                break;
            }
        }

        if (myPV.IsMine)
        {
            body.transform.position = spawner.spawnPoints[myNumberInRoom].transform.position;
        }
        
    }

    [PunRPC]
    public void Die()
    {
        body.SetActive(false);
        
        StartCoroutine(Respawn());
        StartCoroutine(RespawnChangePos());
    }

    IEnumerator RespawnChangePos()
    {
        yield return new WaitForSeconds(7);
        body.transform.position = spawner.spawnPoints[myNumberInRoom].transform.position;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(8);
        body.SetActive(true);
        hp.Respawn();
    }
}
