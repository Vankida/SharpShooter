using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    HP hP;
    RaycastWeapon raycastWeapon;

    Vector3 mPrevPos;

    void Start()
    {
        hP = gameObject.GetComponent<HP>();
        raycastWeapon = GetComponentInChildren<RaycastWeapon>();

        mPrevPos = transform.position;
    }

    void FixedUpdate()
    {
        // Check collision with pick ups
        RaycastHit[] hits = Physics.SphereCastAll(mPrevPos, 2f, (transform.position - mPrevPos).normalized, (transform.position - mPrevPos).magnitude);
		mPrevPos = transform.position;

		for (int i = 0; i < hits.Length; i++){
			if (hits[i].collider.tag == "HealthPickUp")
            {
                hits[i].collider.GetComponent<PickupRespawn>().PickedUp();
                hP.Heal(40);
			}
            else if (hits[i].collider.tag == "DamagePickUp")
            {
                hits[i].collider.GetComponent<PickupRespawn>().PickedUp();
                raycastWeapon.IncreaseDamage(4);
            }
            else if (hits[i].collider.tag == "ShieldPickUp")
            {
                hits[i].collider.GetComponent<PickupRespawn>().PickedUp();
            }
		}
    }
}
