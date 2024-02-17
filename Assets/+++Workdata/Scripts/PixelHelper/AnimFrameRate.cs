using System.Collections;
using UnityEngine;

public class AnimFrameRate : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] int animFPS = 12;
    float frameAlpha = 0.072f;
    float savedAnimSpeed;

    [SerializeField] float _frameAlpha;
    [SerializeField] float fpsPercent;

    void Start()
    {
        savedAnimSpeed = anim.speed;
        StartCoroutine(FrameCycle());
    }

    IEnumerator FrameCycle()
    {
        _frameAlpha = animFPS * 0.6f / 100;
        fpsPercent = Mathf.Clamp(Mathf.RoundToInt(animFPS * 1.667f), 0, 100);
        float fpsAlpha = (fpsPercent / 100);


        for (int i = 0; i < 60 - animFPS - 1; i++)
        {
            yield return null;
        }

        anim.speed = savedAnimSpeed;
        anim.Play(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, anim.GetCurrentAnimatorStateInfo(0).normalizedTime + fpsAlpha);

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