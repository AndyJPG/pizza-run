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

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        _isGameStart = false;
        _isGameOver = false;
        _isGameEntry = false;

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

    // Start game
    public void GameStart()
    {
        _isGameStart = true;
        _isGameEntry = true;
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
}
