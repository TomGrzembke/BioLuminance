using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingerDamage : MonoBehaviour
{
    StingrayStinger stingrayStinger;

    private void Awake()
    {
        stingrayStinger = GetComponentInParent<StingrayStinger>();
    }

    private void Update()
    {
        if (stingrayStinger.isAttacking)
        {
            stingrayStinger.placeholderHealth -= stingrayStinger.stingerDamage; // TODO DO DAMAGE
        }
    }
}