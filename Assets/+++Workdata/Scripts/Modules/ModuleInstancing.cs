using UnityEngine;

/// <summary> Handles an array of given possible Modules to instance itself as one </summary>
public class ModuleInstancing : MonoBehaviour
{
    #region serialized fields
    GameObject[] possibleModules;
    #endregion

    #region private fields

    #endregion

    void Start()
    {
        Instantiate(GetRandomModule());
    }

    GameObject GetRandomModule()
    {
        return possibleModules[Random.Range(0, possibleModules.Length)];
    }
}
