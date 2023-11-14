using UnityEngine;

public class AttachGameObjectPerPos : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Transform attachObject;
    #endregion

    void FixedUpdate()
    {
        transform.position = attachObject.position;
    }

}