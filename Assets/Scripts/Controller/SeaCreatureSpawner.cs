using UnityEngine;

public class SeaCreatureSpawner : MonoBehaviour
{
    public GameObject[] seaCreaturePrefabs;
    public int numberOfCreatures;
    public float safeSpawnRadius = 2f;
    public Transform diverTransform;

    public Transform wallLeft;
    public Transform wallRight;
    public Transform wallTop;
    public Transform wallBottom;

    private float minX, maxX, minY, maxY;

    void Start()
    {
        CalculateSpawnArea();
        SpawnCreatures();
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
                    Random.Range(minX, maxX),
                    Random.Range(minY, maxY)
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.5f);
        bool notOverlapping = colliders.Length == 0;

        bool safeFromDiver = diverTransform == null || Vector2.Distance(position, diverTransform.position) > safeSpawnRadius;

        return notOverlapping && safeFromDiver;
    }
}
