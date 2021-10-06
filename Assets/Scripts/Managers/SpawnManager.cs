using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Game manager
    public GameObject gameManager;
    private GameManager _gameManagerScript;
    
    // Prefabs
    public GameObject[] obstaclePrefabs;
    public GameObject[] scenePrefabs;

    // Level and last update time
    private float level = 0;
    private float levelUpdateInterval = 10.0f;

    // Spawn positions
    private Vector3 spawnPos = new Vector3(45, 0, 4.8f);
    private Vector3 sceneSpawnPos = new Vector3(45, 0, 10);
    private Vector3 carSceneSpawnPos = new Vector3(45, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        // Initilize game manager 
        _gameManagerScript = gameManager.GetComponent<GameManager>();

        // Spawn obstacles
        Invoke("SpawnObstacle", 1f);

        // Update level every 10 second
        Invoke("LevelUpdate", 1f);

        // Spawn Scene
        Invoke("SpawnScene", 1f);
    }

    // Level up by increasing speed
    private void LevelUpdate()
    {
        // Start tracking level update
        if (_gameManagerScript.IsGameStart && !_gameManagerScript.IsGameEntry && level < 3)
        {
            level += 0.5f;
            Debug.Log("Current level: " + level);
            Invoke("LevelUpdate", levelUpdateInterval);
        } 
        else if (!_gameManagerScript.IsGameOver)
        {
            Invoke("LevelUpdate", 1.0f);
        }
    }

    // Spawn obstacles
    private void SpawnObstacle()
    {
        if (_gameManagerScript.IsGameStart && !_gameManagerScript.IsGameEntry)
        {
            GameObject selectedObstacle;

            int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
            selectedObstacle = obstaclePrefabs[obstacleIndex];

            Instantiate(selectedObstacle, spawnPos, transform.rotation);

            // Spawn quicker at higher level
            float obstacleSpawnInterval = Random.Range(1.5f, 5.0f - level);
            Invoke("SpawnObstacle", obstacleSpawnInterval);
        } 
        else if (!_gameManagerScript.IsGameOver)
        {
            Invoke("SpawnObstacle", 1f);
        }
    }

    // Spawn scenes
    private void SpawnScene()
    {
        if (_gameManagerScript.IsGameStart && !_gameManagerScript.IsGameEntry)
        {
            GameObject selectedScene = scenePrefabs[Random.Range(0, scenePrefabs.Length)];

            Instantiate(selectedScene, sceneSpawnPos, transform.rotation);
            Invoke("SpawnScene", Random.Range(1.5f, 5f));
        }
        else if (!_gameManagerScript.IsGameOver)
        {
            Invoke("SpawnScene", 1f);
        }
    }
}
