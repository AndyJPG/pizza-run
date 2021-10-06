using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Player script
    public GameObject player;
    private PlayerController _playerControllerScript;
    private Text _scoreText;
    // Start is called before the first frame update
    void Start()
    {
        _playerControllerScript = player.GetComponent<PlayerController>();
        _scoreText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text ="Score: " + _playerControllerScript.score.ToString();
    }
}
