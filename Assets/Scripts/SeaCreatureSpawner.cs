//using UnityEngine;

//public class SeaCreatureSpawner : MonoBehaviour
//{
//    public GameObject[] seaCreaturePrefabs; // Different sea creature prefabs
//    public int numberOfCreatures; // Adjust per difficulty
//    public float minSpacing = 2.0f; // Minimum distance between creatures

//    private Vector2 spawnAreaMin;
//    private Vector2 spawnAreaMax;
//    private Camera mainCamera;

//    void Start()
//    {
//        mainCamera = Camera.main;
//        CalculateSpawnArea();
//        SpawnCreatures();
//    }

//    void CalculateSpawnArea()
//    {
//        float camHeight = 2f * mainCamera.orthographicSize;
//        float camWidth = camHeight * mainCamera.aspect;

//        spawnAreaMin = new Vector2(-camWidth / 2, -camHeight / 2);
//        spawnAreaMax = new Vector2(camWidth / 2, camHeight / 2);
//    }

//    void SpawnCreatures()
//    {
//        for (int i = 0; i < numberOfCreatures; i++)
//        {
//            Vector2 spawnPosition;
//            bool validPosition;
//            int maxAttempts = 10;
//            int attempts = 0;

//            do
//            {
//                spawnPosition = new Vector2(
//                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
//                    Random.Range(spawnAreaMin.y, spawnAreaMax.y)
//                );

//                validPosition = CheckValidSpawn(spawnPosition);
//                attempts++;
//            } while (!validPosition && attempts < maxAttempts);

//            int randomIndex = Random.Range(0, seaCreaturePrefabs.Length);
//            GameObject chosenCreature = seaCreaturePrefabs[randomIndex];

//            Instantiate(chosenCreature, spawnPosition, Quaternion.identity);
//        }
//    }

//    bool CheckValidSpawn(Vector2 position)
//    {
//        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(position, minSpacing);
//        return nearbyObjects.Length == 0;
//    }
//}
using UnityEngine;

public class SeaCreatureSpawner : MonoBehaviour
{
    public GameObject[] seaCreaturePrefabs;
    public Vector2 spawnAreaMin, spawnAreaMax;
    public int numberOfCreatures;
    public float safeSpawnRadius = 2f;
    private Camera mainCamera;
    public Transform diverTransform;  // Assign in Inspector

    void Start()
    {
        mainCamera = Camera.main;
        CalculateSpawnArea();
        SpawnCreatures();
    }

    void CalculateSpawnArea()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        spawnAreaMin = new Vector2(-cameraWidth / 2, -cameraHeight / 2);
        spawnAreaMax = new Vector2(cameraWidth / 2, cameraHeight / 2);
    }

    void SpawnCreatures()
    {
        for (int i = 0; i < numberOfCreatures; i++)
        {
            Vector2 spawnPosition;
            bool validPosition;
            int maxAttempts = 10;
            int attempts = 0;

            do
            {
                spawnPosition = new Vector2(
                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y)
                );

                validPosition = CheckValidSpawn(spawnPosition);
                attempts++;
            } while (!validPosition && attempts < maxAttempts);

            if (validPosition)
            {
                int randomIndex = Random.Range(0, seaCreaturePrefabs.Length);
                Instantiate(seaCreaturePrefabs[randomIndex], spawnPosition, Quaternion.identity);
            }
        }
    }

    bool CheckValidSpawn(Vector2 position)
    {
        // Prevent overlapping
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.5f);
        bool notOverlapping = colliders.Length == 0;

        // ? Prevent spawning too close to diver
        bool safeFromDiver = diverTransform == null || Vector2.Distance(position, diverTransform.position) > safeSpawnRadius;

        return notOverlapping && safeFromDiver;
    }
}
