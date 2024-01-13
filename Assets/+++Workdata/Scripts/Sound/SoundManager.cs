using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] SoundTypeSO[] soundBank;
    #endregion

    #region private fields

    #endregion


}

public enum SoundType
{
    Null,
    ButtonHover,
    ButtonClick,
    ButtonClickConfirm,
    ButtonClickBack,

    Bubble,

    PointCounter,

}