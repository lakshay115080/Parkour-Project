using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [Header("Speed Settings")]
    [SerializeField] float crouchSpeed = 5f;
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float sprintSpeed = 15f;
    float playerSpeed;
    bool isSprinting;
    Vector2 moveValue;
    [Header("Mouse Settings")]
    [SerializeField] float mouseSensitivity = 1;
    Transform vCam;
    Vector2 lookValue;
    float pitch;
    [Header("Jump Settings")]
    [SerializeField] float jumpValue;
    [SerializeField] GameObject groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float checkRadius = 0.4f;
    bool isJumping;
    bool isGrounded;

    [Header("Crouch Settings")]
    [SerializeField] float standingHeight;
    [SerializeField] float crouchingHeight;
    CapsuleCollider capsule;
    bool isCrouching;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        vCam = Camera.main.transform;
        standingHeight = capsule.height;
    }

    void Update()
    {
        HandleSpeed();
        HandleLook();
        HandleJump();
        HandleCrouch();
    }

    void FixedUpdate()
    {
        HandleMove();
    }

    private void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    private void HandleMove()
    {
        Vector3 currentPos = rb.position;
        Vector3 moveDirection = transform.forward * moveValue.y + transform.right * moveValue.x;
        Vector3 newPos = currentPos + moveDirection * (playerSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    private void OnLook(InputValue value)
    {
        lookValue = value.Get<Vector2>();
    }

    private void HandleLook()
    {
        pitch -= lookValue.y;
        pitch = Mathf.Clamp(pitch, -90, 90);
        vCam.localRotation = Quaternion.Euler(pitch, 0, 0);
        transform.Rotate(0, lookValue.x * mouseSensitivity, 0);
    }

    private void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    private void HandleSpeed()
    {
        if (isCrouching)
        {
            playerSpeed = crouchSpeed;
        }
        else if (isSprinting)
        {
            playerSpeed = sprintSpeed;
        }
        else
        {
            playerSpeed = walkSpeed;
        }
    }

    private void OnJump(InputValue value)
    {
        isJumping = value.isPressed;
    }

    private void HandleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, checkRadius, groundLayer);
        if (isGrounded && isJumping)
        {
            rb.AddForce(Vector3.up * jumpValue, ForceMode.Impulse);
        }
    }

    private void OnCrouch(InputValue value)
    {
        isCrouching = value.isPressed;
    }

    private void HandleCrouch()
    {
        if (isCrouching)
        {
            capsule.height = crouchingHeight;
        }
        else
        {
            capsule.height = standingHeight;
        }
    }
}
