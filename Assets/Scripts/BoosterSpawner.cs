using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    public GameObject boosterPrefab;
    public float spawnInterval = 5f;
    public Transform wallLeft;
    public Transform wallRight;
    public Transform wallTop;
    public Transform wallBottom;

    private float minX, maxX, minY, maxY;

    void Start()
    {
        CalculateSpawnArea();
        InvokeRepeating(nameof(SpawnBooster), 5f, spawnInterval);
    }

    void CalculateSpawnArea()
    {
        float wallWidth = wallLeft.GetComponent<BoxCollider2D>().bounds.size.x;
        float wallHeight = wallTop.GetComponent<BoxCollider2D>().bounds.size.y;

        minX = wallLeft.position.x + wallWidth / 2f;
        maxX = wallRight.position.x - wallWidth / 2f;
        minY = wallBottom.position.y + wallHeight / 2f;
        maxY = wallTop.position.y - wallHeight / 2f;
    }

    void SpawnBooster()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );
        Instantiate(boosterPrefab, spawnPos, Quaternion.identity);
    }
}
