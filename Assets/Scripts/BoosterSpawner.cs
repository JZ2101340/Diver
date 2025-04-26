using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    //data below acts as model
    public GameObject boosterPrefab; 
    public float spawnInterval;
    public int maxBoosters; 
    public Vector2 spawnAreaMin, spawnAreaMax;

    //start and spawnBooster functions below acts as controller

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
        Instantiate(boosterPrefab, spawnPosition, Quaternion.identity); // view, where boosters are instantiated 
    }
}

