using MyBox;
using UnityEngine;

public class ParentSwapper : MonoBehaviour
{
    #region serialized fields
    [SerializeField] bool disablePlayer;
    [SerializeField, ConditionalField(nameof(disablePlayer))] PlayerMovement playerMovement;
    [SerializeField] Transform targetTrans;
    #endregion

    #region private fields
    Transform originalParent;
    Transform obj;
    #endregion

    public void Unparent()
    {
        targetTrans.parent = null;
    }

    public void Swap(Transform _obj, Transform tempParent)
    {
        obj = _obj;
        originalParent = obj.parent;
        obj.parent = tempParent;

        if (disablePlayer)
            playerMovement.SetControlState(PlayerMovement.ControlState.gameControl);
    }

    public void UnSwap()
    {
        obj.parent = originalParent;

        if (disablePlayer)
            playerMovement.ReenableMovement();
    }
}