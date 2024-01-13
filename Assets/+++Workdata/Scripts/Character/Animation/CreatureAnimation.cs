using UnityEngine;
using UnityEngine.AI;

public class CreatureAnimation : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;
    #endregion

    #region private fields
    #endregion

    void Update()
    {
        anim.SetFloat("speed", agent.velocity.magnitude);
        print(agent.velocity.magnitude);
    }

}