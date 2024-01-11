using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region serialized fields
    [SerializeField] TentacleTargetManager targetManager;
    [SerializeField] List<Interaction> creatureInteractions;
    [SerializeField] float interactionCooldown;
    [SerializeField] float fleeCooldown = 13;
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
            if (creatureInteractions[i].targetStatusManager == statusManager)
            {
                StopCoroutine(creatureInteractions[i].fleeCheckCoroutine);
                creatureInteractions[i].fleeCheckCoroutine = StartCoroutine(FleeChecker(creatureInteractions[i]));

                if (creatureInteractions[i].onCooldown) continue;
                creatureInteractions[i].interactedTimes++;
                StartCoroutine(InteractionCooldown(i));
            }
            else if (creatureInteractions[i].targetStatusManager != statusManager)
            {
                bool existsInList = false;
                for (int j = 0; j < creatureInteractions.Count; j++)
                {
                    if (!existsInList)
                        existsInList = CheckIfIndexIsTarget(statusManager, j);
                }
                if (!existsInList)
                    NewEntry(statusManager);
            }
        }

    }

    bool CheckIfIndexIsTarget(StatusManager statusManager, int index)
    {
        if (creatureInteractions[index].targetStatusManager == statusManager)
            return true;
        return false;
    }

    void NewEntry(StatusManager statusManager)
    {
        Interaction newInteraction = new() { targetStatusManager = statusManager };
        newInteraction.Initialize();
        newInteraction.fleeCheckCoroutine = StartCoroutine(FleeChecker(newInteraction));
        creatureInteractions.Add(newInteraction);
    }

    IEnumerator InteractionCooldown(int index)
    {
        creatureInteractions[index].onCooldown = true;
        yield return new WaitForSeconds(interactionCooldown);
        creatureInteractions[index].onCooldown = false;
    }

    IEnumerator FleeChecker(Interaction interaction)
    {
        yield return new WaitForSeconds(fleeCooldown);
        print(interaction.creatureName + " fled");
    }

    [Serializable]
    class Interaction
    {
        [HideInInspector] public string creatureName;
        public StatusManager targetStatusManager;


        public int interactedTimes;
        public bool onCooldown;
        public float fleeCooldown;
        public Coroutine fleeCheckCoroutine;
        public void Initialize()
        {
            if (creatureName.IsNullOrEmpty())
            {
                creatureName = targetStatusManager.CreatureType.ToString();
            }
        }
    }
}