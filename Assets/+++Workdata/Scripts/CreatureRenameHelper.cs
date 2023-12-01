using System.Collections.Generic;
using UnityEngine;

public class CreatureRenameHelper : MonoBehaviour
{
    #region serialized fields
    [SerializeField] string creatureName = "CreatureName";
    [SerializeField] List<GameObject> objToRename = new();
    #endregion

    #region private fields

    #endregion

    void OnValidate()
    {
        for (int i = 0; i < objToRename.Count; i++)
        {
            string[] splitArray = objToRename[i].name.Split("]");
            objToRename[i].name = splitArray[0] + "] " + creatureName;
        }
    }
}