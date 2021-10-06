using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game start and Game over properties
    private bool _isGameStart;
    private bool _isGameOver;
    private bool _isGameEntry;

    // Scene manager
    public GameObject sceneManager;
    private ScenesManager _sceneManagerScript;

    // Score manager
    public GameObject scoreManager;
    private ScoreManager _scoreManagerScript;

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

        // Initialize scene manager
        _sceneManagerScript = sceneManager.GetComponent<ScenesManager>();
        // Initialize score manager
        _scoreManagerScript = scoreManager.GetComponent<ScoreManager>();

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
        _scoreManagerScript.StartCountScore();
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
}
