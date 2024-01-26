using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance;

    public float moveSpeed, gravityForce, jumpForce, sprintSpeed;
    public CharacterController characterController;

    private Vector3 moveInput;

    public Transform cameraTransform;

    public float mouseSensitivity;

    private bool canJump;
    public Transform groundCheckPoint;
    public LayerMask ground;

    public Animator animator;

    public Transform firePoint;

    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();
    public int currentGun;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);

        UI.instance.ammunitionText.text = "" + activeGun.currentAmmunition;
    }

    // Update is called once per frame
    void Update()
    {
        float yVelocity = moveInput.y;

        Vector3 verticalMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontalMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = horizontalMove + verticalMove;
        moveInput.Normalize();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveInput = moveInput * sprintSpeed;
        }
        else
        {
            moveInput = moveInput * moveSpeed;
        }

        moveInput.y = yVelocity;

        moveInput.y += Physics.gravity.y * gravityForce * Time.deltaTime;

        if (characterController.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityForce * Time.deltaTime;
        }

        canJump = Physics.OverlapSphere(groundCheckPoint.position, 0.2f, ground).Length > 0;

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            moveInput.y = jumpForce;
        }

        characterController.Move(moveInput * Time.deltaTime);


        //camera rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        //Shooting
        if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)
        {
            RaycastHit hit;
            if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 200f))
            {
                if(Vector3.Distance(cameraTransform.position, hit.point) > 1f)
                {
                    firePoint.LookAt(hit.point);
                } else
                {
                    firePoint.LookAt(cameraTransform.position + (cameraTransform.forward * 40f));
                }
            }

            //Instantiate(bullet, firePoint.position, firePoint.rotation);
            FireShot();
        }

        if (Input.GetMouseButton(0) && activeGun.canAutoFire)
        {
            if(activeGun.fireCounter <= 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 200f))
                {
                    if (Vector3.Distance(cameraTransform.position, hit.point) > 1f)
                    {
                        firePoint.LookAt(hit.point);
                    }
                    else
                    {
                        firePoint.LookAt(cameraTransform.position + (cameraTransform.forward * 40f));
                    }
                }

                FireShot();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchGun();
        }

        animator.SetFloat("moveSpeed", moveInput.magnitude);
        animator.SetBool("onGround", canJump);
    }

    public void FireShot()
    {
        if(activeGun.currentAmmunition > 0)
        {
            activeGun.currentAmmunition--;

            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);

            activeGun.fireCounter = activeGun.fireRate;

            UI.instance.ammunitionText.text = "" + activeGun.currentAmmunition;
        }       
    }

    public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);

        currentGun++;

        if(currentGun >= allGuns.Count)
        {
            currentGun = 0;
        }

        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);

        UI.instance.ammunitionText.text = "" + activeGun.currentAmmunition;
    }
}
