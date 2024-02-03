using UnityEngine;

public class BubbleInteractable : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public Animator animator;

    void Awake()
    {
        tutorialManager = GetComponentInParent<TutorialManager>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play("BubblePop");
        }
    }
}