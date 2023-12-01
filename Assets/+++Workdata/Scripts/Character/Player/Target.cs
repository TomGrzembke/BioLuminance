using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Camera cam;
    public List<StatusManager> PossibleTargets => possibleTargets;
    [SerializeField] List<StatusManager> possibleTargets = new();
    public bool HasTargets => possibleTargets.Count > 0;
    public LayerMask CreatureLayer => creatureLayer;
    [SerializeField] LayerMask creatureLayer;
    [SerializeField] float detectionRadius;
    [SerializeField] StatusManager playerStatusManager;
    #endregion

    #region private fields
    Transform trans;
    #endregion

    void Awake()
    {
        trans = transform;
    }

    void Update()
    {
        trans.position = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        DetectTargets();
    }

    private void DetectTargets()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, creatureLayer);

        if (colliders.Length <= 0)
        {
            possibleTargets.Clear();
            return;
        }

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