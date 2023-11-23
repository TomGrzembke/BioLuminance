using System.Collections.Generic;
using UnityEngine;

public class SpeedSubject : MonoBehaviour
{
    #region serialized fields

    [SerializeField] float speed;
    public float Speed => speed;
    [SerializeField] float maxSpeed;
    static readonly List<float> SpeedModifier = new List<float>();
    #endregion

    #region private fields

    #endregion

}