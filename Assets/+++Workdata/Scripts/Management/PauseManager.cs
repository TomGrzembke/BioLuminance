using UnityEngine;

public class PauseManager : MonoBehaviour
{
    #region serialized fields

    #endregion

    #region private fields
    PlayerInputActions inputActions;
    bool paused;
    #endregion

    void Awake()
    {
        inputActions = new();

        inputActions.Player.Pause.performed += ctx => PauseButton();
    }

    void PauseButton()
    {
        GameStateManager.OptionsWindow(!paused);
    }

    public void PauseLogic()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
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