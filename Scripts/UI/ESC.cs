using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESC : MonoBehaviour
{
    public GameObject esc;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && esc.activeSelf == false)
        {
            esc.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && esc.activeSelf == true)
        {
            esc.SetActive(false);
        }
    }
}
