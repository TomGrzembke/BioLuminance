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
    [SerializeField] float smoothing = 10;
    [SerializeField] ControlState controlState;
    #endregion

    #region private fields
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
        inputActions.Player.Sprint.performed += ctx => Sprint(true);
        inputActions.Player.Sprint.canceled += ctx => Sprint(false);

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = defaultSpeed;
    }

    void FixedUpdate()
    {
        HandleRotation();
        if (controlState != ControlState.playerControl) return;


        SetAgentPosition();
        Smoothing();
    }

    void Smoothing()
    {
        if (movement != Vector2.zero) return;
        SetAgentPosition(Vector3.Lerp(transform.position, gameObject.transform.localPosition, smoothing));
    }

    private void HandleRotation()
    {
        if (agent.velocity == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, agent.velocity.normalized);
        float step = 5 * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
    }

    void Movement(Vector2 direction)
    {
        movement = direction.normalized;
    }

    public void SetAgentPosition()
    {
        if (movement != Vector2.zero)
            SetAgentPosition(transform.position + new Vector3(movement.x + 0.0001f, movement.y, 0));
    }

    public void SetAgentPosition(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
    }

    void Sprint(bool condition)
    {
        if (condition)
        {
            speed = sprintSpeed;
            agent.speed = speed;
        }
        else if (!condition)
        {
            speed = defaultSpeed;
            agent.speed = speed;
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