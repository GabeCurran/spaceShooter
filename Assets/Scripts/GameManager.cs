using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;  // Reference to the enemy prefab

    void Start()
    {
        // Spawn an initial enemy when the game starts
        SpawnEnemy();
    }

    // Spawns a new enemy at a random position
    public void SpawnEnemy()
    {
        Vector3 randomPosition = GetRandomSpawnPosition();
        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    }

    // Get a random spawn position within the camera's view boundaries
    private Vector3 GetRandomSpawnPosition()
    {
        // Get the camera's orthographic size and aspect ratio to calculate map width and height
        float mapHeight = Camera.main.orthographicSize * 2; // Full height of the camera view
        float mapWidth = mapHeight * Camera.main.aspect;     // Full width based on aspect ratio

        // Randomly position within the map boundaries
        float randomX = Random.Range(-mapWidth / 2, mapWidth / 2);
        float randomY = Random.Range(-mapHeight / 2, mapHeight / 2);

        return new Vector3(randomX, randomY, 0);  // z=0 for 2D games
    }
}
