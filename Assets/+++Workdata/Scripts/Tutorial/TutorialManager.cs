using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> bubbles;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            bubbles.Add(child.gameObject);
        }
    }

    [ButtonMethod]
    public void SnapToPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.transform.position = player.transform.position;
        ActivateBubbles();
    }

    public void ActivateBubbles()
    {
        for (int i = 0; i < bubbles.Count; i++)
        {
            bubbles[i].SetActive(true);
        }
    }
}