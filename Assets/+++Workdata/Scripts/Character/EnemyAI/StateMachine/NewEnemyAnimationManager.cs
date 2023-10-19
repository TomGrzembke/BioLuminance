using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyAnimationManager : MonoBehaviour
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