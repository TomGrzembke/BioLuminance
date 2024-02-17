using System.Collections;
using UnityEngine;

public class AnimFrameRate : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] int animeFPS = 12;
    [SerializeField] int divider = 12;
    void Start()
    {
        StartCoroutine(FrameCycle());
    }

    IEnumerator FrameCycle()
    {
        anim.speed = 0;

        for (int i = 0; i < divider - 1; i++)
        {
            yield return null;
        }
        anim.speed = 1;

        yield return null;
        anim.speed = 0;

        StartCoroutine(FrameCycle());
    }
    void Update()
    {
        //print(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 2)
        //    anim.speed = 0;
    }
}