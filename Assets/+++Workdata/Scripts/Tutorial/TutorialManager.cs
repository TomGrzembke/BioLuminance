using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    
    public GameObject player;
    public List<GameObject> bubbles;

    private void Awake()
    {
        Instance = this;
        
        player = GameObject.FindGameObjectWithTag("Player");
        
        foreach (Transform child in transform)
        {
            bubbles.Add(child.gameObject);
        }
    }
    
    [ButtonMethod]
    public void SnapBubblesToPlayer()
    {
        gameObject.transform.position = player.transform.position;
        ActivateBubbles();
    }

    private void ActivateBubbles()
    {
        for (int i = 0; i < bubbles.Count; i++)
        {
            bubbles[i].SetActive(true);
        }
    }

    [ButtonMethod]
    public void ResetBubbleAnimator()
    {
        for (int i = 0; i < bubbles.Count; i++)
        {
            Animator anim = bubbles[i].GetComponent<Animator>();
            anim.Play("BubbleIdle");
        }
    }
}