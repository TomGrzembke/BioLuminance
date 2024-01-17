using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeathState : State
{
    #region serialized fields
    [SerializeField] float deathTime = 5;
    [SerializeField] Transform gfxTrans;
    [SerializeField] Transform headTrans;
    [SerializeField] Animator anim;
    [SerializeField] GameObject[] objToDisableFirst;
    [SerializeField] GameObject[] objToDisableLast;
    [SerializeField] MonoBehaviour[] scriptsToDisable;

    #endregion

    #region private fields

    #endregion

    public override State SwitchStateInternal()
    {

        return this;
    }

    protected override void EnterInternal()
    {
        if (gfxTrans != null && headTrans != null)
            gfxTrans.SetParent(headTrans);

        StartCoroutine(DeathCoroutine());
        StartCoroutine(SlowDownAnimCoroutine());
    }

    protected override void ExitInternal()
    {
    }

    protected override void FixedUpdateInternal()
    {
    }

    protected override void UpdateInternal()
    {
    }

    IEnumerator DeathCoroutine()
    {

        for (int i = 0; i < objToDisableFirst.Length; i++)
        {
            if (objToDisableFirst[i])
                objToDisableFirst[i].SetActive(false);
        }

        List<MonoBehaviour> list = scriptsToDisable.ToList();
        list.RemoveAll(x => x == null);
        for (int i = 0; i < scriptsToDisable.Length; i++)
        {
            scriptsToDisable[i].enabled = false;
        }

        yield return new WaitForSeconds(deathTime);


        for (int i = 0; i < objToDisableLast.Length; i++)
        {
            if (objToDisableLast[i])
                objToDisableLast[i].SetActive(false);
        }
    }

    IEnumerator SlowDownAnimCoroutine()
    {
        float slowTime = 0;
        if (anim)
            while (slowTime < deathTime)
            {
                slowTime += Time.deltaTime;
                anim.speed *= -(-1 + slowTime / deathTime);
                yield return null;
            }
    }
}