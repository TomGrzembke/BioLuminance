using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    public enum ControlState
    {
        playerControl,
        gameControl
    }

    #region serialized fields
    [SerializeField] float speed;
    [SerializeField] float defaultSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float slowPercentage;
    [SerializeField] float multiplier;
    [SerializeField] bool isSprinting;
    [SerializeField] bool isSlowed;
    [SerializeField] ControlState controlState;
    #endregion

    #region private fields
    [SerializeField] Vector2 movement;
    PlayerInputActions inputActions;
    NavMeshAgent agent;
    #endregion

    void Awake()
    {
        inputActions = new();

        inputActions.Player.Move.performed += ctx => Movement(ctx.ReadValue<Vector2>());
        inputActions.Player.Move.canceled += ctx => Movement(ctx.ReadValue<Vector2>());
        inputActions.Player.Sprint.performed += ctx => Sprint();
        inputActions.Player.Sprint.canceled += ctx => Sprint();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = defaultSpeed;
    }

    void Update()
    {
        if (controlState == ControlState.playerControl)
            SetAgentPosition();
    }

    void OnDrawGizmosSelected()
    {
    }

    void Movement(Vector2 direction)
    {
        if (controlState != ControlState.playerControl) return;

        movement = new(direction.x, direction.y);
    }

    public void SetAgentPosition()
    {
        if (movement != Vector2.zero)
            agent.SetDestination(transform.position + new Vector3(movement.x + 0.0001f, movement.y, 0) * multiplier);
    }

    float NormalizeValue(float value)
    {
        var passValue = value switch
        {
            float n when n > 0 && n != 0 => 1,
            float n when n < 0 && n != 0 => -1,
            _ => 0
        };
        return passValue;
    }

    public void CheckForSlow()
    {
        if (!isSlowed)
        {
            if (!isSprinting)
                SetMoveSpeed(defaultSpeed);
            else
                SetMoveSpeed(sprintSpeed);
            return;
        }

        SetMoveSpeed(speed * slowPercentage);
    }

    void Sprint()
    {
        if (!isSprinting)
        {
            speed = sprintSpeed;
            agent.speed = speed;
            isSprinting = true;
            CheckForSlow();
        }
        else if (isSprinting)
        {
            speed = defaultSpeed;
            agent.speed = speed;
            isSprinting = false;
            CheckForSlow();
        }
    }

    #region Getters
    public float GetPlayerSpeed()
    {
        return speed;
    }
    #endregion

    #region Setters

    public void SetMoveSpeed(float newSpeed)
    {
        speed = newSpeed;
        agent.speed = speed;
    }

    public void SetIsSlowed(bool value)
    {
        isSlowed = value;
        CheckForSlow();
    }

    public void SetControlState(ControlState newControlState)
    {
        controlState = newControlState;
    }
    public void ReenableMovement()
    {
        controlState = ControlState.playerControl;
    }
    #endregion

    #region OnEnable/Disable
    public void OnEnable()
    {
        inputActions.Enable();
    }

    public void OnDisable()
    {
        inputActions.Disable();
    }
    #endregion
}