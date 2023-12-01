using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Camera cam;
    #endregion

    #region private fields
    Transform trans;
    #endregion

    void Awake()
    {
        trans = transform;
    }

    void Update()
    {
        trans.position = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}