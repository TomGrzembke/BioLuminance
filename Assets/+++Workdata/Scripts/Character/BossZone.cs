using Cinemachine;
using System.Collections;
using UnityEngine;

public class BossZone : MonoBehaviour
{
    #region serialized fields
    [SerializeField] CanvasGroup bossBarCG;
    [SerializeField] float fadeTime = 2;
    [SerializeField] CinemachineVirtualCamera playerCam;
    [SerializeField] float zoomOutTime = 2;
    [SerializeField] float newCamDistance = 25;
    [SerializeField] int initialRadius = 40;
    [SerializeField] int exitRadius = 65;
    [SerializeField] CircleCollider2D col;
    #endregion

    #region private fields
    float oldCamSize = 14;
    Coroutine uiFadeCor;
    Coroutine camFadeCor;
    #endregion

    void Start()
    {
        col.radius = initialRadius;
    }

    public void Enter()
    {
        if (!enabled) return;

        col.radius = exitRadius;

        if (playerCam && camFadeCor == null)
            oldCamSize = playerCam.m_Lens.OrthographicSize;

        if (uiFadeCor != null)
            StopCoroutine(uiFadeCor);
        uiFadeCor = StartCoroutine(UIFade(true));

        if (camFadeCor != null)
            StopCoroutine(camFadeCor);
        camFadeCor = StartCoroutine(CamFade(true));
    }

    public void Exit()
    {
        if (!enabled) return;

        col.radius = initialRadius;

        if (uiFadeCor != null)
            StopCoroutine(uiFadeCor);
        uiFadeCor = StartCoroutine(UIFade(false));

        if (camFadeCor != null)
            StopCoroutine(camFadeCor);
        camFadeCor = StartCoroutine(CamFade(false));
    }

    IEnumerator UIFade(bool condition)
    {
        float timeSpend = 0;
        float currentAlpha = bossBarCG.alpha;
        while (timeSpend < fadeTime)
        {
            timeSpend += Time.deltaTime;
            bossBarCG.alpha = condition ? Mathf.Lerp(currentAlpha, 1, timeSpend / fadeTime) : Mathf.Lerp(currentAlpha, 0, timeSpend / fadeTime);
            yield return null;
        }
    }

    IEnumerator CamFade(bool condition)
    {
        float timeSpend = 0;
        float currentCamSize = playerCam.m_Lens.OrthographicSize;
        playerCam.m_Lens.OrthographicSize -= .1f;

        while (timeSpend < zoomOutTime)
        {
            timeSpend += Time.deltaTime;
            playerCam.m_Lens.OrthographicSize = condition ? Mathf.Lerp(currentCamSize, newCamDistance, timeSpend / zoomOutTime) : Mathf.Lerp(currentCamSize, oldCamSize, timeSpend / zoomOutTime);
            yield return null;
        }

        camFadeCor = null;
    }
}