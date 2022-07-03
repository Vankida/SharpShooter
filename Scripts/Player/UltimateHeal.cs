using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UltimateHeal : MonoBehaviourPun
{
    public bool canUlt = true;
    HP hp;

    public GameObject eText;
    public GameObject ultCDTextObj;
    public TextMeshProUGUI ultCDText;
    public Image ultImage;
    private float timeRemaining = 0f;

    Color oc;
    Color c;

    public ParticleSystem HealEffect;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            hp = gameObject.GetComponent<HP>();

            oc = ultImage.color;
            c = ultImage.color;
            oc.a = ultImage.color.a;
            c.a = .08f;

            timeRemaining = 0;
            ultCDTextObj.SetActive(false);
            eText.SetActive(true);
            ultImage.color = oc;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            // Heal the player to the max hp
            if (Input.GetButtonDown("Fire2") && canUlt || Input.GetKeyDown(KeyCode.E) && canUlt)
            {
                HealEffect.Play();

                canUlt = false;
                //hp.hp = hp.maxHP;
                photonView.RPC("Ultimate", RpcTarget.AllBuffered);

                StartCoroutine(UltCoolDown());

                eText.SetActive(false);
                ultCDTextObj.SetActive(true);
                ultImage.color = c;

                timeRemaining = 20f;
            }
        }

        if (photonView.IsMine)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                ultCDText.text = ((int)timeRemaining).ToString();
            }
        }
        
    }

    IEnumerator UltCoolDown()
    {
        canUlt = false;
        yield return new WaitForSeconds(20);
        canUlt = true;
        ultCDTextObj.SetActive(false);
        eText.SetActive(true);
        ultImage.color = oc;
    }

    [PunRPC]
    public void ResetHealCD()
    {
        canUlt = true;
        timeRemaining = 0;
        ultCDTextObj.SetActive(false);
        eText.SetActive(true);
        ultImage.color = oc;
    }
}
