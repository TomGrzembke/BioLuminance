using UnityEngine;

/// <summary> Saves the current game Seed and initializes the level if it isnt 0 </summary>
public class ModuleSeeding : MonoBehaviour
{
    #region serialized fields
    [SerializeField] string currentSeed;

    #endregion

    #region private fields

    #endregion

    void Awake()
    {
        if (currentSeed == "")
            GenerateRandomSeed();
        else
            SetRandomSeed(currentSeed);
    } 

    public void GenerateRandomSeed()
    {
        int tempSeed = (int)System.DateTime.Now.Ticks;
        currentSeed = tempSeed.ToString();

        Random.InitState(tempSeed);
    }

    public void SetRandomSeed(string seed = "")
    {
        currentSeed = seed;
        int tempSeed = 0;

        tempSeed = int.Parse(seed);

        Random.InitState(tempSeed);
    }

    public void SetRandomSeed(int seed)
    {
        currentSeed = seed.ToString();
        int tempSeed = 0;

        Random.InitState(tempSeed);
    }

}