using UnityEngine;

public class AttachGameObjectPerPos : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Transform attachObject;
    Transform trans;
    #endregion

    void Awake() => trans = transform;

    void FixedUpdate()
    {
        trans.position = attachObject.position;
    }

}