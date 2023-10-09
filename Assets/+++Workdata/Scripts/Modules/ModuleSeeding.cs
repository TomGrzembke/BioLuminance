using UnityEngine;

/// <summary> Saves the current game Seed and initializes the level if it isnt 0 </summary>
public class ModuleSeeding : MonoBehaviour
{
    #region serialized fields
    [Tooltip("Randomizes the level at start when 0, loads level with given seed when not 0")]
    [SerializeField] int seed;

    #endregion

    #region private fields

    #endregion

    void Awake()
    {
        if (seed == 0)
            GenerateRandomSeed();
        else
            SetRandomSeed(seed);
    }

    void GenerateRandomSeed()
    {
        seed = (int)System.DateTime.Now.Ticks;
        Random.InitState(seed);
    }

    void SetRandomSeed(int newSeed)
    {
        seed = newSeed;
        Random.InitState(seed);
    }
}