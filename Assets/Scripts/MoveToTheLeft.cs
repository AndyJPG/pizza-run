using System;
using UnityEngine;

public class MoveToTheLeft : MonoBehaviour
{
    // Game manager script
    private GameManager _gameManagerScript;
    // Player script
    private PlayerController _playerControllerScript;

    // Move speed and boundry
    private float _moveSpeed = 20.0f;
    private float _leftBoundry = -15f;
    private string[] _ofBoundaryTags = { "Obstacle", "Scene" };

    // Start is called before the first frame update
    void Start()
    {
        // Initialize game manager script
        _gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Initialize player script
        _playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManagerScript.IsGameStart && !_gameManagerScript.IsGameOver && !_gameManagerScript.IsGameEntry)
        {
            OnMove();
            DestoryOutOfBoundary();
        }
    }

    // Moving object
    private void OnMove()
    {
        if (_playerControllerScript.dashMode)
        {
            transform.Translate(Vector3.left * Time.deltaTime * _moveSpeed * 2);
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * _moveSpeed);
        }
    }

    // Destory object when it went off the boundary
    private void DestoryOutOfBoundary()
    {
        if (Array.IndexOf(_ofBoundaryTags, gameObject.tag) > -1)
        {
            if (transform.position.x <= _leftBoundry)
            {
                // Destroy obstacle and background scene
                Destroy(gameObject);
            }
        }
    }
}
