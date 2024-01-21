using System.Collections;
using UnityEngine;

public class DoubleAnimOnImpact : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Animator anim;
    [SerializeField] float multiplier = 2;
    [SerializeField] float duration = 2;
    [SerializeField] float cooldown = 3;
    #endregion

    #region private fields
    Coroutine speedRoutine;
    #endregion

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Creature"))
        {
            if (speedRoutine == null)
                speedRoutine = StartCoroutine(DoubleSpeedCor());
        }
    }

    IEnumerator DoubleSpeedCor()
    {
        float timeWentBy = 0;
        float initialSpeed = anim.speed;

        anim.speed = initialSpeed * multiplier;

        while (timeWentBy < duration)
        {
            anim.speed = Mathf.Lerp(initialSpeed * multiplier, initialSpeed, timeWentBy / duration);
            timeWentBy += Time.deltaTime;
            yield return null;
        }

        anim.speed = initialSpeed;
        speedRoutine = null;
    }
}