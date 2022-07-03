using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RaycastWeapon : MonoBehaviourPun
{
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer bulletTrail;
    public GameObject raycastOrigin;
    public GameObject rightArm;

    Ray ray;
    RaycastHit hitInfo;

    Ray rayLeft;
    RaycastHit hitInfoLeft;
    public ParticleSystem muzzleFlashLeft;
    public GameObject raycastOriginLeft;
    public GameObject leftArm;


    [SerializeField]
    private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    private LayerMask Mask;
    [SerializeField]
    private float shootRange = 40f;
    Vector3 direction;

    public float fireRate = .1f;
    float timer;
    Vector3 recoilSmoothDampVelocity;
	Vector3 originalGunPosition;

    public bool usingUltimate = false;

    public Animator animator;

    // Damage
    public float originalDamage = 8f;
    public float damage;

    // Reloading
    public float ammo;
    public float maxAmmo;
    public TextMeshProUGUI ammoDisplay;
    public TextMeshProUGUI reloadingText;
    bool isFiring = false;
    public float reloadTime;
    bool isReloading;
    public bool disableShoot = false;

    bool dmgPickupActive = false;

    AudioSource shotSFX;

    public TextMeshProUGUI dmgText;

    PhotonView myPV;

    private void Start()
    {
        myPV = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            // Save the gun's original transform to move it back to it after the knock back for the recoil
            originalGunPosition = transform.localPosition;

            damage = originalDamage;

            shotSFX = gameObject.GetComponent<AudioSource>();
        }
        
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            timer += Time.deltaTime;

            ammoDisplay.text = ammo.ToString() + " / " + maxAmmo.ToString();
        }
        
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.R) && !isReloading)
            {
                animator.SetBool("Reloading", true);
                isReloading = true;
                StartCoroutine(Reload());
            }
            else if (ammo <= 0 && !isReloading)
            {
                animator.SetBool("Reloading", true);
                isReloading = true;
                StartCoroutine(Reload());
            }

            // Raycast Shooting
            if (Input.GetButton("Fire1") && timer >= fireRate && !isFiring && ammo > 0 && !isReloading)
            {
                shotSFX.Play();
                ammo--;
                //StartFiring();
                photonView.RPC("StartFiring", RpcTarget.AllBuffered);
                CameraShake.Instance.ShakeCamera();
                // Gunshot Knockback
                transform.localPosition += transform.parent.InverseTransformDirection(-transform.forward) * 0.002f;
                timer = 0f;
            }
            // Animate the recoil
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, originalGunPosition, ref recoilSmoothDampVelocity, 0.1f);


            // Update dmg text
            dmgText.text = "DMG: " + damage;
        }
        
    }

    [PunRPC]
    public void StartFiring(){
            muzzleFlash.Play();
            ray.origin = raycastOrigin.transform.position;

            direction = GetDirection(rightArm.transform.forward);
            ray.direction = direction;

            var tracer = Instantiate(bulletTrail, ray.origin, Quaternion.identity);
            tracer.AddPosition(ray.origin);

            if (Physics.Raycast(ray, out hitInfo, shootRange, Mask)){
                tracer.transform.position = hitInfo.point;

                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);

                PhotonView target = hitInfo.collider.gameObject.GetComponent<PhotonView>();

                //Damage
                if (target.tag == "Player" && !target.IsMine)
                {
                    target.RPC("NewDamage", RpcTarget.AllBuffered, damage, hitInfo.point, ray.direction);
                }
            }
            else
            {
                tracer.transform.position = raycastOrigin.transform.position + ray.direction * shootRange;
            }
        
    }

    private Vector3 GetDirection(Vector3 direction)
    {
        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }

    public void Aim(Vector3 aimPoint)
    {
		rightArm.transform.LookAt(aimPoint);
		leftArm.transform.LookAt(aimPoint);

		// Lock the X-axis rotation
		rightArm.transform.localEulerAngles = new Vector3(0, rightArm.transform.localEulerAngles.y, rightArm.transform.localEulerAngles.z);
		leftArm.transform.localEulerAngles = new Vector3(0, leftArm.transform.localEulerAngles.y, leftArm.transform.localEulerAngles.z);
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;

        if (dmgPickupActive)
        {
            damage -= 4;
            dmgPickupActive = false;
        }

        isReloading = false;
        reloadingText.gameObject.SetActive(false);
        animator.SetBool("Reloading", false);
    }

    public void IncreaseDamage(float damagePoints)
    {
        if (!dmgPickupActive)
        {
            damage += damagePoints;
            ammo = maxAmmo;
            dmgPickupActive = true;
        }
    }

    public void FlameDamage(float dmgPoints)
    {
        damage += dmgPoints;
    }
    public void ResetFlame(float dmgPoints)
    {
        damage -= dmgPoints;
    }
}