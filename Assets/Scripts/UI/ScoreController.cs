using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance;
    public int score;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        // Ensure only one instance of the ScoreController exists
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);  // Optional: keep the ScoreController across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    public static void AddScore(int amount)
    {
        if (Instance != null)
        {
            Instance.score += amount;
            Instance.UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}
