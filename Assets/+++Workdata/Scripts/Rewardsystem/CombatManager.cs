using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
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
        for (int i = 0; i < creatureInteractions.Count; i++)
        {
            if (creatureInteractions[i].targetStatusManager != statusManager)
            {
                Interaction newInteraction = new() { targetStatusManager = statusManager };
                creatureInteractions.Add(newInteraction);
            }
            else if (creatureInteractions[i].targetStatusManager == statusManager)
                creatureInteractions[i].interactedTimes++;
        }
    }

    [Serializable]
    class Interaction
    {
        public StatusManager targetStatusManager;
        public int interactedTimes;
        public float interactionCooldwon;
    }
}