using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPun
{
    RaycastWeapon raycastWeapon;

    private CharacterController controller;
    public Camera mainCamera;
    public GameObject playerCamera;
    public GameObject vC;
    public GameObject postprocessingVolume;
    public float moveSpeed = 8f;
    private Vector3 mousePos;
    private Rigidbody rb;

    Vector3 direction;

    public float dashSpeed;
    public float dashTime;
    private bool canDash = true;
    public float dashCD;

    float playerOriginalY = 3f;
    public float hoverHeight = 0.4f;
    public float hoverSpeed = 4f;

    public GameObject spaceText;
    public GameObject dashCDTextObj;
    public TextMeshProUGUI dashCDText;
    public Image dashImage;
    private float timeRemaining = 0f;

    Color oc;
    Color c;
    

    bool isDead;

    private void Awake() {
        
        if (photonView.IsMine)
        {
            playerCamera.SetActive(true);
            vC.SetActive(true);
            postprocessingVolume.SetActive(true);
        }
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            raycastWeapon = GetComponentInChildren<RaycastWeapon>();
            controller = GetComponent<CharacterController>();

            oc = dashImage.color;
            c = dashImage.color;
            oc.a = dashImage.color.a;
            c.a = .08f;


            canDash = true;
            dashCDTextObj.SetActive(false);
            spaceText.SetActive(true);
            dashImage.color = oc;
        }
        
    }


    void Update()
    {
        if (photonView.IsMine)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                Move();
            }


            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            float hitDis = 0.0f;

            if (playerPlane.Raycast(ray, out hitDis))
            {
                Vector3 targetPoint = ray.GetPoint(hitDis);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                targetRotation.x = 0f;
                targetRotation.z = 0f;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1000f * Time.deltaTime);

                // Gun Rotation
                if ((new Vector2(targetPoint.x, targetPoint.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 8)
                {
                    raycastWeapon.Aim(targetPoint);
                }
            }

            // Player hovering animation
            var pos = transform.position;
            var newY = playerOriginalY + hoverHeight * Mathf.Sin(Time.time * hoverSpeed);
            transform.position = new Vector3(pos.x, newY, pos.z);



            // Dash
            if (Input.GetKeyDown("space") && canDash)
            {
                canDash = false;
                Dash();
                StartCoroutine(DashCoolDown());

                spaceText.SetActive(false);
                dashCDTextObj.SetActive(true);
                dashImage.color = c;

                timeRemaining = 5f;
            }
        }

        if (photonView.IsMine)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                dashCDText.text = ((int)timeRemaining).ToString();
            }
        }
    }

    [PunRPC]
    public void ResetDashCD()
    {
        timeRemaining = 0;
        canDash = true;
        dashCDTextObj.SetActive(false);
        spaceText.SetActive(true);
        dashImage.color = oc;
    }

    void Move()
    {
        controller.Move(direction * moveSpeed * Time.deltaTime);
    }

    void Dash()
    {
        controller.Move(direction * dashSpeed);
    }

    IEnumerator DashCoolDown()
    {
        canDash = false;
        yield return new WaitForSeconds(5);
        canDash = true;
        dashCDTextObj.SetActive(false);
        spaceText.SetActive(true);
        dashImage.color = oc;
    }
}
