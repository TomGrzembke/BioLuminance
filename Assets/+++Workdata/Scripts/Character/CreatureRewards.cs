using UnityEngine;

public class CreatureRewards : MonoBehaviour
{
    #region serialized fields
    public GameObject PassiveReward => passiveReward; 
    [SerializeField] GameObject passiveReward;
    public GameObject ActiveReward => activeReward;
    [SerializeField] GameObject activeReward;
    #endregion
}