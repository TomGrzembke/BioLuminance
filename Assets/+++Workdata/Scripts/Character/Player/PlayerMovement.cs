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
    [SerializeField] float defaultSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] ControlState controlState;
    #endregion

    #region private fields
    bool isSprinting;
    float speed;
    Vector2 movement;
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

    void FixedUpdate()
    {
        if (controlState == ControlState.playerControl)
            SetAgentPosition();
    }

    void Movement(Vector2 direction)
    {
        movement = direction.normalized;
    }

    public void SetAgentPosition()
    {
        agent.SetDestination(transform.position + new Vector3(movement.x + 0.0001f, movement.y, 0));
    }

    void Sprint()
    {
        if (!isSprinting)
        {
            speed = sprintSpeed;
            agent.speed = speed;
            isSprinting = true;
        }
        else if (isSprinting)
        {
            speed = defaultSpeed;
            agent.speed = speed;
            isSprinting = false;
        }
    }
    void OnDrawGizmosSelected()
    {
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