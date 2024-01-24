using System.Collections;
using UnityEngine;

public class ClickBubble : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    [SerializeField] HealthSubject healthSubject;
    [SerializeField] GameObject parent;
    Animator animator;


    void Awake()
    {
        tutorial.SetActive(PlayerPrefs.GetInt("TutorialFinished") == 0);
        animator = GetComponent<Animator>();
        if (gameObject.activeInHierarchy)
            StartCoroutine(CheckDead());
    }

    void OnDeath(bool died)
    {
        if (died)
        {
            animator.Play("BubblePop");
            PlayerPrefs.SetInt("TutorialFinished", 1);
        }
    }

    void OnEnable()
    {
        healthSubject.RegisterOnCreatureDied(OnDeath);
    }

    void OnDisable()
    {
        healthSubject.OnCreatureDied -= OnDeath;
    }

    IEnumerator CheckDead()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (healthSubject.currentHealth != 0) continue;
            StartCoroutine(IsDead());

            yield break;
        }
    }

    IEnumerator IsDead()
    {
        yield return new WaitForSeconds(0.2f);
        parent.SetActive(false);
    }
}