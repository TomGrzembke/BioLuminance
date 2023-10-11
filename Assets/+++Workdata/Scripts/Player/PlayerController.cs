using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MyBox;

public class PlayerController : MonoBehaviour
{
    #region serialized fields

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float deceleration = 10f;
    [Space(10)]
    [SerializeField] bool isSprinting;
    [SerializeField, Tooltip("Multiplies Move Speed by Sprint Speed")] float sprintSpeed = 2f;
    [Space(10)]
    [SerializeField] bool isDashing;
    [SerializeField, Tooltip("Distance to Dash")] float dashPower = 20f;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashingCooldown = 1f;
    [Space(10)]
    [SerializeField] Rigidbody2D rb;

    #endregion

    #region private fields

    PlayerInputActions playerControls;
    Vector2 moveDir = Vector2.zero;
    Vector2 currentVelocity = Vector2.zero;
    InputAction move;

    bool canDash = true;
    #endregion

    void Awake()
    {
        playerControls = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        move = playerControls.Player.Move;

        playerControls.Player.Sprint.performed += ctx => isSprinting = true;
        playerControls.Player.Sprint.canceled += ctx => isSprinting = false;

        playerControls.Player.Dash.performed += ctx => StartCoroutine(Dash());

        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        if (isDashing) return;
        moveDir = move.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (isDashing) return;
        Movement();
    }

    private void Movement()
    {
        Vector2 targetVelocity = new Vector2(moveDir.x, moveDir.y).normalized * moveSpeed * (isSprinting ? sprintSpeed : 1);
        
        currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, deceleration * Time.fixedDeltaTime);

        if (currentVelocity.magnitude < 0.1f) currentVelocity = Vector2.zero;

        rb.velocity = currentVelocity;

        Flip();
    }

    private IEnumerator Dash()
    {
        if (!canDash || isDashing) yield break;

        canDash = false;
        isDashing = true;

        Vector2 dashDirection = moveDir;

        dashDirection.Normalize();

        rb.velocity = dashDirection * dashPower;

        yield return new WaitForSeconds(dashingTime);

        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;
    }

    private void Flip()
    {
        if (moveDir.x > 0) // Moving right
            gameObject.transform.localScale = new Vector3(1, 1, 1);

        else if (moveDir.x < 0) // Moving left
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
    }
}