using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public bool gameStart;
    private Button startButton;
    private Button restartButton;
    private CanvasGroup canvasGroup;
    private float fadeAmount = 1f;

    private PlayerController playerControllerScript;

    private void Start()
    {
        // Get start button
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);

        // Get restart button
        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(RestartGame);
        restartButton.gameObject.SetActive(false);

        // Get canvas group
        canvasGroup = gameObject.GetComponent<CanvasGroup>();

        // Get player controller script
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

        // Initialize game
        gameStart = false;
    }

    void Update()
    {
        if (gameStart && !playerControllerScript.isGameOver)
        {
            HideMenu();
        }

        if (playerControllerScript.isGameOver)
        {
            restartButton.gameObject.SetActive(true);
            startButton.gameObject.SetActive(false);
            ShowMenu();
        }
    }

    private void StartGame()
    {
        Debug.Log("Start Game");
        gameStart = true;
    }

    private void HideMenu()
    {
        if (fadeAmount > 0)
        {
            fadeAmount = fadeAmount - (1f * Time.deltaTime);
            canvasGroup.alpha = fadeAmount;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void ShowMenu()
    {
        if (fadeAmount < 1)
        {
            fadeAmount = fadeAmount + (1f * Time.deltaTime);
            canvasGroup.alpha = fadeAmount;
            canvasGroup.blocksRaycasts = true;
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
