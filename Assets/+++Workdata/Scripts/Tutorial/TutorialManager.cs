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
        StartCoroutine(DistanceCheck());
    }

    private void ActivateBubbles()
    {
        for (int i = 0; i < bubbles.Count; i++)
        {
            bubbles[i].SetActive(true);
        }
    }
    
    private void DeactivateBubbles()
    {
        for (int i = 0; i < 4; i++)
        {
            bubbles[i].SetActive(false);
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

    IEnumerator DistanceCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            float distance = Vector2.Distance(gameObject.transform.position, player.transform.position);
        
            if (distance > 30)
            {
                print("check");
                DeactivateBubbles();
                yield break;
            }
        }
    }

}