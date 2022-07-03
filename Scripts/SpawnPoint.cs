using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool isBeingUsed = false;

    private void OnTriggerEnter(Collider other) {
        isBeingUsed = true;
    }
    private void OnTriggerExit(Collider other) {
        isBeingUsed = false;
    }
}
