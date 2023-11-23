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
    [SerializeField] SpeedSubject speedSubject;
    [SerializeField] float sprintSpeed;
    [SerializeField] float smoothing = 10;
    [SerializeField] ControlState controlState;
    [SerializeField] bool isPerformingMove;
    [SerializeField] float timeUntilMaximumSpeed = 1;
    [SerializeField] float timeUntilZeroSpeed = 1;
    #endregion

    float currentAgentSpeed;

    #region private fields
    Vector2 movement;
    Vector2 moveSafe;
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
        agent.speed = speedSubject.Speed;

        currentAgentSpeed = speedSubject.Speed;
    }

    void FixedUpdate()
    {
        HandleRotation();
        if (controlState != ControlState.playerControl) return;

        Smoothing();
        SetAgentPosition();
    }

    void Smoothing()
    {
        if (isPerformingMove)
            currentAgentSpeed += SpeedRamp();
        else
            currentAgentSpeed -= SpeedRamp();

        currentAgentSpeed = Mathf.Clamp(currentAgentSpeed, 0, speedSubject.Speed);
        agent.speed = currentAgentSpeed;
    }

    float SpeedRamp()
    {
        return Time.deltaTime * speedSubject.Speed / timeUntilMaximumSpeed;
    }

    void HandleRotation()
    {
        if (agent.velocity == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, agent.velocity.normalized);
        float step = smoothing * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
    }

    void Movement(Vector2 direction)
    {
        isPerformingMove = direction != Vector2.zero;

        if (direction != Vector2.zero)
            movement = direction.normalized;
    }

    public void SetAgentPosition()
    {
        if (movement == Vector2.zero) return;

        //moveSafe += Time.deltaTime * movement * 3;
        //moveSafe.x = Mathf.Clamp(moveSafe.x, -1, 1);
        //moveSafe.y = Mathf.Clamp(moveSafe.y, -1, 1);
        moveSafe = Vector2.Lerp(moveSafe, movement, Time.deltaTime * smoothing);
        SetAgentPosition(transform.position + new Vector3(moveSafe.x + 0.0001f, moveSafe.y, 0));
    }

    public void SetAgentPosition(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
    }

    void Sprint(bool condition)
    {
        if (condition)
            speedSubject.AddSpeedModifier(sprintSpeed);
        else
            speedSubject.RemoveSpeedModifier(sprintSpeed);
    }
    void OnDrawGizmosSelected()
    {
    }


    #region Setters
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