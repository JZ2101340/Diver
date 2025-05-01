using UnityEngine;

public class SeaCreatureSpawner : MonoBehaviour
{
    public GameObject[] seaCreaturePrefabs;
    public int numberOfCreatures;
    public float safeSpawnRadius = 3f;
    public Transform diverTransform;

    public BoxCollider2D topCollider;
    public BoxCollider2D bottomCollider;
    public BoxCollider2D leftCollider;
    public BoxCollider2D rightCollider;

    void Start()
    {
        SpawnCreatures();
    }

    Vector2 GetRandomSpawnPosition()
    {
        float left = leftCollider.bounds.max.x;
        float right = rightCollider.bounds.min.x;
        float top = topCollider.bounds.min.y;
        float bottom = bottomCollider.bounds.max.y;

        float x = Random.Range(left, right);
        float y = Random.Range(bottom, top);

        return new Vector2(x, y);
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
                spawnPosition = GetRandomSpawnPosition();
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
