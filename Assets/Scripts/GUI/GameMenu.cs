using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    // Game Manager
    public GameObject gameManager;
    private GameManager _gameManagerScript;

    // Buttons
    public Button startButton;
    public Button restartButton;

    // Menu and menu fade rate
    private CanvasGroup _menu;
    private float fadeAmount;

    private void Start()
    {
        // Initialize game manager
        _gameManagerScript = gameManager.GetComponent<GameManager>();

        // Add start event listner
        startButton.onClick.AddListener(StartGame);

        // Add restart event listener and hide restart button
        restartButton.onClick.AddListener(RestartGame);
        restartButton.gameObject.SetActive(false);

        // Get canvas group and set fading rate
        _menu = gameObject.GetComponent<CanvasGroup>();
        fadeAmount = 1f;
    }

    void Update()
    {
        // Hide menu when game started
        if (_gameManagerScript.IsGameStart && !_gameManagerScript.IsGameOver)
        {
            HideMenu();
        }

        // Show menu when game over
        if (_gameManagerScript.IsGameOver)
        {
            ShowMenu();
        }
    }

    // Start game
    private void StartGame()
    {
        _gameManagerScript.GameStart();
    }

    // Restart game
    private void RestartGame()
    {
        _gameManagerScript.GameRestart();
    }

    // Fade out effect
    private void HideMenu()
    {
        if (fadeAmount > 0)
        {
            fadeAmount = fadeAmount - (1f * Time.deltaTime);
            _menu.alpha = fadeAmount;
            _menu.blocksRaycasts = false;
        }
        // Set menu active false
        startButton.gameObject.SetActive(false);
    }

    // Fade in effect
    private void ShowMenu()
    {
        if (fadeAmount < 1)
        {
            fadeAmount = fadeAmount + (1f * Time.deltaTime);
            _menu.alpha = fadeAmount;
            _menu.blocksRaycasts = true;
        }
        // Show restart button and hide start button
        restartButton.gameObject.SetActive(true);
    }
}
