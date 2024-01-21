using System.Collections;
using MyBox;
using UnityEngine;

public class PointSubject : MonoBehaviour
{
    [SerializeField] StatusManager statusManager;
    [SerializeField] Creatures creatures;
    public float points;

    IEnumerator Start()
    {
        yield return null;
        creatures = statusManager.CreatureType;
        Points();
    }

    public void Points()
    {
        PointSystem.Instance.SetCreatureDnaStats(creatures, points);
    }

    [ButtonMethod]
    public void PercentOption()
    {
        PointSystem.Instance.CalculatePoints(creatures);
    }
}