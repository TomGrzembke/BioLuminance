using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    #region serialized fields

    #endregion

    #region private fields

    Animator anim;

    #endregion

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
}