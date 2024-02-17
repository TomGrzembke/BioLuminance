using System.Collections;
using UnityEngine;

public class AnimFrameRate : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] int animFPS = 12;
    [SerializeField] float frameAlpha = 0.072f;
    float FrameAlpha => animFPS * 0.6f / 100;
    void Start()
    {
        StartCoroutine(FrameCycle());
    }

    IEnumerator FrameCycle()
    {
        for (int i = 0; i < animFPS - 1; i++)
        {
            yield return null;
        }

        anim.Play(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, anim.GetCurrentAnimatorStateInfo(0).normalizedTime + FrameAlpha);

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