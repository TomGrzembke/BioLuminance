using System.Collections;
using UnityEngine;

public class AnimFrameRate : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] int animFPS = 12;
    float frameAlpha = 0.072f;
    float savedAnimSpeed;
    float lastRenderTime = 0;
    [SerializeField] int frameRate = 12;

    [SerializeField] float _frameAlpha;
    [SerializeField] float fpsPercent;

    void Start()
    {
        savedAnimSpeed = anim.speed;
        StartCoroutine(FrameCycle());
    }

    void Update()
    {
        if (Time.time - lastRenderTime > 1f / frameRate)
        {
            lastRenderTime = Time.time;
            
        }
    }

    IEnumerator FrameCycle()
    {
        _frameAlpha = animFPS * 0.6f / 100;
        fpsPercent = Mathf.Clamp(Mathf.RoundToInt(animFPS * 1.667f), 0, 100);
        float fpsAlpha = (fpsPercent / 100);
        float frameDifference = 60 - animFPS;
        float animClipLength = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        float _frameAlpha1 = animClipLength / anim.GetCurrentAnimatorStateInfo(0).normalizedTime;


        for (int i = 0; i < frameDifference; i++)
        {
            yield return null;
        }

        anim.speed = savedAnimSpeed;
        anim.Play(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, anim.GetCurrentAnimatorStateInfo(0).normalizedTime + _frameAlpha);

        yield return null;
        anim.speed = 0;

        for (int i = 0; i < animFPS - 1; i++)
        {
            yield return null;
        }


        StartCoroutine(FrameCycle());
    }
}