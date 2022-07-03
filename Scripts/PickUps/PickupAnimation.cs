using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAnimation : MonoBehaviour
{
    public float rotationSpeed = 120f;

    float pickUpOriginalY = 3f;
    public float hoverHeight = 0.4f;
    public float hoverSpeed = 4f;
    
    void FixedUpdate()
    {
        // Pickup rotating animation
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Pickup hovering animation
        var pos = transform.position;
        var newY = pickUpOriginalY + hoverHeight * Mathf.Sin(Time.time * hoverSpeed);
        transform.position = new Vector3(pos.x, newY, pos.z);
    }
}
