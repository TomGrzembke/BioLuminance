using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [SerializeField] private GameObject givePointsTo;
    public List<PointPool> pointPools = new List<PointPool>();
    public StatusManager[] allObjects;
    public List<Creatures> creaturesList;

    private void Awake()
    {
        CollectCreatures();
    }

    private void Start()
    {
        // Goes through every element in PointPool List and adds the Flag (creature) to the private string (creatureName)
        foreach (PointPool pointPool in pointPools)
        {
            pointPool._creatureName = pointPool.creature.ToString();
        }
    }

    public void CollectCreatures()
    {
        // Goes through every object and checks for the script "StatusManager"
        allObjects = FindObjectsOfType<StatusManager>();
        AssignCreatures();
    }

    public void AssignCreatures()
    {
        // Goes through the list "allObjects"
        foreach (StatusManager statusManager in allObjects)
        {
            // Adds the Flag to a Custom List of Flags, and sorts out duplicates
            if (!creaturesList.Contains(statusManager.CreatureType))
            {
                creaturesList.Add(statusManager.CreatureType);
            }
        }

        // This gets the length of creaturesList, adds it to pointPools and changes the name accordingly
        foreach (Creatures creatures in creaturesList)
        {
            PointPool pointPool = new PointPool();
            pointPool.creature = creatures;
            pointPools.Add(pointPool);
        }

        foreach (PointPool pointPool in pointPools)
        {
            //pointPool.creatureAmount = creaturesAmount;
        }
    }
}

[Serializable]
public class PointPool
{
    [HideInInspector] public string _creatureName;
    public Creatures creature;
    public float creatureAmount;
}