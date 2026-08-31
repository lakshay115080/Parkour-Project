using System;
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
    public bool canJump;
    bool isJumping;
    bool isGrounded;

    [Header("Crouch Settings")]
    [SerializeField] float standingHeight;
    [SerializeField] float crouchingHeight;
    CapsuleCollider capsule;
    bool isCrouching;

    [Header("Slide Settings")]
    [SerializeField] float slideValue;
    [SerializeField] float slideDuration;
    float slideTimer;
    bool isSliding;

    public event Action OnSlide;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        canJump = true;
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
        HandleSlide();
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
        if (isSliding) return;
        Vector3 moveDirection = transform.forward * moveValue.y + transform.right * moveValue.x;
        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveDirection.x * playerSpeed;
        velocity.z = moveDirection.z * playerSpeed;

        rb.linearVelocity = velocity;
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
        if (isCrouching && !isJumping)
        {
            playerSpeed = crouchSpeed;
        }
        else if (isSprinting && isGrounded)
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
        if (isGrounded && isJumping && canJump)
        {
            rb.AddForce(Vector3.up * jumpValue, ForceMode.Impulse);
        }
    }

    private void OnCrouch(InputValue value)
    {
        isCrouching = value.isPressed;
        if (isSprinting && isCrouching && !isSliding && isGrounded)
        {
            isSliding = true;
            slideTimer = 0;
            rb.AddForce(transform.forward * slideValue, ForceMode.Impulse);
            OnSlide?.Invoke();
        }
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

    private void HandleSlide()
    {
        if (isSliding)
        {
            slideTimer += Time.deltaTime;
            if (slideTimer >= slideDuration || !isCrouching)
            {
                slideTimer = 0;
                isSliding = false;

                rb.linearVelocity = Vector3.zero;
            }
        }
    }
}
