using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    public static PointSystem Instance;
    public float Points => points;
    [SerializeField] float points;
    [SerializeField] List<PointPool> pointPools;
    [SerializeField] List<StatusManager> allObjects;
    public List<Creatures> creaturesList = new();

    void Awake()
    {
        Instance = this;
        points = PlayerPrefs.GetFloat("SkillPoints");
    }

    void Update()
    {
        foreach (var pointPool in pointPools)
        {
            pointPool.creaturesInScene = Mathf.Clamp(pointPool.creaturesInScene, 0, 99999);
            pointPool.initialDNAAmount = Mathf.Clamp(pointPool.initialDNAAmount, 0, 99999);
        }
    }

    [ButtonMethod]
    public void GetCreatures()
    {
        pointPools.Clear();
        allObjects.Clear();
        creaturesList.Clear();

        CollectCreatures();
    }

    void CollectCreatures()
    {
        // Goes through the Inspector and adds any object with the StatusManager script to the List "allObjects"
        allObjects = new List<StatusManager>(FindObjectsOfType<StatusManager>());
        AssignCreatures();
    }

    void AssignCreatures()
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
            PointPool pointPool = new();
            pointPool.creature = creatures;

            // Count the occurrences of the creature type in allObjects
            int count = allObjects.Count(obj => obj.CreatureType == creatures);

            // Set the creatureAmount property
            pointPool.creaturesInScene = count;

            pointPools.Add(pointPool);
        }

        // Goes through every element in PointPool List and adds the Flag (creature) to the private string (creatureName)
        foreach (PointPool pointPool in pointPools)
        {
            pointPool._creatureName = pointPool.creature.ToString();
        }
    }

    public void SetCreatureDnaStats(Creatures creatureType, float points)
    {
        foreach (var pointPool in pointPools)
        {
            if (pointPool.creature == creatureType)
                pointPool.initialDNAAmount = points;
        }
    }

    public void CalculatePoints(Creatures creatureType)
    {
        float beforCalcPoints = points;

        foreach (var pointPool in pointPools)
        {
            if (pointPool.creature == creatureType)
            {
                points += pointPool.initialDNAAmount;
                RewardWindow.Instance.ShowCurrency(points, pointPool.initialDNAAmount);
                float percent = pointPool.initialDNAAmount / pointPool.creaturesInScene;
                pointPool.initialDNAAmount -= percent;
                pointPool.creaturesInScene--;

                pointPool.initialDNAAmount = Mathf.Round(pointPool.initialDNAAmount);
            }
        }

        if (points != beforCalcPoints)
            PlayerPrefs.SetFloat("SkillPoints", points);
    }

    private void OnApplicationQuit()
    { 
        PlayerPrefs.SetFloat("SkillPoints", points);
    }

    public void CalculatePoints(int pointsMinus)
    {
        points -= pointsMinus;
        if (RewardWindow.Instance != null)
            RewardWindow.Instance.ShowCurrency(points, -pointsMinus);
    }
}

[Serializable]
public class PointPool
{
    [HideInInspector] public string _creatureName;
    public Creatures creature;
    public float creaturesInScene;
    [Space(5)] public float initialDNAAmount;
}