using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region serialized fields

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float deceleration = 10f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerInputActions playerControls;
    [SerializeField] SpriteRenderer spriteRenderer;

    #endregion

    #region private fields

    Vector2 moveDir = Vector2.zero;
    Vector2 currentVelocity = Vector2.zero;
    InputAction move;
    public bool isSprinting;

    #endregion

    void Awake()
    {
        playerControls = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        move = playerControls.Player.Move;

        playerControls.Player.Sprint.started += ctx =>
        {
            isSprinting = true;
            Debug.Log("Sprint Started");
        };
        playerControls.Player.Sprint.performed += ctx =>
        {
            isSprinting = true;
            Debug.Log("Sprint Performed");
        };
        playerControls.Player.Sprint.canceled += ctx =>
        {
            isSprinting = false;
            Debug.Log("Sprint Canceled");
        };

        move.Enable();
    }

    void OnDisable()
    {
        move.Disable();
    }

    void Update()
    {
        moveDir = move.ReadValue<Vector2>();
        //gameObject.transform.localScale = new Vector3(-1, 1, 1);

        if (moveDir.x > 0) // Moving right
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveDir.x < 0) // Moving left
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }


    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 targetVelocity = new Vector2(moveDir.x, moveDir.y).normalized * moveSpeed;

        if (moveDir == Vector2.zero) //no Input
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        else
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, deceleration * Time.fixedDeltaTime);

        rb.velocity = currentVelocity;
    }
    void Test()
    {
        print("test");
    }
}