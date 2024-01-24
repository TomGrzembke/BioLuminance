using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DeathState : State
{
    #region serialized fields
    [SerializeField] float deathTime = 5;
    [SerializeField] float animDyingSpeedDivider = 2;
    [SerializeField] Transform gfxTrans;
    [SerializeField] Transform headTrans;

#pragma warning disable CS0108 
    [SerializeField] ParticleSystem particleSystem;
#pragma warning restore CS0108 

    [SerializeField] Animator anim;
    [SerializeField] GameObject[] objToDisableFirst;
    [SerializeField] GameObject[] objToDisableLast;
    [SerializeField] UnityEvent eventAtLast;
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
        particleSystem.Play();

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



        yield return new WaitForSeconds(deathTime / 3);

        float dissolveTime = 0;
        Vector3 currentScale = gfxTrans.localScale;
        while (dissolveTime < deathTime)
        {
            dissolveTime += Time.deltaTime * 3;
            gfxTrans.localScale = currentScale * -(-1 + (dissolveTime / deathTime));
            gfxTrans.Rotate(0, 0, -.1f);
            yield return null;
        }

        yield return new WaitForSeconds(deathTime / 3);

        gfxTrans.gameObject.SetActive(false);

        eventAtLast?.Invoke();

        for (int i = 0; i < objToDisableLast.Length; i++)
        {
            if (objToDisableLast[i])
                objToDisableLast[i].SetActive(false);
        }

        Destroy(headTrans.gameObject);
    }

    IEnumerator SlowDownAnimCoroutine()
    {
        if (!anim) yield break;

        float slowTime = 0;
        float animSpeed = anim.speed;
        while (slowTime < deathTime)
        {
            slowTime += Time.deltaTime * animDyingSpeedDivider;
            anim.speed = Mathf.Clamp(animSpeed * (-(-1 + slowTime / deathTime)), 0, animSpeed);
            yield return null;
        }
    }
}