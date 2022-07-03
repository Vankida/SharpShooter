using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class HP : MonoBehaviourPun
{
    public float maxHP = 100;
    public float hp;

    public TextMeshProUGUI healthText;
    public Image healthBar;
    private float lerpSpeed;
    public float LS = 3f;

    public GameObject xDecal;

    public GameObject deathEffect;

    public bool isDead = false;
    
    
    Death death;

    DeathCanvas deathCanvas;

    PhotonView myPV;

    public PhotonView parentView;

    void Start()
    {
        hp = maxHP;

        death = gameObject.GetComponentInParent<Death>();

        deathCanvas = gameObject.GetComponentInParent<DeathCanvas>();

        myPV = GetComponent<PhotonView>();
    }

    void Update()
    {
        healthText.text = "HP: " + hp;
        HealthBarFiller();
        //ColorChanger();
        lerpSpeed = LS * Time.deltaTime;
    }

    [PunRPC]
    public void Heal(int healPoints)
    {
        if (hp + healPoints >= maxHP)
        {
            hp = maxHP;
        }
        else
        {
            hp += healPoints;
        }
    }

    [PunRPC]
    public void NewDamage(float damagePoints, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (photonView.IsMine && hp > 0)
        {
            if (hp - damagePoints <= 0f)
            {
                hp = 0f;
                this.GetComponent<PhotonView>().RPC("KillingEffect", RpcTarget.AllBuffered, hitPoint, hitDirection);

                this.GetComponent<PhotonView>().RPC("ResetHealCD", RpcTarget.AllBuffered);

                this.GetComponent<PhotonView>().RPC("ResetDashCD", RpcTarget.AllBuffered);
                
                //Dead();
                this.GetComponent<PhotonView>().RPC("Dead", RpcTarget.AllBuffered);
            }
            else
            {
                hp -= damagePoints;
            }
        }
    }

    public void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, hp/maxHP, lerpSpeed);
    }
    public void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (hp / maxHP));
        healthBar.color = healthColor;
    }


    [PunRPC]
    public void Dead()
    {
        if (myPV.IsMine)
        {
            deathCanvas.SetCanvasActive();
        }

        var pos = transform.position;
        xDecal.transform.position = new Vector3(pos.x, -7.400001f, pos.z);
        xDecal.SetActive(true);

        parentView.RPC("Die", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void KillingEffect(Vector3 hitPoint, Vector3 hitDirection)
    {
        Instantiate(deathEffect, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection));
    }

    [PunRPC]
    public void DeathEffect()
    {
        Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity);
    }

    [PunRPC]
    public void Respawn()
    {
        isDead = false;
        xDecal.SetActive(false);
        
        // reset hp
        hp = maxHP;
    }

    [PunRPC]
    public void Ultimate()
    {
        hp = maxHP;
    }
}
