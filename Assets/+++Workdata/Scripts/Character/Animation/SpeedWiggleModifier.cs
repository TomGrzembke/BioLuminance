using UnityEngine;
using UnityEngine.AI;

public class SpeedWiggleModifier : MonoBehaviour
{

    #region serialized fields
    [SerializeField] NavMeshAgent agent;
    [SerializeField] TentacleBehavior[] tentacleBehaviorsToModify;
    [SerializeField] float dividedBy = 10;
    #endregion

    void Update()
    {
        ModifyWiggle(agent.speed);
    }

    void ModifyWiggle(float speed)
    {
        float modifier = speed / dividedBy;
        for (int i = 0; i < tentacleBehaviorsToModify.Length; i++)
        {
            tentacleBehaviorsToModify[i].SetMod_WiggleMagnitudeParameters(modifier);
        }
    }
}