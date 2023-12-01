using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Transform middle;
    public List<StatusManager> PossibleTargets => possibleTargets;
    [SerializeField] List<StatusManager> possibleTargets = new();
    public bool HasTargets => possibleTargets.Count > 0;
    public LayerMask CreatureLayer => creatureLayer;
    [SerializeField] LayerMask creatureLayer;
    [SerializeField] float detectionRadius;
    [SerializeField] StatusManager playerStatusManager;
    #endregion

    #region private fields

    #endregion

    void Update()
    {
        DetectTargets();
    }

    private void DetectTargets()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(middle.position, detectionRadius, creatureLayer);
        possibleTargets.Clear();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (!colliders[i].TryGetComponent(out StatusManager statusTarget))
                statusTarget = colliders[i].GetComponentInChildren<StatusManager>();

            if (statusTarget == playerStatusManager)
                continue;

            if (!possibleTargets.Contains(statusTarget))
                possibleTargets.Add(statusTarget);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, detectionRadius); //This visualizes the detection radius
    }
#endif
}