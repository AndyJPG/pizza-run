using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Game manager
    public GameObject gameManager;
    private GameManager _gameManagerScript;

    // Score
    private int _score;

    private void Start()
    {
        // Initialize score
        _score = 0;

        // Initialize game manager
        _gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    // Get score
    public int Score
    {
        get { return _score; }
    }

    // Update score every seconds
    public void StartCountScore()
    {
        if (!_gameManagerScript.IsGameOver)
        {
            _score += 1;

            if (_gameManagerScript.DashMode)
            {
                // If is in dash mode score update faster
                Invoke("StartCountScore", 0.5f);
            }
            else
            {
                Invoke("StartCountScore", 1f);
            }
        }
    }

    // Add bonus score
    public void AddBonusScore(int bonus)
    {
        _score += bonus;
    }
}
