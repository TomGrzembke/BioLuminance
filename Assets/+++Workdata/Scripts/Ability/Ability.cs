using UnityEngine;

public class Ability : MonoBehaviour
{
    #region serialized fields
    [SerializeField] protected Sprite abilitySprite;
    public Sprite AbilitySprite => abilitySprite;
    #endregion

    #region private fields

    #endregion
    void Start()
    {
        OnInitialized();
    }

    public void Execute(bool deactivate = false)
    {
        print(deactivate + " Exec " + gameObject.name);
    }

    public void OnInitialized()
    {
        print("Init " + gameObject.name);
    }
}