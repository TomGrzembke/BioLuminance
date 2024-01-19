using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBubble : MonoBehaviour
{
    public HealthSubject healthSubject;
    public GameObject parent;
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CheckDead());
    }

    private void Update()
    {
        if (healthSubject.currentHealth == 0)
        {
            animator.Play("BubblePop");
        }
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