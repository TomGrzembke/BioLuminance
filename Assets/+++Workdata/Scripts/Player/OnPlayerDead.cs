using UnityEngine;

public class OnPlayerDead : MonoBehaviour
{
    #region serialized fields
    [SerializeField] HealthSubject healthSubject;
    #endregion

    #region private fields

    #endregion

    void OnEnable()
    {
        healthSubject.RegisterOnAliveChanged(OnPlayerDied);
    }
    void OnDisable()
    {
        healthSubject.OnCreatureDied -= OnPlayerDied;
    }

    void OnPlayerDied(bool dead)
    {
        if (dead)
            SkillManager.ToggleSkillManager();
    }
}