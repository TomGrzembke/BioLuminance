using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
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
    [SerializeField] bool isSprinting;
    [SerializeField] bool isSlowed;
    [SerializeField] ControlState controlState;
    [SerializeField] Transform currentWASDMovepoint;
    [SerializeField] Transform[] movePoints;

    #endregion

    #region private fields
    float movementX, movementY;
    Vector2 currentIdlePoint;
    PlayerInputActions inputActions;
    NavMeshAgent agent;
    Animator anim;

    #endregion

    void Awake()
    {
        inputActions = new();

        inputActions.Player.Move.performed += ctx => Movement(ctx.ReadValue<Vector2>());
        inputActions.Player.Move.canceled += ctx => Movement(ctx.ReadValue<Vector2>());
        inputActions.Player.Sprint.performed += ctx => Sprint();
        inputActions.Player.Sprint.canceled += ctx => Sprint();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = defaultSpeed;
        currentWASDMovepoint = transform;
    }

    void FixedUpdate()
    {
        if (agent.remainingDistance < 0.2f)
        {
            //anim.SetBool("isMoving", false);
        }

        if (controlState == ControlState.playerControl)
            SetAgentPosition();
    }

    void Movement(Vector2 direction)
    {
        if (controlState != ControlState.playerControl) return;

        movementX = NormalizeValue(direction.x);
        movementY = NormalizeValue(direction.y);

        if (!CalculateCurrentWasdPoint())
            return; //Returns if movementX and Y is 0

        //anim.SetBool("isMoving", true);
        //anim.SetFloat("moveDirX", movementX);
        //anim.SetFloat("moveDirY", movementY);
        currentIdlePoint = new Vector2(movementX, movementY);
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

    /// <returns> The return determines if movement or Idle would be played in the movement </returns>
    bool CalculateCurrentWasdPoint()
    {
        Vector2 movementVector = new(movementX, movementY);

        switch (movementVector)
        {
            case Vector2 v when v.Equals(new(0, 0)):
                currentWASDMovepoint = transform;
                anim.SetBool("isMoving", false);
                return false;
            case Vector2 v when v.Equals(new(1, 1)):
                currentWASDMovepoint = movePoints[2];
                break;
            case Vector2 v when v.Equals(new(1, 0)):
                currentWASDMovepoint = movePoints[4];
                break;
            case Vector2 v when v.Equals(new(1, -1)):
                currentWASDMovepoint = movePoints[7];
                break;
            case Vector2 v when v.Equals(new(0, 1)):
                currentWASDMovepoint = movePoints[1];
                break;
            case Vector2 v when v.Equals(new(0, -1)):
                currentWASDMovepoint = movePoints[6];
                break;
            case Vector2 v when v.Equals(new(-1, 1)):
                currentWASDMovepoint = movePoints[0];
                break;
            case Vector2 v when v.Equals(new(-1, 0)):
                currentWASDMovepoint = movePoints[3];
                break;
            case Vector2 v when v.Equals(new(-1, -1)):
                currentWASDMovepoint = movePoints[5];
                break;
            default:
                break;
        }
        return true;
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

    public void MoveToRight()
    {
        controlState = ControlState.gameControl;
        var movePos = new Vector3(movePoints[4].position.x + 5, movePoints[4].position.y, movePoints[4].position.z);
        agent.ResetPath();
        agent.SetDestination(movePos);
        anim.SetBool("isMoving", true);
        anim.SetFloat("moveDirX", 1);
    }

    public void MoveToCliff(Vector3 position)
    {
        agent.SetDestination(position);
        anim.SetBool("isMoving", true);
        anim.SetFloat("moveDirY", 1);
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

    public void UpdateForCutscene()
    {
        currentWASDMovepoint = transform;
        anim.SetBool("isMoving", false);
        anim.SetFloat("moveDirX", 0);
        anim.SetFloat("moveDirY", 1);
        currentIdlePoint = new Vector2(0, 1);
    }

    public void ResetWASDPoint()
    {
        movementX = 0;
        movementY = 0;
        CalculateCurrentWasdPoint();
    }

    #region Getters
    public Vector2 GetCurrentIdlePoint()
    {
        return currentIdlePoint;
    }

    public float GetPlayerSpeed()
    {
        return speed;
    }
    #endregion

    #region Setters
    public void SetAgentPosition()
    {
        agent.SetDestination(currentWASDMovepoint.position);
    }

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

        if (controlState == ControlState.gameControl)
            ResetWASDPoint();
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

