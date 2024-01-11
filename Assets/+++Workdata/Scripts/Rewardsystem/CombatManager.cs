using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] TentacleTargetManager targetManager;
    [SerializeField] List<Interaction> creatureInteractions;
    #endregion

    #region private fields

    #endregion

    public void CreatureInteraction(StatusManager statusManager)
    {
        if (creatureInteractions.Count == 0)
        {
            NewEntry(statusManager);
        }

        for (int i = 0; i < creatureInteractions.Count; i++)
        {
            if (creatureInteractions[i].targetStatusManager != statusManager)
            {
                NewEntry(statusManager);
            }
            else if (creatureInteractions[i].targetStatusManager == statusManager)
                creatureInteractions[i].interactedTimes++;
        }
    }

    void NewEntry(StatusManager statusManager)
    {
        Interaction newInteraction = new() { targetStatusManager = statusManager };
        newInteraction.SetName();
        creatureInteractions.Add(newInteraction);
    }

    [Serializable]
    class Interaction
    {
        [HideInInspector] public string creatureName;
        public StatusManager targetStatusManager;


        public int interactedTimes;
        public float interactionCooldwon;

        public void SetName()
        {
            if (creatureName.IsNullOrEmpty())
            {
                creatureName = targetStatusManager.CreatureType.ToString();
            }
        }
    }
}