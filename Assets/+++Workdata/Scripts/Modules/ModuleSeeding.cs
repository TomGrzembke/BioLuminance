using UnityEngine;

/// <summary> Saves the current game Seed and initializes the level if it isnt 0 </summary>
public class ModuleSeeding : MonoBehaviour
{
    #region serialized fields
    [Tooltip("Randomizes the level at start when 0, loads level with given seed when not 0")]
    [SerializeField] int currentSeed;

    #endregion

    #region private fields

    #endregion

    void Awake()
    {
        if (currentSeed == 0)
            GenerateRandomSeed();
        else
            SetRandomSeed(currentSeed);
    }

    public void GenerateRandomSeed()
    {
        currentSeed = (int)System.DateTime.Now.Ticks;

        Random.InitState(currentSeed);
    }

    public void SetRandomSeed(int seed)
    {
        currentSeed = seed;

        Random.InitState(currentSeed);
    }
}