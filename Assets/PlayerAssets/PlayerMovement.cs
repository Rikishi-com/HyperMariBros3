using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float groundDrag;

    public float JumpForce;
    public float JumpCoolDown;
    public float airMultiplier;
    bool ReadyToJump;

    public float playerheight;
    public LayerMask Ground;
    bool grounded;

    public Transform orientation;

    float HorizontalInput;
    float VerticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // 追加部分（Statusから移植）
    private float baseSpeed;
    private bool isSpeedBoosted = false;
    private float boostDuration = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ReadyToJump = true;
        baseSpeed = moveSpeed;  // 基本速度を保存
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerheight * 0.5f + 0.2f, Ground);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        ProcessInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    private void ProcessInput()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && grounded && ReadyToJump)
        {
            ReadyToJump = false;

            Jump();

            Invoke(nameof(resetJump), JumpCoolDown);
        }
    }

    private void movePlayer()
    {
        moveDirection = orientation.forward * VerticalInput + orientation.right * HorizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private void resetJump()
    {
        ReadyToJump = true;
    }

    // --- ここから移植機能 ---

    public void SpeedBoost()
    {
        if (!isSpeedBoosted)
        {
            isSpeedBoosted = true;
            moveSpeed = baseSpeed * 1.5f; // 50%アップ
            Invoke(nameof(ResetSpeed), boostDuration);
        }
    }

    private void ResetSpeed()
    {
        moveSpeed = baseSpeed;
        isSpeedBoosted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedBoost"))
        {
            SpeedBoost();
            Destroy(other.gameObject);
        }
    }
}