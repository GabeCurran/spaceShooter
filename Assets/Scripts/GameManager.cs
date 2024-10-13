using UnityEngine;
using UnityEngine.SceneManagement;  // Import for scene management

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Singleton instance
    public GameObject enemyPrefab;  // Reference to the enemy prefab
    public GameObject bossPrefab;   // Reference to the boss prefab

    void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate GameManager instances
        }
    }

    void Start()
    {
    }

    void Update()
    {
        // If the player presses R, reload the game scene when it's not already the active scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene currentScene = SceneManager.GetActiveScene();

            // Only reload if it's not already the "MainScene"
            if (currentScene.name != "MainScene")
            {
                SceneManager.LoadScene("MainScene");
            }
        }
        // If there are no enemies left, and the player exists, spawn a new enemy
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && PlayerShip.Instance != null){
            SpawnEnemy();
        }
    }

    // Spawns a new enemy at a random position
    public void SpawnEnemy()
    {
        Vector3 randomPosition = GetRandomSpawnPosition();
        if (ScoreController.Instance != null && ScoreController.Instance.score == 500)
        {
            Instantiate(bossPrefab, randomPosition, Quaternion.identity);
        }
        else
        {
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }

    // Get a random spawn position within the camera's view boundaries
    private Vector3 GetRandomSpawnPosition()
    {
        // Get the camera's orthographic size and aspect ratio to calculate map width and height
        float mapHeight = Camera.main.orthographicSize * 2;  // Full height of the camera view
        float mapWidth = mapHeight * Camera.main.aspect;     // Full width based on aspect ratio

        // Randomly position within the map boundaries
        float randomX = Random.Range(-mapWidth / 2, mapWidth / 2);
        float randomY = Random.Range(-mapHeight / 2, mapHeight / 2);

        return new Vector3(randomX, randomY, 0);  // z=0 for 2D games
    }

    public void GameOver(bool playerWon)
    {
        if (playerWon)
        {
            SceneManager.LoadScene("WinScene");
        }
        else
        {
            SceneManager.LoadScene("DeathScene");
        }
    }
}
