using UnityEngine;
using UnityEngine.Events;

public class DebugControls : MonoBehaviour
{
    #region serialized fields
    [SerializeField] UnityEvent slashEvent;
    [SerializeField] UnityEvent asteriskEvent;
    [SerializeField] UnityEvent num7;

    [SerializeField] GameObject[] toggleUIAsterisk;
    [SerializeField] Transform playerTrans;
    [SerializeField] Transform bossTargetTrans;
    [SerializeField] GameObject[] abilitiesToAdd;
    #endregion

    #region private fields
    PlayerInputActions inputActions;
    #endregion

    void Awake()
    {
        inputActions = new();
        inputActions.Player.Debug7.performed += ctx => CallNum7(num7);
        inputActions.Player.DebugAsterisk.performed += ctx => CallAsteriskEvent();
        inputActions.Player.DebugSlash.performed += ctx => CallSlashEvent();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();
    }

    public void CallNum7(UnityEvent givenEvent)
    {
        for (int i = 0; i < abilitiesToAdd.Length; i++)
        {
            AbilitySlotManager.Instance.AddNewAbility(abilitiesToAdd[i]);
        }

        givenEvent?.Invoke();
    }
    public void CallAsteriskEvent()
    {
        for (int i = 0; i < toggleUIAsterisk.Length; i++)
        {
            toggleUIAsterisk[i].SetActive(!toggleUIAsterisk[i].activeInHierarchy);
        }

        asteriskEvent?.Invoke();
    }
    public void CallSlashEvent()
    {
        slashEvent?.Invoke();

        if (playerTrans == null || bossTargetTrans == null) return;
        playerTrans.rotation = Quaternion.identity;
        playerTrans.position = bossTargetTrans.position;
    }
}