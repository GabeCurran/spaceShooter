using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance;
    public int score;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        // Initialize the singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate ScoreControllers
        }

        // Initialize the score display
        UpdateScoreText();
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
