using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleInteractable : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public Animator animator;

    private void Awake()
    {
        tutorialManager = GetComponentInParent<TutorialManager>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play("BubblePop");
            print("triggered");
        }
    }
}