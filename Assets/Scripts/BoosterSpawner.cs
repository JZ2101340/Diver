using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    public GameObject boosterPrefab;
    public float spawnInterval;
    public int maxBoosters; // Number of boosters to spawn
    public Vector2 spawnAreaMin, spawnAreaMax;

    void Start()
    {
        InvokeRepeating("SpawnBooster", 5f, spawnInterval);
    }

    void SpawnBooster()
    {
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );
        Instantiate(boosterPrefab, spawnPosition, Quaternion.identity);
    }
}
