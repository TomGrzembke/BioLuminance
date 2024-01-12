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
    [SerializeField] int interactionNeeded = 4;
    [SerializeField] float fleeCooldown = 13;
    #endregion

    #region private fields
    public static CombatManager Instance;
    #endregion
    void Awake()
    {
        Instance = this;
    }
    void GiftRewardPassive(Interaction interaction)
    {
        AbilitySlotManager.Instance.AddNewAbility(interaction.targetStatusManager.CreatureRewards.PassiveReward);
    }

    void GiftRewardActive(int index)
    {
        AbilitySlotManager.Instance.AddNewAbility(creatureInteractions[index].targetStatusManager.CreatureRewards.ActiveReward);
    }

    public void CreatureInteraction(StatusManager statusManager)
    {
        if (statusManager.HealthSubject.IsDead) return;

        if (creatureInteractions.Count == 0)
            NewEntry(statusManager);


        for (int i = 0; i < creatureInteractions.Count; i++)
        {
            if (creatureInteractions[i].targetStatusManager == statusManager)
            {
                StopCoroutine(creatureInteractions[i].fleeCheckCoroutine);
                creatureInteractions[i].fleeCheckCoroutine = StartCoroutine(FleeChecker(creatureInteractions[i]));

                if (creatureInteractions[i].onCooldown) continue;
                creatureInteractions[i].interactedTimes++;
                creatureInteractions[i].interactionCooldownCoroutine = StartCoroutine(InteractionCooldown(i));
            }
            else
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

    void CheckCreatureKilled(bool _)
    {
        for (int i = 0; i < creatureInteractions.Count; i++)
        {
            if (!creatureInteractions[i].dead) continue;

            StopCoroutine(creatureInteractions[i].fleeCheckCoroutine);
            StopCoroutine(creatureInteractions[i].interactionCooldownCoroutine);

            GiftRewardActive(i);
            creatureInteractions.RemoveAt(i);
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

        HealthSubject newInteractionHealthSubject = newInteraction.targetStatusManager.HealthSubject;
        newInteractionHealthSubject.RegisterOnCreatureDied(newInteraction.ChangeDead);
        newInteractionHealthSubject.RegisterOnCreatureDied(CheckCreatureKilled);
    }

    IEnumerator InteractionCooldown(int index)
    {
        creatureInteractions[index].onCooldown = true;
        yield return new WaitForSeconds(interactionCooldown);
        if (creatureInteractions[index] != null)
            creatureInteractions[index].onCooldown = false;
    }

    IEnumerator FleeChecker(Interaction interaction)
    {
        yield return new WaitForSeconds(fleeCooldown);
        if (interactionNeeded <= interaction.interactedTimes)
        {
            GiftRewardPassive(interaction);
        }

        creatureInteractions.Remove(interaction);
    }

    [Serializable]
    class Interaction
    {
        [HideInInspector] public string creatureName;
        public StatusManager targetStatusManager;

        public bool dead = false;
        public int interactedTimes;
        public bool onCooldown;
        public Coroutine fleeCheckCoroutine;
        public Coroutine interactionCooldownCoroutine;

        public void Initialize()
        {
            if (creatureName.IsNullOrEmpty())
            {
                creatureName = targetStatusManager.CreatureType.ToString();
            }
        }
        public void ChangeDead(bool _dead)
        {
            dead = _dead;
        }
    }
}