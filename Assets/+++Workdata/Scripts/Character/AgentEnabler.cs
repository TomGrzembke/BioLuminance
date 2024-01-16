using UnityEngine;
using UnityEngine.AI;

public class AgentEnabler : MonoBehaviour
{
    #region serialized fields
    NavMeshAgent agent;
    #endregion

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.enabled = true;
    }
}