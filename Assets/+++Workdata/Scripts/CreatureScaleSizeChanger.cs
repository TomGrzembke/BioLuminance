using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreatureScaleSizeChanger : MonoBehaviour
{
    float randomSpriteScale;
    [SerializeField] MinMaxFloat minMaxFloat = new(.7f,1.5f);

    void Awake()
    {
        randomSpriteScale = Random.Range(minMaxFloat.Min, minMaxFloat.Max);
    }

    void Start()
    {
        gameObject.transform.localScale = new Vector3(randomSpriteScale, randomSpriteScale);
    }
}