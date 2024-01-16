using UnityEngine;

public class AttachGameObjectPerPos : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Transform attachObject;
    [SerializeField] bool nullZ;
    Transform trans;
    #endregion

    void Awake() => trans = transform;

    void Update()
    {
        Vector3 attachPos = attachObject.position;
        if (nullZ)
            attachPos.z = 0;
        trans.position = attachPos;
    }

}