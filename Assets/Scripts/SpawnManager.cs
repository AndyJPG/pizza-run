using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject[] scenePrefabs;
    public GameObject[] furiousPrefabs;

    private float obstacleSpawnInterval;

    // Level and last update time
    private float level = 0;
    private float levelUpdateInterval = 10.0f;

    private PlayerController playerControllerScript;
    private Vector3 spawnPos = new Vector3(45, 0, 4.8f);
    private Vector3 sceneSpawnPos = new Vector3(45, 0, 10);
    private float startDelay = 1f;
    //private float repeatRate = 2f;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

        obstacleSpawnInterval = Random.Range(3.0f, 5.0f);
        Invoke("SpawnObstacle", startDelay);

        // Update level every 10 second
        Invoke("LevelUpdate", levelUpdateInterval);

        // Spawn Scene
        Invoke("SpawnScene", startDelay + 2f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LevelUpdate()
    {
        if (!playerControllerScript.isGameOver && !playerControllerScript.onGameEntry)
        {
            if (level < 3)
            {
                level += 0.5f;
                Debug.Log("Current level: " + level);
            }

            Invoke("LevelUpdate", levelUpdateInterval);
        }
    }

    private void SpawnObstacle()
    {
        if (!playerControllerScript.isGameOver && !playerControllerScript.onGameEntry)
        {
            GameObject selectedObstacle;

            if (playerControllerScript.onFuriousMode)
            {
                int obstacleIndex = Random.Range(0, furiousPrefabs.Length);
                selectedObstacle = furiousPrefabs[obstacleIndex];
            } else
            {
                int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
                selectedObstacle = obstaclePrefabs[obstacleIndex];
            }

            Instantiate(selectedObstacle, spawnPos, transform.rotation);

            obstacleSpawnInterval = Random.Range(1.5f, 5.0f - level);
        }

        //Debug.Log("Next spawn time: " + obstacleSpawnInterval);
        Invoke("SpawnObstacle", obstacleSpawnInterval);
    }

    private void SpawnScene()
    {
        if (!playerControllerScript.isGameOver && !playerControllerScript.onGameEntry)
        {
            GameObject selectedScene = scenePrefabs[Random.Range(0, scenePrefabs.Length)];

            Instantiate(selectedScene, sceneSpawnPos, transform.rotation);
        }

        //Debug.Log("Next spawn time: " + obstacleSpawnInterval);
        Invoke("SpawnScene", Random.Range(1.5f, 5f));
    }
}
