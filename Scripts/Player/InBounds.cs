using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InBounds : MonoBehaviourPun
{
    private float groundCheckTimer = 1f;

    public float distanceToGround = 4f;

    Ray ray;
    RaycastHit hitInfo;

    HP hp;

    bool dead = false;

    private void Start()
    {
        hp = gameObject.GetComponent<HP>();
    }
    

    private void Update() {
        
        if (photonView.IsMine)
        {
            if (groundCheckTimer > 0)
            {
                groundCheckTimer -= Time.deltaTime;
            }
            else
            {
                if (hp.hp > 0)
                {
                    IsInBounds();
                    dead = false;
                }
            }
        }

        
    }

    private void IsInBounds()
    {
        groundCheckTimer = 1f;

        ray.origin = transform.position;
        ray.direction = -Vector3.up;

        PhotonView mine = gameObject.GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            if (!Physics.Raycast(ray, out hitInfo, distanceToGround) && dead == false)
            {
                hp.hp -= 10f;
                if (hp.hp <= 0f)
                {
                    mine.RPC("DeathEffect", RpcTarget.AllBuffered);
                    mine.RPC("Dead", RpcTarget.AllBuffered);
                    dead = true;
                }
            }
        }
        
    }
}
