using System.Collections.Generic;
using UnityEngine;
using MyBox;
using Random = UnityEngine.Random;

public class CreatureSpawner : MonoBehaviour
{
    public SpawnType spawnType;
    
    public float numberToSpawn;
    Collider2D collider;
    [ConditionalField(nameof(spawnType), false, SpawnType.singleTypeSpawn)] public GameObject creatureToSpawn;
    [ConditionalField(nameof(spawnType), false, SpawnType.randomTypeSpawn)] public WeightedArray[] creaturesToSpawn;

    //TODO put this into CreatureLogic
    public Minimap minimap;
    
    public enum SpawnType
    {
        singleTypeSpawn,
        randomTypeSpawn,
    }

    void Awake()
    {
        collider = GetComponent<Collider2D>();
    }
    
    [ButtonMethod]
    void RandomSpawn()
    {
        SpawnCreatureRandom();
    }
    
    [ButtonMethod]
    void Spawn()
    {
        SpawnCreature();
    }

    void SpawnCreature()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector2 spawnPosition = GetRandomSpawnPosition(collider);
            Instantiate(creatureToSpawn, spawnPosition, Quaternion.identity);
        }
    }
    
    void SpawnCreatureRandom()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector2 spawnPosition = GetRandomSpawnPosition(collider);
            Instantiate(creaturesToSpawn[Random.Range(0, creaturesToSpawn.Length)].creatureToSpawnn, spawnPosition, Quaternion.identity);
        }
    }

    Vector2 GetRandomSpawnPosition(Collider2D spawnableAreaCollider)
    {
        Vector2 spawnposition = Vector2.zero;
        bool isSpawnPosValid = false;

        int attemptCount = 0;
        int maxAttempts = 200;

        int layerToNotSpawnOn = LayerMask.NameToLayer("Obstacle");

        while (!isSpawnPosValid && attemptCount < maxAttempts)
        {
            spawnposition = GetRandomPointInCollider(spawnableAreaCollider);
            Collider2D[] collider = Physics2D.OverlapCircleAll(spawnposition, minimap.indicatorSizeVec.x);

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
            {
                isSpawnPosValid = true;
            }

            attemptCount++;
        }

        if (!isSpawnPosValid)
        {
            Debug.Log("Could not find spawn position");
        }

        return spawnposition;
    }

    [ButtonMethod]
    Vector2 GetRandomPointInCollider(Collider2D collider2D)
    {
        Bounds collBounds = collider.bounds;

        Vector2 minBounds = new Vector2(collBounds.min.x, collBounds.min.y);
        Vector2 maxBounds = new Vector2(collBounds.max.x, collBounds.max.y);

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);

        return new Vector2(randomX, randomY);
    }
}

[System.Serializable]
public class WeightedArray
{
    public GameObject creatureToSpawnn;
    
    [Range(0f, 100f)]public float weight;
}