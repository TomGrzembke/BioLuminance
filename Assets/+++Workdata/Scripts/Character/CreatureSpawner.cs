using System;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using Random = UnityEngine.Random;

public class CreatureSpawner : MonoBehaviour
{
    public bool spawnOnStart;
    [SerializeField] private bool randomNumberToSpawn;

    [SerializeField, ConditionalField(nameof(randomNumberToSpawn), true)]
    float numberToSpawn;

    [SerializeField, ConditionalField(nameof(randomNumberToSpawn))]
    float minNumberToSpawn;

    [SerializeField, ConditionalField(nameof(randomNumberToSpawn))]
    float maxNumberToSpawn;

    [Separator] [SerializeField] Transform spawnInto;
    [SerializeField] WeightedArray[] creaturesToSpawn;
    [SerializeField] List<GameObject> instantiatedObjects;

    Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();

        if (spawnOnStart)
            SpawnRandomCreatures();
    }

    private void Start()
    {
        _collider.isTrigger = true;
    }

    private void OnValidate()
    {
        foreach (WeightedArray weightedArray in creaturesToSpawn)
        {
            weightedArray.characterName = weightedArray._creatureToSpawn.name;
        }
    }

    [ButtonMethod]
    void SpawnRandomCreatures()
    {
        float setNumberToSpawn = new float();

        if (!randomNumberToSpawn)
            setNumberToSpawn = numberToSpawn;
        else if (randomNumberToSpawn)
            setNumberToSpawn = Random.Range(minNumberToSpawn, maxNumberToSpawn);

        for (int i = 0; i < setNumberToSpawn; i++)
        {
            float totalWeight = 0f;
            foreach (WeightedArray weightedArrays in creaturesToSpawn)
            {
                totalWeight += weightedArrays._weight;
            }

            float rand = Random.Range(0, totalWeight);

            float cummChance = 0f;
            foreach (WeightedArray weightedArrays in creaturesToSpawn)
            {
                cummChance += weightedArrays._weight;
                if (rand <= cummChance)
                {
                    Vector2 spawnPosition = GetRandomSpawnPosition(_collider);
                    GameObject instantiated = Instantiate(weightedArrays._creatureToSpawn, spawnPosition,
                        Quaternion.identity);
                    instantiated.transform.SetParent(spawnInto);
                    instantiatedObjects.Add(instantiated);
                    break;
                }
            }
        }
    }

    Vector2 GetRandomSpawnPosition(Collider2D spawnableAreaCollider)
    {
        Vector2 spawnPosition = Vector2.zero;
        bool isSpawnPosValid = false;

        int attemptCount = 0;
        int maxAttempts = 200;

        int layerToNotSpawnOn = LayerMask.NameToLayer("Obstacle");

        float radius = 0f;

        foreach (WeightedArray weightedArray in creaturesToSpawn)
            radius = weightedArray._radius;

        while (!isSpawnPosValid && attemptCount < maxAttempts)
        {
            spawnPosition = GetRandomPointInCollider(spawnableAreaCollider);

            Collider2D[] collider = Physics2D.OverlapCircleAll(spawnPosition, radius);

            bool isInvalidCollision = false;
            foreach (Collider2D colliders in collider)
            {
                if (colliders.gameObject.layer == layerToNotSpawnOn)
                {
                    isInvalidCollision = true;
                    break;
                }
            }

            if (!isInvalidCollision)
                isSpawnPosValid = true;

            attemptCount++;
        }

        if (!isSpawnPosValid)
        {
            Debug.Log("Could not find spawn position");
            Destroy(instantiatedObjects[0 - 1000]);
            instantiatedObjects.Clear();
        }

        return spawnPosition;
    }

    Vector2 GetRandomPointInCollider(Collider2D collider2D)
    {
        Bounds collBounds = _collider.bounds;

        Vector2 minBounds = new Vector2(collBounds.min.x, collBounds.min.y);
        Vector2 maxBounds = new Vector2(collBounds.max.x, collBounds.max.y);

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);

        return new Vector2(randomX, randomY);
    }
}

[Serializable]
public class WeightedArray
{
    public GameObject _creatureToSpawn;

    [Tooltip("The Radius in which the creature is spawned x amount away from a wall")]
    public float _radius = 2f;

    [Tooltip("This defines the probability in which a creature is spawned")] [Range(0, 100)]
    public float _weight = 100f;

    [HideInInspector] public string characterName;
}