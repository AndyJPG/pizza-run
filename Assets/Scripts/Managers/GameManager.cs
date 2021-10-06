using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game start and Game over properties
    public bool _isGameStart;
    public bool _isGameOver;
    public bool _isGameEntry;

    // Scene manager
    public GameObject sceneManager;
    private ScenesManager _sceneManagerScript;

    // Score
    private int _score;

    // Dash mode
    private bool _dashMode;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        _isGameStart = false;
        _isGameOver = false;
        _isGameEntry = false;
        _score = 0;

        // Initialize scene manager
        _sceneManagerScript = sceneManager.GetComponent<ScenesManager>();
    }

    // Is game start
    public bool IsGameStart
    {
        get { return _isGameStart; }
    }

    // Is game over
    public bool IsGameOver
    {
        get { return _isGameOver; }
    }

    // Is on game entry animation
    public bool IsGameEntry
    {
        get { return _isGameEntry; }
    }

    // Get score
    public int Score
    {
        get { return _score; }
    }

    // Is play on dash mode
    public bool DashMode
    {
        get { return _dashMode; }
    }

    // Start game
    public void GameStart()
    {
        _isGameStart = true;
        _isGameEntry = true;
        UpdateScore();
    }

    // Game entry completed
    public void GameEntryComplete()
    {
        _isGameEntry = false;
    }

    // Game over
    public void GameOver()
    {
        _isGameStart = false;
        _isGameOver = true;
    }

    // Restart game
    public void GameRestart()
    {
        // Restart current active scene
        _sceneManagerScript.RestartCurrentScene();
    }

    // Enter dash mode
    public void EnterDashMode()
    {
        Debug.Log("Enter dash mode");
        _dashMode = true;
    }

    // Exit dash mode
    public void ExitDashMode()
    {
        Debug.Log("Exit dash mode");
        _dashMode = false;
    }

    // Update score every seconds
    private void UpdateScore()
    {
        Debug.Log("GAME MANAGER SCORE: " + _score);
        if (!_isGameOver)
        {
            _score += 1;

            if (_dashMode)
            {
                // If is in dash mode score update faster
                Invoke("UpdateScore", 0.5f);
            }
            else
            {
                Invoke("UpdateScore", 1f);
            }
        }
    }
}
