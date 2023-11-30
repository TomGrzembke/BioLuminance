using UnityEngine;

public class TentacleController : MonoBehaviour
{
    #region serialized fields
    [SerializeField] TentacleTargetManager tentacleTargetManager;
    [SerializeField] Target target;
    #endregion

    #region private fields
    PlayerInputActions inputActions;

    #endregion

    void Awake()
    {
        tentacleTargetManager = GetComponent<TentacleTargetManager>();
        inputActions = new();

        inputActions.Player.Attack.performed += ctx => Attack();
        inputActions.Player.Attack.canceled += ctx => ResetTentacles();
    }

    void Attack()
    {
        tentacleTargetManager.AddAttackPoint(target.transform);
    }
    void ResetTentacles()
    {
        tentacleTargetManager.RemoveAttackPoint(target.transform);
    }

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