using MyBox;
using UnityEngine;

public class TestPoints : MonoBehaviour
{
    public Creatures creatures;

    [ButtonMethod]
    public void PercentOption()
    {
        PointSystem.Instance.CalculatePoints(creatures);
    }
}