using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Player script
    public GameObject gameManager;
    private GameManager _gameManagerScript;

    // UI text
    private Text _scoreText;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        _gameManagerScript = gameManager.GetComponent<GameManager>();
        _scoreText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text ="Score: " + _gameManagerScript.Score.ToString();
    }
}
