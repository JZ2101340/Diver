using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    public GameObject boosterPrefab;
    public int numberOfBoosters;

    public BoxCollider2D wallLeft;
    public BoxCollider2D wallRight;
    public BoxCollider2D wallTop;
    public BoxCollider2D wallBottom;

    private float minX, maxX, minY, maxY;

    void Start()
    {
        CalculateSpawnArea();
        SpawnMultipleBoosters();
    }

    void CalculateSpawnArea()
    {
        minX = wallLeft.bounds.max.x;
        maxX = wallRight.bounds.min.x;
        minY = wallBottom.bounds.max.y;
        maxY = wallTop.bounds.min.y;
    }

    void SpawnMultipleBoosters()
    {
        for (int i = 0; i < numberOfBoosters; i++)
        {
            Vector2 spawnPos = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            Instantiate(boosterPrefab, spawnPos, Quaternion.identity);
        }
    }
}
