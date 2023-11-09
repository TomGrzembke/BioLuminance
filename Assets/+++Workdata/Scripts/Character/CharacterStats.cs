using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    #region serialized fields
    
    [Header("Health Stats")]
    public int health;
    public int maxHealth;
    public int currentHealth;

    #endregion

    #region private fields

    private Health h;
    
    #endregion

    private void Awake() => h = GetComponent<Health>();

    public Health GetHealth()
    {
        return h;
    }
}