using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCanvas : MonoBehaviour
{
    public GameObject deathCanvas;

    float deathTime = 8f;

    public TextMeshProUGUI deathTimerTxt;


    // Update is called once per frame
    void Update()
    {
        if (deathTime > 0)
        {
            deathTime -= Time.deltaTime;

            deathTimerTxt.text = ((int)deathTime).ToString();
        }
    }

    public void SetCanvasActive()
    {
        deathTime = 8f;
        deathCanvas.SetActive(true);
        StartCoroutine(SetCanvasInactive());
    }

    IEnumerator SetCanvasInactive()
    {
        yield return new WaitForSeconds(8);
        deathCanvas.SetActive(false);
    }
}
