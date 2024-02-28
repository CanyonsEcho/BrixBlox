using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 3f;
    public float verticalRotationLimit = 22.5f;
    public float jumpForce = 10f;

    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    private Rigidbody rb;
    private Camera activeCamera;
    private bool isGrounded;
    private float verticalRotation = 0f;
    private bool isWalking = false;
    private Animator anim;
    private bool hasJumped = false;

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

        if (movement.magnitude > 0f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            
            transform.Rotate(Vector3.up, mouseX);

            verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
            firstPersonCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }

        if (isGrounded && Input.GetButtonDown("Jump") && !hasJumped)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            hasJumped = true;
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
            hasJumped = false;
            AdjustRigidbodyForGrounded();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            AdjustRigidbodyForGrounded();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            AdjustRigidbodyForAirborne();
        }
    }

    private void AdjustRigidbodyForGrounded()
    {
        rb.drag = 5f;
        rb.angularDrag = 5f;
    }

    private void AdjustRigidbodyForAirborne()
    {
        rb.drag = 2f;
        rb.angularDrag = 0.05f;
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
        else if (!isGrounded && !isWalking)
        {
            anim.StopPlayback();
            anim.Play("Jumping");
        }
        else if (isGrounded && !isWalking)
        {
            anim.StopPlayback();
            anim.Play("Idle");
        }
    }
}
