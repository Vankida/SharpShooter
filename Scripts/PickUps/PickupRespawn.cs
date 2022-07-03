using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRespawn : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Collider colliderr;

    private float respawnTime = 4f;

    Color c;


    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        colliderr = GetComponent<Collider>();

        c = meshRenderer.material.color;
    }

    public void PickedUp()
    {
        c.a = 0.1f;
        meshRenderer.material.color = c;
        colliderr.enabled = false;

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        c.a = 1.0f;
        meshRenderer.material.color = c;
        colliderr.enabled = true;
    }
}
