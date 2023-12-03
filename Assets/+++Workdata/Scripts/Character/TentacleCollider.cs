using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class TentacleCollider : MonoBehaviour
{
    #region serialized fields

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
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 lineRendererPoint = lineRenderer.GetPosition(i);
            edges.Add(new(lineRendererPoint.x, lineRendererPoint.y));
            edges[i] = transform.InverseTransformPoint(edges[i]);;
        }

        edgeCollider.SetPoints(edges);
    }
}