using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 3f;
    public float verticalRotationLimit = 80f;
    public float jumpForce = 10f;

    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    private Rigidbody rb;
    private Camera activeCamera;
    private bool isGrounded;
    private float verticalRotation = 0f;
    private bool isWalking = false;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        activeCamera = Camera.main;
        anim = GetComponent<Animator>();

        if (activeCamera == null)
        {
            Debug.LogError("no camera :sad:");
        }

        if (firstPersonCamera == null || thirdPersonCamera == null)
        {
            Debug.LogError("silly goober, assign the cameras");
        }

        SetActiveCamera(firstPersonCamera);
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement = transform.TransformDirection(movement);
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
        
        isWalking = movement.magnitude > 0f;

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            Vector3 newRotation = transform.rotation.eulerAngles + new Vector3(0f, mouseX, 0f);
            rb.MoveRotation(Quaternion.Euler(newRotation));

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

            if (activeCamera != null)
            {
                activeCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            }
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            ToggleCamera();
        }

        UpdateAnimation();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void SetActiveCamera(Camera newActiveCamera)
    {
        if (activeCamera != null)
        {
            activeCamera.enabled = false;
        }

        newActiveCamera.enabled = true;
        activeCamera = newActiveCamera;
    }

    private void ToggleCamera()
    {
        if (activeCamera == firstPersonCamera)
        {
            SetActiveCamera(thirdPersonCamera);
        }
        else
        {
            SetActiveCamera(firstPersonCamera);
        }
    }

    private void UpdateAnimation()
    {
        if (isWalking)
        {
            anim.Play("Walking");
        }
        else if (!isGrounded && isWalking)
        {
            anim.Play("Walking");
        }
        else if (!isGrounded && !isWalking)
        {
            anim.Play("Jumping");
        }
        else if (isGrounded)
        {
            anim.Play("Idle");
        }
    }
}
