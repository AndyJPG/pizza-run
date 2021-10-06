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

    // Game physics
    private float _gravityModifier = 1.5f;

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

        // Set game physics
        Physics.gravity = new Vector3(0, -9.81f * _gravityModifier, 0);
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
        _dashMode = true;
    }

    // Exit dash mode
    public void ExitDashMode()
    {
        _dashMode = false;
    }

    // Update score every seconds
    private void UpdateScore()
    {
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

    // Add bonus score
    public void AddBonusScore(int bonus)
    {
        _score += bonus;
    }
}
