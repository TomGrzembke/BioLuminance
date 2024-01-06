using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class TentacleDetection : MonoBehaviour
{
    #region serialized fields
    [SerializeField] TentacleEffects tentacleEffects = new();
    [SerializeField] StatusManager ownStatusManager;
    [SerializeField] int pointsDividedBy = 3;
    [SerializeField] ContactFilter2D contactFilter;
    #endregion

    #region private fields
    EdgeCollider2D edgeCollider;
    LineRenderer lineRenderer;
    List<Collider2D> colliders = new();
    #endregion

    void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        SetEdgeCollider();
        HandleDetection();
    }

    void HandleDetection()
    {
        if (Physics2D.OverlapCollider(edgeCollider, contactFilter, colliders) < 0) return;

        for (int i = 0; i < colliders.Count; i++)
        {
            if (!colliders[i].TryGetComponent(out LimbSubject _limbTarget)) continue;

            if (_limbTarget == ownStatusManager) continue;

            if (_limbTarget.ownStatusManager == ownStatusManager) continue;

            _limbTarget.AddDamage(tentacleEffects.damagePerInstance);
            _limbTarget.ownStatusManager.ApplyTentacle(tentacleEffects);
        }
    }

    void SetEdgeCollider()
    {
        List<Vector2> edges = new();
        AddPoints(edges, 0);
        for (int i = 0; i < lineRenderer.positionCount / pointsDividedBy; i++)
        {
            AddPointsDivided(edges, i);
        }
        AddPoints(edges, lineRenderer.positionCount - 1);

        edgeCollider.SetPoints(edges);
    }

    void AddPointsDivided(List<Vector2> edges, int i)
    {
        Vector2 lineRendererPoint = transform.InverseTransformPoint(lineRenderer.GetPosition(i * pointsDividedBy));
        edges.Add(lineRendererPoint);
    }

    void AddPoints(List<Vector2> edges, int i)
    {
        Vector2 lineRendererPoint = transform.InverseTransformPoint(lineRenderer.GetPosition(i));
        edges.Add(lineRendererPoint);
    }

    [Serializable]
    public struct TentacleEffects
    {
        public float damagePerInstance;
        public float stunPerInstance;
    }

    void OnEnable()
    {
        edgeCollider.enabled = true;
    }
    void OnDisable()
    {
        edgeCollider.enabled = false;
    }
}