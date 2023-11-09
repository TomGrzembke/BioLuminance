using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureLogic : MonoBehaviour
{
    public static event Action<CreatureLogic> OnEnemyDied;
    private Health h;
    
    private void Awake() => h = GetComponent<Health>();

    private void OnEnable()
    {
        h.OnHealthChanged += TestForDeath;
    }

    private void TestForDeath(float health)
    {
        if (health <= 0)
        {
            if (OnEnemyDied != null)
                OnEnemyDied(this);

            Destroy(gameObject);
        }
    }
    
    public Health GetHealth()
    {
        return h;
    }
}