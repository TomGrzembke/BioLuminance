using UnityEngine;

public class ForceResolution : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Vector2 screenRes;
    #endregion

    #region private fields

    #endregion

    void Start()
    {
        Screen.SetResolution((int)screenRes.x, (int)screenRes.y, Screen.fullScreen);
    }
}