using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    #region serialized fields

    [SerializeField] GameObject blade;

    #endregion

    #region private fields

    Animator anim;

    #endregion

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void StopAttackAnim()
    {
        anim.SetBool("shouldAttack", false);
    }
    public void PlayAttackAnim()
    {
        anim.SetBool("shouldAttack", true);
        anim.Play("EnemyAttack");
    }
}