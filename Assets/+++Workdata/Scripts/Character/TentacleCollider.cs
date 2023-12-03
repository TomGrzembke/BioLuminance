using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class TentacleCollider : MonoBehaviour
{
    #region serialized fields
    [SerializeField] int pointsDividedBy = 3;
    #endregion

    #region private fields
    EdgeCollider2D edgeCollider;
    LineRenderer lineRenderer;
    #endregion

    void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        SetEdgeCollider();
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
}