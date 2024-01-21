using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBubble : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    [SerializeField] HealthSubject healthSubject;
    [SerializeField] GameObject parent;
    private Animator animator;


    private void Awake()
    {
        tutorial.SetActive(PlayerPrefs.GetInt("TutorialFinished") == 0);
        animator = GetComponent<Animator>();
        StartCoroutine(CheckDead());
    }

    private void OnDeath(bool died)
    {
        if (died)
        {
            animator.Play("BubblePop");
            PlayerPrefs.SetInt("TutorialFinished", 1);
        }
    }

    private void OnEnable()
    {
        healthSubject.RegisterOnCreatureDied(OnDeath);
    }

    private void OnDisable()
    {
        healthSubject.OnCreatureDied -= OnDeath;
    }

    IEnumerator CheckDead()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            
            print("ded");

            if (healthSubject.currentHealth != 0) continue;
            animator.Play("BubblePop");
            StartCoroutine(IsDead());
                
            yield break;
        }
    }

    IEnumerator IsDead()
    {
        yield return new WaitForSeconds(0.2f);
        print("wat");
        parent.SetActive(false);
    }
}